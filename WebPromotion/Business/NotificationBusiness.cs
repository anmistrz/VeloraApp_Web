using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebPromotion.Business.Interface;
using WebPromotion.Services.DTO;
using WebPromotion.Services.Interface;

namespace WebPromotion.Business
{
    public class NotificationBusiness : INotificationBusiness
    {
        private readonly INotificationServices _notificationServices;

        public NotificationBusiness(INotificationServices notificationServices)
        {
            _notificationServices = notificationServices;
        }

        public Task<List<NotificationDTO>> GetLimitNotification(int limit, int customId)
        {
            try
            {
                return _notificationServices.GetLimitNotification(limit, customId);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error fetching limited notifications", ex);
            }
        }

        public Task<NotificationDTO> GetNotificationById(int id)
        {
            try
            {
                return _notificationServices.GetNotificationById(id);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error fetching notification by ID", ex);
            }
        }

        public Task<List<NotificationDTO>> GetNotificationBySearchBussiness(string searchTerm, int customId)
        {
            try
            {
                return _notificationServices.GetNotificationsBySearchClient(searchTerm, customId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching notification by search", ex);
            }
        }

        public Task<List<NotificationDTO>> GetUnreadNotificationsBusiness(int customId)
        {
            try
            {
                return _notificationServices.GetUnreadNotificationsClient(customId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching unread notification by searcg", ex);
            }
        }

        public Task<List<NotificationDTO>> UpdateReadAllNotifications(bool isRead)
        {
            throw new NotImplementedException();
        }

        public Task<NotificationDTO> UpdateReadAndNotificationTypeBusiness(NotificationDTO notification)
        {
            try
            {
                return _notificationServices.UpdateReadAndNotificationTypeClient(notification);
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching update and read notification", ex);
            }
        }

        public Task<NotificationDTO> UpdateReadNotificationBusiness(int id, bool isRead)
        {
            throw new NotImplementedException();
        }

        public Task<NotificationDTO> UpdateReadNotificationStatus(int id, bool isRead)
        {
            try
            {
                return _notificationServices.UpdateReadNotificationStatus(id, isRead);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error updating notification status", ex);
            }
        }
    }
}