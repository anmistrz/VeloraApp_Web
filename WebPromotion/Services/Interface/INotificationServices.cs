using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebPromotion.Services.DTO;

namespace WebPromotion.Services.Interface
{
    public interface INotificationServices
    {
        public Task<List<NotificationDTO>> GetAllNotifications();
        public Task<NotificationDTO> GetNotificationById(int id, string NotificationType);
        public Task<List<NotificationDTO>> GetLimitNotification(int limit, int customId);
        public Task<NotificationDTO> UpdateReadNotificationStatus(int id, bool isRead);
        public Task<List<NotificationDTO>> UpdateReadAllNotifications(bool isRead);
        public Task<NotificationDTO> UpdateReadAndNotificationTypeClient(NotificationDTO notification, int salesPersonId);
        public Task<List<NotificationDTO>> GetUnreadNotificationsClient(int customId);
        public Task<List<NotificationDTO>> GetNotificationsBySearchClient(string searchTerm, int customId);

    }
}