using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZealandDimselab.Models;

namespace ZealandDimselab.Services
{
    public class NotificationService : GenericService<Notification>
    {

        public NotificationService(IDbService<Notification> dbService): base(dbService)
        {
        }

        public async Task AddNotification(Booking booking)
        {
            Notification notification = new Notification();
            notification.Booking = booking;
            notification.Seen = false;
            await DbService.AddObjectAsync(notification);
        }


    }
}
