using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZealandDimselab.Models;
using ZealandDimselab.Services;

namespace ZealandDimselabTest
{
    [TestClass]
    public class BookingServiceTest
    {
        private BookingService bookingService;
        private IDbService<Booking> dbService;

        [TestInitialize]
        public void InitializeTest()
        {
            dbService = new BookingMockData<Booking>();
            bookingService = new BookingService(dbService);
        }

        //AddBookingTestCases
        //[TestMethod]
        //public async Task AddBookingAsync_CountAsync()
        //{
        //    var expectedCount = bookingService.GetAllBookings().Count + 1;

        //    await bookingService.AddBookingAsync(booking);

        //    var actualValueDb = (await dbService.GetObjectsAsync()).Count();
        //    var actualValueList = bookingService.GetAllBookings().Count;

        //    Assert.AreEqual(expectedCount, actualValueDb);
        //    Assert.AreEqual(expectedCount, actualValueList);
        //}

        [TestMethod]
        public async Task AddBookingAsync_AddBooking_IncrementCount()
        {
            ///////////////GAMLE MÅDE/////////////////////////////
            //// Arrange
            //var expectedCount = 4;

            //List<Item> Items = new List<Item>() { new Item("Raspery Pi", "minicomputer") };
            //User user = new User("Mikkel", "meilig@.com", "1234");

            //Booking booking = new Booking(Items, user, "Details", DateTime.Now, DateTime.Now);
            //await bookingService.AddBookingAsync(booking);

            //// Act
            //var actualCount = bookingService.GetAllBookings().ToList().Count;

            //// Assert
            //Assert.AreEqual(expectedCount, actualCount);
            ////////////////////////////////////////////////////////////////////////////////////
            
            // Arrange
            var expectedCount = dbService.GetObjectsAsync().Result.ToList().Count + 1;

            List<Item> Items = new List<Item>() { new Item("Thanoscopter", "peak comedy") };
            List<BookingItem> bookingItems = new List<BookingItem>();

            foreach (var item in Items) // Nyt
            {
                bookingItems.Add(new BookingItem() { Item = item });
            }
            User user = new User("Mikkel", "meilig@.com", "1234");

            Booking booking = new Booking(bookingItems, user, "Details", DateTime.Now, DateTime.Now);
            await bookingService.AddBookingAsync(booking);

            // Act
            var actualCount = dbService.GetObjectsAsync().Result.ToList().Count;

            // Assert
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public async Task UpdateBookingAsync_UpdateExsitingBooking_ReturnsUpdatedObject()
        {
            //// Arrange
            //Booking booking = await bookingService.GetBookingByKeyAsync(2);
            //List<Item> expecteItems = new List<Item>() { new Item("Batmobil", "luksusbil") };
            //User expecteUser = new User("daddycool", "batman@secret.com", "007Jamesbond");


            //// Act
            //booking.Items = expecteItems;
            //booking.User = expecteUser;
            //await bookingService.UpdateBookingAsync(booking);
            //Booking actualBooking = await bookingService.GetBookingByKeyAsync(2);

            //// Assert
            //Assert.AreEqual(expecteItems, actualBooking.Items);
            //Assert.AreEqual(expecteUser, actualBooking.User);

            // Arrange
            Booking booking = await bookingService.GetBookingByKeyAsync(2);          
            User expecteUser = new User("daddycool", "batman@secret.com", "007Jamesbond");
            List<Item> Items = new List<Item>() { new Item("Batmobil", "luksusbil") };
            List<BookingItem> expetedBookingItem = new List<BookingItem>();

            foreach (var item in Items) // Nyt
            {
                expetedBookingItem.Add(new BookingItem() { Item = item });
            }

            // Act
            booking.BookingItems = expetedBookingItem;
            booking.User = expecteUser;
            await bookingService.UpdateBookingAsync(booking);
            Booking actualBooking = await bookingService.GetBookingByKeyAsync(2);

            // Assert
            Assert.AreEqual(expetedBookingItem, actualBooking.BookingItems);
            Assert.AreEqual(expecteUser, actualBooking.User);
        }

        [TestMethod]
        public async Task GetBookingByIdAsync_ValidId_ReturnsBookingObject()
        {
            //    string expectedDetials = "Details";

            //    // Act
            //    Booking actualBooking = await bookingService.GetBookingByKeyAsync(3);

            //    // Assert
            //    Assert.AreEqual(expectedDetials, actualBooking.Details);

              string expectedDetials = "Details";

              // Act
              Booking actualBooking = await bookingService.GetBookingByKeyAsync(2);
              // Assert
              Assert.AreEqual(expectedDetials, actualBooking.Details);

        }

        [TestMethod]
        public async Task GetBookingByEmailAsync_ValidEmail_ReturnsBookingObject()
        {
            //Arrange
            string expectedEmail = "smelly@.com";
            string expectedBookingDetails = "skal bruges i morgen";
            List<BookingItem> expetedBookingItem = new List<BookingItem>();
            List<Item> Items = new List<Item>() {new Item("RC car", "fjernstyret bil")};

            foreach (var item in Items) // Nyt
            {
                expetedBookingItem.Add(new BookingItem() { Item = item });
            }

            Booking expecBooking = new Booking(expetedBookingItem, new User("Simon", "smelly@.com", "1234"), "skal bruges i morgen", DateTime.Now, DateTime.Now);
            List<Booking> expectedBookings = new List<Booking>() { expecBooking };

            // Act
            List<Booking> actualBookings = bookingService.GetBookingsByEmail(expectedEmail);

            // Assert
            Assert.AreEqual(expectedBookings, actualBookings);
        }


        public class BookingMockData<T> : IDbService<T> where T : class
        {
            private DimselabDbContext dbContext;

            public BookingMockData()
            {
                var options = new DbContextOptionsBuilder<DimselabDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;
                dbContext = new DimselabDbContext(options);
                LoadDatabase();
            }

            public async Task<IEnumerable<T>> GetObjectsAsync()
            {
                //return await dbContext.Set<T>().AsNoTracking().ToListAsync();
                List<Booking> booking = new List<Booking>();
                //dbContext.Bookings
                //    //.Include(b => b.User)
                //    //.Include(i => i.Items)
                //    //.Include(bd => bd.BookingDate)
                //    //.Include(rd => rd.ReturnDate)
                //    .Include(b => b).ToList();

                foreach (var Booking in dbContext.Bookings)
                {
                    booking.Add(Booking);
                }

                return (IEnumerable<T>)booking;
            }

            public async Task AddObjectAsync(T obj)
            {
                await dbContext.Set<T>().AddAsync(obj);
                await dbContext.SaveChangesAsync();
            }

            public async Task DeleteObjectAsync(T obj)
            {
                dbContext.Set<T>().Remove(obj);
                await dbContext.SaveChangesAsync();
            }

            public async Task UpdateObjectAsync(T obj)
            {
                dbContext.Set<T>().Update(obj);
                await dbContext.SaveChangesAsync();
            }

            public async Task<T> GetObjectByKeyAsync(int id)
            {
                return await dbContext.Set<T>().FindAsync(id);
            }

            private async Task LoadDatabase()
            {
                List<Item> item1 = new List<Item>() { new Item("RC car", "fjernstyret bil") };
                List<Item> item2 = new List<Item>() { new Item("vr headset", "glasses") };
                List<Item> item3 = new List<Item>() { new Item("Din Mor", "Er fed") };

                List<BookingItem> bookingItems1 = CreateBookingItems(item1);
                List<BookingItem> bookingItems2 = CreateBookingItems(item2);
                List<BookingItem> bookingItems3 = CreateBookingItems(item3);


                dbContext.Bookings.Add(new Booking(bookingItems1, new User("Simon", "smelly@.com", "1234"), "skal bruges i morgen", DateTime.Now, DateTime.Now));
                dbContext.Bookings.Add(new Booking(bookingItems2, new User("Mikkel", "meilig@.com", "1234"), "Details", DateTime.Now, DateTime.Now));
                dbContext.Bookings.Add(new Booking(bookingItems3, new User("Oscar", "oscar@.com", "1234"), "Details", DateTime.Now, DateTime.Now));

                await dbContext.SaveChangesAsync();
            }

            private List<BookingItem> CreateBookingItems(List<Item> items)
            {
                List<BookingItem> bookingItems = new List<BookingItem>();
                foreach (var item in items)
                {
                    bookingItems.Add(new BookingItem() { Item = item });
                }

                return bookingItems;
            }

        }
    }
}
