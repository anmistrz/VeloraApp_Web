using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerApi.Entities.Models;

namespace ClassLibrary.DAL.Interfaces
{
    public interface INotifications
    {
        public Task<(Notification, ConsultHistory)> GetNotificationByIdIncludesConsultHistoryAsync(int id);
        public Task<(Notification, TestDrive)> GetNotificationByIdIncludesTestDriveAsync(int id);
        public Task<IEnumerable<Notification>> GetAllNotificationsAsync();
        public Task<IEnumerable<Notification>> GetLimitNotificationAsync(int limit, int customId);
        public Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(int customId);
        public Task<IEnumerable<Notification>> GetNotificationsBySearchAsync(string searchTerm, int customId);
        public Task<Notification> UpdateReadNotificationAsync(int id, bool isRead);

        public Task<Notification> UpdateReadAndNotificationTypeAsync(Notification dataNotification, int salesPersonId);
        public Task<IEnumerable<Notification>> UpdateReadAllNotificationsAsync(bool isRead);
        public Task<bool> DeleteNotificationAsync(int id, string notificationType);
    }
}