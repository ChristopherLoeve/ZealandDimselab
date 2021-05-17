using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZealandDimselab.Models
{
    public class Notifications
    {
        [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  public int Id { get; set; }
        public int BookingId { get; set; }
        [ForeignKey("BookingId")] public Booking Booking { get; set; }
        public bool Seen { get; set; }

        public Notifications()
        {
            
        }


    }
}
