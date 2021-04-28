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
using ZealandDimselab.MockData;
using ZealandDimselab.Models;
using ZealandDimselab.Repository;
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
            services.AddTransient<GenericDbService<Item>, GenericDbService<Item>>();
            services.AddTransient<IDbService<User>, GenericDbService<User>>();
            // DATABASE END //

            // SERVICES START //
            services.AddSingleton<UserService, UserService>();
            services.AddSingleton<IRepository<User>, MockUsers>(); // No need anymore
            services.AddSingleton<ItemService, ItemService>();
            // SERVICES END //


            // AUTHENTICATION START //
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
