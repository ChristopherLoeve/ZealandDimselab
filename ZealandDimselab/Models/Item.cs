using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Permissions;

namespace ZealandDimselab.Models
{
    public class BookingQuantityLessThanStock : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            Item item = (Item)value;
            return item.BookingQuantity <= item.Stock;
        }
    }
    public class Item
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
        [Required][MaxLength(50)] public string Name { get; set; }
        [Required][MaxLength(500)] public string Description { get; set; }
        public string ImageName { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        [Required] public int Quantity { get; set; }
        [Required] public int Stock { get; set; }
        private int _bookingQuantity;
        [NotMapped]
        [BookingQuantityLessThanStock(ErrorMessage = "Quantity can not be more than what is currently in stock")]
        public int BookingQuantity
        {
            get { return _bookingQuantity; }
            set
            {
                if (value <= Stock)
                {
                    _bookingQuantity = value;
                }
            }
        }

        public ICollection<BookingItem> BookingItems { get; set; }

        public Item()
        {
        }

        public Item(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public bool Equals(Item item)
        {
            return this.Id == item.Id && this.Name == item.Name && this.Description == item.Description;
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}