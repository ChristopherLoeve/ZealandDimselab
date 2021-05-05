﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZealandDimselab.Models
{
    public class Booking
    {
        [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Id { get; set; }
        [Required] public List<Item> Items { get; set; }
        [Required] public User User { get; set; }
        public string Details { get; set; }
        [Required] public DateTime BookingDate { get; set; }
        [Required] public DateTime ReturnDate { get; set; }


        public Booking(int id, List<Item> items, User user, string details, DateTime bookingDate, DateTime returnDate)
        {
            Id = id;
            Items = items;
            User = user;
            Details = details;
            BookingDate = bookingDate;
            ReturnDate = returnDate;

        }
        
        public Booking()
        {
            Items = new List<Item>();
        }
    }
}