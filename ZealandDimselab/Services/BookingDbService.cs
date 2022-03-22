using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZealandDimselab.Interfaces;
using ZealandDimselab.Models;

namespace ZealandDimselab.Services
{
    public class BookingDbService : GenericDbService<Booking>, IBookingDb
    {

        //public override async Task<IEnumerable<Booking>> GetObjectsAsync()
        //{
        //    List<Booking> bookings;
        //    using (var context = new DimselabDbContext())
        //    {
        //        bookings = await context.Bookings
        //            .Include(u => u.User)
        //            .Include(i => i.BookingItems)
        //            .ThenInclude(bi => bi.Item).ToListAsync();
        //    }
        //    return bookings;
        //}

        //public override async Task<Booking> GetObjectByKeyAsync(int id)
        //{
        //    Booking booking = new Booking();
        //    using (var context = new DimselabDbContext())
        //    {
        //        booking = await context.Bookings
        //            .Include(u => u.User)
        //            .Include(i => i.BookingItems)
        //            .Where(b => b.Id == id).FirstOrDefaultAsync();
        //    }

        //    return booking;
        //}
    }
}
