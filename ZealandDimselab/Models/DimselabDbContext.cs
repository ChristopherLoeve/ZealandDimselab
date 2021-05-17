using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZealandDimselab.Models;

namespace ZealandDimselab.Models
{
    public class DimselabDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<BookingItem> BookingItems { get; set; }

        public DimselabDbContext()
        {
            
        }

        
        public DimselabDbContext(DbContextOptions<DimselabDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured) // If no options provided by DimselabDbContext constructor, use this:
            {
                //options.UseSqlServer(@"Server=tcp:dimselab.database.windows.net,1433;Initial Catalog=dimselabDb;Persist Security Info=False;User ID=dimselabadmin;Password=516zVIbTxK5T;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
                options.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ZealandDimselab; Integrated Security=True; Connect Timeout=30; Encrypt=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookingItem>().HasKey(bi => new { bi.BookingId, bi.ItemId });
        }
    }
}