using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary.DAL.Interfaces;
using DealerApi.DAL.Context;
using DealerApi.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.DAL.DAL
{
    public class NotificationDAL : INotifications
    {
        private readonly DealerRndDBContext _context;

        public NotificationDAL(DealerRndDBContext context)
        {
            _context = context;
        }


        public Task<IEnumerable<Notification>> GetAllNotificationsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Notification>> GetLimitNotificationAsync(int limit, int customId)
        {
            try
            {
                return await _context.Notifications
                    .Where(n => n.DealerId == customId || n.SalesPersonId == customId)
                    .OrderByDescending(n => n.CreatedAt)
                    .Take(limit)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("Error fetching notifications", ex);
            }
        }

        public async Task<Notification> GetNotificationByIdAsync(int id)
        {
            try
            {
                return await _context.Notifications.FindAsync(id); 
            } catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("Error fetching notification", ex);
            }
        }

        public async Task<IEnumerable<Notification>> GetNotificationsBySearchAsync(string searchTerm, int customId)
        {
            try
            {
                Console.WriteLine($"GetNotificationsBySearchAsync: Searching for notifications with term '{searchTerm}' for customId {customId}");
                var result = await _context.Notifications
                    .Where(n => (n.DealerId == customId || n.SalesPersonId == customId) &&
                                (n.Message.Contains(searchTerm) || n.NotificationType.Contains(searchTerm)))
                    .ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("Error fetching notifications by search", ex);
            }
        }

        public async Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(int customId)
        {
            try
            {
                var result = await _context.Notifications
                    .Where(n => n.DealerId == customId || n.SalesPersonId == customId)
                    .Where(n => !n.IsRead)
                    .ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("Error fetching unread notifications", ex);
            }
        }

        public async Task<IEnumerable<Notification>> UpdateReadAllNotificationsAsync(bool isRead)
        {
            try
            {
                var notifications = await _context.Notifications
                    .Where(n => !n.IsRead)
                    .ToListAsync();

                foreach (var notification in notifications)
                {
                    notification.IsRead = isRead;
                }

                await _context.SaveChangesAsync();
                return notifications;
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("Error updating all notifications", ex);
            }
        }

        public async Task<Notification> UpdateReadAndNotificationTypeAsync(Notification dataNotification)
        {
            try
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    if (dataNotification.NotificationType == "ConsultationRequest")
                    {
                        dataNotification.NotificationType = "ConsultationRequestHandled";
                    }
                    else if (dataNotification.NotificationType == "TestDriveRequest")
                    {
                        dataNotification.NotificationType = "TestDriveRequestHandled";
                    }

                    var bodySalesActivity = new SalesActivityLog
                    {
                        NotificationId = dataNotification.NotificationId,
                        CustomerId = dataNotification.CustomerId ?? 0,
                        SalesPersonId = dataNotification.SalesPersonId ?? 0,
                        DealerId = dataNotification.DealerId ?? 0,
                        ActivityType = dataNotification.NotificationType
                    };

                    var activitySales = await _context.SalesActivityLogs.AddAsync(bodySalesActivity);
                    await _context.SaveChangesAsync();


                    var notification = await _context.Notifications.FindAsync(dataNotification.NotificationId);
                    if (notification != null)
                    {
                        if (notification.NotificationType == "ConsultationRequest")
                        {
                            notification.IsRead = true;
                            notification.NotificationType = "ConsultationRequestHandled";
                        }
                        else if (notification.NotificationType == "TestDriveRequest")
                        {
                            notification.IsRead = true;
                            notification.NotificationType = "TestDriveRequestHandled";
                        }

                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return notification;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("Error updating notification status", ex);
            }
            return null;
        }

        public Task<Notification> UpdateReadNotificationAsync(int id, bool isRead)
        {
            throw new NotImplementedException();
        }
    }
}