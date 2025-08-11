using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerApi.Application.DTO;

namespace DealerApi.Application.Interface
{
    public interface INotificationServices
    {
        public List<NotificationDTO> GetAllNotifications();
        public NotificationDTO GetNotificationById(int id);
        public List<NotificationDTO> GetLimitNotification(int limit, int customId);
        public NotificationDTO UpdateReadNotificationStatus(int id, bool isRead);
        public List<NotificationDTO> UpdateReadAllNotifications(bool isRead);

        public NotificationDTO UpdateReadAndNotificationType(NotificationDTO notification);
        public List<NotificationDTO> GetUnreadNotifications(int customId);
        public List<NotificationDTO> GetNotificationsBySearch(string searchTerm, int customId);
    }
}