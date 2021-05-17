using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ZealandDimselab.Models;

namespace ZealandDimselab.Services
{
    public class BookingService : GenericService<Booking>
    {
        private UserService UserService;
        private Notifications Notifications;

        public BookingService(IDbService<Booking> dbService) : base(dbService)
        {
        }

        public List<Booking> GetAllBookings()
        {
            List<Booking> bookings;
            using (var context = new DimselabDbContext())
            {
                bookings = context.Bookings
                    .Include(u => u.User)
                    .Include(i => i.BookingItems)
                    .ThenInclude(bi => bi.Item).ToList();
            }
            return bookings;
        }
        //public List<Booking> GetAllBookingsTest()
        //{
        //    return dbContext;
        //}

        public async Task<Booking> GetBookingByKeyAsync(int id)
        {
            return await GetObjectByKeyAsync(id);
        }

        public async Task AddBookingAsync(Booking booking)
        {
            string notifycation = $@"A new booking for {booking.ToString()} has been added";
            Notifications.StringNotifycations.Add(notifycation);
            await AddObjectAsync(booking);
        }

        public async Task DeleteBookingAsync(int id)
        {
            await DeleteObjectAsync(await GetBookingByKeyAsync(id));
        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            await UpdateObjectAsync(booking);
        }
        
        public List<Booking> GetBookingsByEmail(string email) // TODO Pretty sure this doesn't work
        {
            List<Booking> userBookings = new List<Booking>();
            foreach (Booking booking in GetAllBookings())
            {
                if (booking.User.Email.ToLower() == email.ToLower())
                {
                    userBookings.Add(booking);
                    
                }
            }
            return userBookings;
        }
    }
}
