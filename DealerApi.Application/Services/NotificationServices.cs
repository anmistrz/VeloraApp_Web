using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary.DAL.Interfaces;
using DealerApi.Application.DTO;
using DealerApi.Application.Interface;
using DealerApi.Entities.Models;

namespace DealerApi.Application.Services
{
    public class NotificationServices : INotificationServices
    {
        private readonly INotifications _notificationDAL;
        public NotificationServices(INotifications notificationDAL)
        {
            _notificationDAL = notificationDAL;
        }


        public List<NotificationDTO> GetAllNotifications()
        {
            throw new NotImplementedException();
        }

        public List<NotificationDTO> GetLimitNotification(int limit, int customId)
        {
            try
            {
                var notifications = _notificationDAL.GetLimitNotificationAsync(limit, customId).Result;
                Console.WriteLine($"Fetched {notifications.Count()} notifications with limit {limit}");
                var result = notifications.Select(n => new NotificationDTO
                {
                    NotificationId = n.NotificationId,
                    SalesPersonId = n.SalesPersonId,
                    CustomerId = n.CustomerId,
                    ConsultHistoryId = n.ConsultHistoryId,
                    TestDriveId = n.TestDriveId,
                    Message = n.Message,
                    NotificationType = n.NotificationType,
                    IsRead = n.IsRead,
                    Priority = n.Priority,
                    DealerId = n.DealerId
                }).ToList();
                
                return result;
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error fetching limited notifications", ex);
            }
        }

        public NotificationDTO GetNotificationById(int id)
        {
            try
            {
                var notification = _notificationDAL.GetNotificationByIdAsync(id).Result;
                if (notification != null)
                {
                    return new NotificationDTO
                    {
                        NotificationId = notification.NotificationId,
                        SalesPersonId = notification.SalesPersonId,
                        CustomerId = notification.CustomerId,
                        ConsultHistoryId = notification.ConsultHistoryId,
                        TestDriveId = notification.TestDriveId,
                        Message = notification.Message,
                        NotificationType = notification.NotificationType,
                        IsRead = notification.IsRead,
                        Priority = notification.Priority,
                        DealerId = notification.DealerId
                    };
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error fetching notification by ID", ex);
            }

            return null;
        }

        public List<NotificationDTO> GetNotificationsBySearch(string searchTerm, int customId)
        {
            try
            {
                var notifications = _notificationDAL.GetNotificationsBySearchAsync(searchTerm, customId).Result;
                return notifications.Select(n => new NotificationDTO
                {
                    NotificationId = n.NotificationId,
                    SalesPersonId = n.SalesPersonId,
                    CustomerId = n.CustomerId,
                    ConsultHistoryId = n.ConsultHistoryId,
                    TestDriveId = n.TestDriveId,
                    Message = n.Message,
                    NotificationType = n.NotificationType,
                    IsRead = n.IsRead,
                    Priority = n.Priority,
                    DealerId = n.DealerId
                }).ToList();
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error fetching notifications by search", ex);
            }
        }

        public List<NotificationDTO> GetUnreadNotifications(int customId)
        {
            try
            {
                var notifications = _notificationDAL.GetUnreadNotificationsAsync(customId).Result;
                return notifications.Select(n => new NotificationDTO
                {
                    NotificationId = n.NotificationId,
                    SalesPersonId = n.SalesPersonId,
                    CustomerId = n.CustomerId,
                    ConsultHistoryId = n.ConsultHistoryId,
                    TestDriveId = n.TestDriveId,
                    Message = n.Message,
                    NotificationType = n.NotificationType,
                    IsRead = n.IsRead,
                    Priority = n.Priority,
                    DealerId = n.DealerId
                }).ToList();
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error fetching unread notifications", ex);
            }
        }

        public List<NotificationDTO> UpdateReadAllNotifications(bool isRead)
        {
            try
            {
                var notifications = _notificationDAL.UpdateReadAllNotificationsAsync(isRead).Result;
                return notifications.Select(n => new NotificationDTO
                {
                    NotificationId = n.NotificationId,
                    SalesPersonId = n.SalesPersonId,
                    CustomerId = n.CustomerId,
                    ConsultHistoryId = n.ConsultHistoryId,
                    TestDriveId = n.TestDriveId,
                    Message = n.Message,
                    NotificationType = n.NotificationType,
                    IsRead = n.IsRead,
                    Priority = n.Priority,
                    DealerId = n.DealerId
                }).ToList();
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error updating all notifications", ex);
            }
        }

        public NotificationDTO UpdateReadAndNotificationType(NotificationDTO notification)
        {
            try
            {
                var body = new Notification
                {
                    NotificationId = notification.NotificationId,
                    SalesPersonId = notification.SalesPersonId,
                    CustomerId = notification.CustomerId,
                    ConsultHistoryId = notification.ConsultHistoryId,
                    TestDriveId = notification.TestDriveId,
                    Message = notification.Message,
                    NotificationType = notification.NotificationType,
                    IsRead = notification.IsRead,
                    Priority = notification.Priority,
                    DealerId = notification.DealerId
                };
                var updatedNotification = _notificationDAL.UpdateReadAndNotificationTypeAsync(body).Result;
                return new NotificationDTO
                {
                    NotificationId = updatedNotification.NotificationId,
                    SalesPersonId = updatedNotification.SalesPersonId,
                    CustomerId = updatedNotification.CustomerId,
                    ConsultHistoryId = updatedNotification.ConsultHistoryId,
                    TestDriveId = updatedNotification.TestDriveId,
                    Message = updatedNotification.Message,
                    NotificationType = updatedNotification.NotificationType,
                    IsRead = updatedNotification.IsRead,
                    Priority = updatedNotification.Priority,
                    DealerId = updatedNotification.DealerId
                };
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error updating notification", ex);
            }
        }

        public NotificationDTO UpdateReadNotificationStatus(int id, bool isRead)
        {
            try
            {
                var updatedNotification = _notificationDAL.UpdateReadNotificationAsync(id, isRead).Result;
                return new NotificationDTO
                {
                    NotificationId = updatedNotification.NotificationId,
                    SalesPersonId = updatedNotification.SalesPersonId,
                    CustomerId = updatedNotification.CustomerId,
                    ConsultHistoryId = updatedNotification.ConsultHistoryId,
                    TestDriveId = updatedNotification.TestDriveId,
                    Message = updatedNotification.Message,
                    NotificationType = updatedNotification.NotificationType,
                    IsRead = updatedNotification.IsRead,
                    Priority = updatedNotification.Priority,
                    DealerId = updatedNotification.DealerId
                };
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error updating notification status", ex);
            }
        }
    }
}