using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZealandDimselab.Models
{
    public class Administrator : User, IObserver<Booking>
    {
        private List<Booking> bookings;
        public Administrator()
        {
            bookings = new List<Booking>();
        }

        public Administrator(string name, string email, string password) : base(name, email, password)
        {
            bookings = new List<Booking>();
        }

        public Administrator(int id, string name, string email, string password) : base(id, name, email, password)
        {
            bookings = new List<Booking>();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(Booking value)
        {
            bookings.Add(value);
        }
    }
}
