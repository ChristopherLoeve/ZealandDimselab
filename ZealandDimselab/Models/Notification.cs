using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZealandDimselab.Models
{
    public class Notification
    {
        [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BookingId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("BookingId")]
        public Booking Booking { get; set; }
        public bool Seen { get; set; }

        public Notification()
        {
            
        }


    }
}
