using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZealandDimselab.Models
{
    public class BookingItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ItemId { get; set; }
        public Item Item { get; set; }

        public BookingItem()
        {

        }

        public BookingItem(Booking booking, Item item)
        {
            Booking = booking;
            Item = item;
        }
    }
}
