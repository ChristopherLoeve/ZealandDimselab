using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ZealandDimselab.Models;
using ZealandDimselab.Services;

namespace ZealandDimselab
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();


            // DATABASE START //
            services.AddDbContext<DimselabDbContext>();
            services.AddTransient<IDbService<Item>, GenericDbService<Item>>();
            services.AddTransient<IDbService<User>, GenericDbService<User>>();
            services.AddTransient<IDbService<Category>, GenericDbService<Category>>();
            services.AddTransient<IDbService<Booking>, GenericDbService<Booking>>();
            services.AddTransient<IDbService<Notification>, GenericDbService<Notification>>();
            services.AddTransient<ItemDbService, ItemDbService>();
            services.AddTransient<BookingService, BookingService>();
            // DATABASE END //

            // SERVICES START //
            services.AddSingleton<UserService, UserService>();
            services.AddSingleton<ItemService, ItemService>();
            services.AddSingleton<BookingService, BookingService>();
            services.AddSingleton<NotificationService, NotificationService>();

            services.AddSingleton<CategoryService, CategoryService>();
            // SERVICES END //

            // SESSION START //
            services.AddSession(); // Giver mulighed for at gemme i brugerens cache.
            // SESSION END

            // AUTHENTICATION START //
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator", policy =>
                    policy.RequireClaim(ClaimTypes.Role, "admin"));
            });
            services.AddMvc()
                .AddRazorPagesOptions(options =>
                    {
                        options.Conventions.AuthorizeFolder("/Account");
                        options.Conventions.AllowAnonymousToPage("/Account/Login");

                        options.Conventions.AuthorizeFolder("/Items");
                        options.Conventions.AllowAnonymousToPage("/Items/AllItems");
                        options.Conventions.AllowAnonymousToPage("/Items/ItemDetails");

                        //options.Conventions.AuthorizeFolder("/BookingPages");
                    }
                );
            // AUTHENTICATION END //
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            // SESSION START //
            app.UseSession(); // Starter automatisk en session med brugeren.
            // SESSION END // 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
