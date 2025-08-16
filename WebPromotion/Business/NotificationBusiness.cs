using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DealerApi.Entities.Models;
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

        public Task<NotificationDTO> GetNotificationById(int id, string NotificationType)
        {
            try
            {
                return _notificationServices.GetNotificationById(id, NotificationType);
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

        public Task<NotificationDTO> UpdateReadAndNotificationTypeBusiness(NotificationDTO notification, int salesPersonId)
        {
            try
            {
                var body = new NotificationDTO
                {
                    NotificationId = notification.NotificationId,
                    IsRead = notification.IsRead,
                    NotificationType = notification.NotificationType,
                    SalesPersonId = salesPersonId,
                    DealerId = notification.DealerId,
                    CustomerId = notification.CustomerId,
                    ConsultHistoryId = notification.ConsultHistoryId,
                    TestDriveId = notification.TestDriveId,
                    Message = notification.Message,
                    CreatedAt = notification.CreatedAt
                };
                Console.WriteLine($"UpdateReadAndNotificationTypeBusiness: {JsonSerializer.Serialize(body)}");
                return _notificationServices.UpdateReadAndNotificationTypeClient(body, salesPersonId);
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