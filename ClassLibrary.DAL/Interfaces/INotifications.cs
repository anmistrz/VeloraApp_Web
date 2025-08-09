using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerApi.Entities.Models;

namespace ClassLibrary.DAL.Interfaces
{
    public interface INotifications
    {
        public Task<Notification> GetNotificationByIdAsync(int id);
        public Task<IEnumerable<Notification>> GetAllNotificationsAsync();
        public Task<IEnumerable<Notification>> GetLimitNotificationAsync(int limit);
        public Task<Notification> UpdateReadNotificationStatusAsync(int id, bool isRead);
        public Task<IEnumerable<Notification>> UpdateReadAllNotificationsAsync(bool isRead);
    }
}