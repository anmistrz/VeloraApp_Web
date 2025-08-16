using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary.DAL.Interfaces;
using DealerApi.Application.DTO;
using DealerApi.Application.Interface;
using DealerApi.Entities.Models;

namespace DealerApi.Application.BusinessLogic
{
    public class NotificationBL : INotificationBL
    {
        private readonly INotifications _notificationDAL;
        public NotificationBL(INotifications notificationDAL)
        {
            _notificationDAL = notificationDAL;
        }

        public bool DeleteNotification(int id, string notificationType)
        {
            try
            {
                return _notificationDAL.DeleteNotificationAsync(id, notificationType).Result;
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error deleting notification", ex);
            }
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

        public NotificationDTO GetNotificationById(int id, string notificationType)
        {
            try
            {
                Console.WriteLine($"Fetching notification by ID: {id}, Type: {notificationType}");
                if (notificationType == "ConsultationRequest" || notificationType == "ConsultationRequestHandled")
                {
                    var (notification, consultHistory) = _notificationDAL.GetNotificationByIdIncludesConsultHistoryAsync(id).Result;
                    return new NotificationDTO
                    {
                        NotificationId = notification.NotificationId,
                        SalesPersonId = notification.SalesPersonId,
                        CustomerId = notification.CustomerId,
                        ConsultHistoryId = consultHistory.ConsultHistoryId,
                        TestDriveId = notification.TestDriveId,
                        Message = notification.Message,
                        NotificationType = notification.NotificationType,
                        IsRead = notification.IsRead,
                        Priority = notification.Priority,
                        DealerId = notification.DealerId,
                        Notes = consultHistory.Note,
                        CreatedAt = notification.CreatedAt
                    };
                }
                else if (notificationType == "TestDriveRequest" || notificationType == "TestDriveRequestHandled")
                {
                    var (notification, testDrive) = _notificationDAL.GetNotificationByIdIncludesTestDriveAsync(id).Result;
                    return new NotificationDTO
                    {
                        NotificationId = notification.NotificationId,
                        SalesPersonId = notification.SalesPersonId,
                        CustomerId = notification.CustomerId,
                        ConsultHistoryId = notification.ConsultHistoryId,
                        TestDriveId = testDrive.TestDriveId,
                        Message = notification.Message,
                        NotificationType = notification.NotificationType,
                        IsRead = notification.IsRead,
                        Priority = notification.Priority,
                        DealerId = notification.DealerId,
                        Notes = testDrive.Note,
                        CreatedAt = notification.CreatedAt
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
                    DealerId = n.DealerId,
                    CreatedAt = n.CreatedAt
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
                Console.WriteLine($"Fetching unread notifications for customId: {customId}");
                var notifications = _notificationDAL.GetUnreadNotificationsAsync(customId).Result;
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
                    DealerId = n.DealerId,
                    CreatedAt = n.CreatedAt
                }).ToList();

                Console.WriteLine($"Fetched {result.Count} unread notifications for customId {customId}");
                return result;
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
                    DealerId = n.DealerId,
                    CreatedAt = n.CreatedAt
                }).ToList();
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error updating all notifications", ex);
            }
        }

        public NotificationDTO UpdateReadAndNotificationType(NotificationDTO notification, int salesPersonId)
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
                var updatedNotification = _notificationDAL.UpdateReadAndNotificationTypeAsync(body, salesPersonId).Result;
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