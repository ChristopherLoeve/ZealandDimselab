using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZealandDimselab.Models
{
    public class Booking : IObservable<Booking>
    {
        [Key]
        public int Id { get; set; }
        [Required] public List<BookingItem> BookingItems { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)] 
        public int UserId { get; set; }
        public User User { get; set; }
        public string Details { get; set; }
        [Required] public DateTime BookingDate { get; set; }
        [Required] public DateTime ReturnDate { get; set; }



        public Booking()
        {
            
        }

        public Booking(List<BookingItem> items, User user, string details, DateTime bookingDate, DateTime returnDate)
        {
            BookingItems = items;
            User = user;
            Details = details;
            BookingDate = bookingDate;
            ReturnDate = returnDate;

        }

        public Booking(int id, List<BookingItem> items, User user, string details, DateTime bookingDate, DateTime returnDate)
        {
            Id = id;
            BookingItems = items;
            User = user;
            Details = details;
            BookingDate = bookingDate;
            ReturnDate = returnDate;

        }

        public IDisposable Subscribe(IObserver<Booking> observer)
        {
            observer.OnNext(this);
            return null;
        }
    }
}
