using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebPromotion.Services.DTO;

namespace WebPromotion.Business.Interface
{
    public interface INotificationBusiness
    {
        //         public Task<NotificationDTO> UpdateReadNotificationStatus(int id, bool isRead);
        // public Task<List<NotificationDTO>> UpdateReadAllNotifications(bool isRead);
        // public Task<NotificationDTO> UpdateReadAndNotificationTypeClient(NotificationDTO notification);
        // public Task<List<NotificationDTO>> GetUnreadNotificationsClient(int customId);
        // public Task<List<NotificationDTO>> GetNotificationsBySearchClient(string searchTerm, int customId);

        public Task<NotificationDTO> GetNotificationById(int id, string NotificationType);
        public Task<List<NotificationDTO>> GetLimitNotification(int limit, int customId);
        public Task<NotificationDTO> UpdateReadNotificationStatus(int id, bool isRead);
        public Task<List<NotificationDTO>> GetNotificationBySearchBussiness(string searchTerm, int customId);
        public Task<List<NotificationDTO>> GetUnreadNotificationsBusiness(int customId);
        public Task<NotificationDTO> UpdateReadAndNotificationTypeBusiness(NotificationDTO notification, int salesPerson);
        public Task<NotificationDTO> UpdateReadNotificationBusiness(int id, bool isRead);
        public Task<List<NotificationDTO>> UpdateReadAllNotifications(bool isRead);
        
    }
}