using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ClassLibrary.BO.ModelNotConnectDB;
using ClassLibrary.DAL.Interfaces;
using DealerApi.DAL.Context;
using DealerApi.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.DAL.DAL
{
    public class NotificationDAL : INotifications
    {
        private readonly DealerRndDBContext _context;
        private readonly IEmailNotification _emailNotification;

        public NotificationDAL(DealerRndDBContext context, IEmailNotification emailNotification)
        {
            _context = context;
            _emailNotification = emailNotification;
        }

        public async Task<bool> DeleteNotificationAsync(int id, string notificationType)
        {
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    var notification = _context.Notifications.Find(id);
                    if (notification == null)
                    {
                        throw new Exception("Notification not found.");
                    }

                    if (notificationType == "ConsultationRequestHandled")
                    {
                        var consultHistory = _context.ConsultHistories.FirstOrDefault(c => c.ConsultHistoryId == notification.ConsultHistoryId);
                        if (consultHistory != null)
                        {
                            consultHistory.StatusConsultation = "Canceled";
                            _context.ConsultHistories.Update(consultHistory);
                            await _context.SaveChangesAsync();

                        }
                    }
                    else if (notificationType == "TestDriveRequestHandled")
                    {
                        var testDrive = _context.TestDrives.FirstOrDefault(t => t.TestDriveId == notification.TestDriveId);
                        if (testDrive != null)
                        {
                            testDrive.Status = "Canceled";
                            _context.TestDrives.Update(testDrive);
                            await _context.SaveChangesAsync();
                        }
                    }

                    _context.Notifications.Remove(notification);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return true;
                }
                
            } catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("Error deleting notification", ex);
            }
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

        public async Task<(Notification, ConsultHistory)> GetNotificationByIdIncludesConsultHistoryAsync(int id)
        {
            try
            {
                    var result = await _context.Notifications.Join(
                        _context.ConsultHistories,
                        notification => notification.ConsultHistoryId,
                        history => history.ConsultHistoryId,
                        (notification, history) => new { notification, history }
                    )
                    .Where(x => x.notification.NotificationId == id)
                    .FirstOrDefaultAsync();

                    if (result?.notification == null || result?.history == null)
                    {
                        throw new Exception("Notification or ConsultHistory not found.");
                    }

                    return (result?.notification, result?.history);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("Error fetching notification", ex);
            }
        }

        public async Task<(Notification, TestDrive)> GetNotificationByIdIncludesTestDriveAsync(int id)
        {
            try
            {
                var result = await _context.Notifications.Join(
                    _context.TestDrives,
                    notification => notification.TestDriveId,
                    testDrive => testDrive.TestDriveId,
                    (notification, testDrive) => new { notification, testDrive }
                )
                .Where(x => x.notification.NotificationId == id)
                .FirstOrDefaultAsync();

                if (result?.notification == null || result?.testDrive == null)
                {
                    throw new Exception("Notification or TestDrive not found.");
                }

                return (result?.notification, result?.testDrive);
            }
            catch (Exception ex)
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

        public async Task<Notification> UpdateReadAndNotificationTypeAsync(Notification dataNotification, int salesPersonId)
        {
            try
            {
                Console.WriteLine($"UpdateReadAndNotificationTypeAsync: {JsonSerializer.Serialize(dataNotification)} SalesPersonId= {salesPersonId}");
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

                    // Only add SalesActivityLog if ConsultHistory exists (for ConsultationRequestHandled)
                    bool canInsertSalesActivity = true;
                    int consultHistoryId = dataNotification.ConsultHistoryId ?? 0;
                    if (dataNotification.NotificationType == "ConsultationRequestHandled")
                    {
                        if (consultHistoryId == 0 ||
                            !_context.ConsultHistories.Any(c => c.ConsultHistoryId == consultHistoryId))
                        {
                            canInsertSalesActivity = false;
                            Console.WriteLine($"[Warning] Skipping SalesActivityLog insert: ConsultHistoryId {consultHistoryId} does not exist.");
                        }
                    }

                    if (canInsertSalesActivity)
                    {
                        var bodySalesActivity = new SalesActivityLog
                        {
                            NotificationId = dataNotification.NotificationId,
                            CustomerId = dataNotification.CustomerId ?? 0,
                            SalesPersonId = salesPersonId,
                            DealerId = dataNotification.DealerId ?? 0,
                            ActivityType = dataNotification.NotificationType,
                            ConsultationId = consultHistoryId == 0 ? null : consultHistoryId,
                            TestDriveId = dataNotification.TestDriveId == 0 ? null : dataNotification.TestDriveId,
                            ActivityDate = DateTime.UtcNow
                        };

                        Console.WriteLine($"SalesActivityLog: {JsonSerializer.Serialize(bodySalesActivity)}");

                        await _context.SalesActivityLogs.AddAsync(bodySalesActivity);
                        await _context.SaveChangesAsync();
                    }

                    var notification = await _context.Notifications.FindAsync(dataNotification.NotificationId);

                    if (notification != null)
                    {
                        // Update Notification
                        if (notification.NotificationType == "ConsultationRequest")
                        {
                            notification.IsRead = true;
                            notification.NotificationType = "ConsultationRequestHandled";
                            notification.SalesPersonId = salesPersonId;
                        }
                        else if (notification.NotificationType == "TestDriveRequest")
                        {
                            notification.IsRead = true;
                            notification.NotificationType = "TestDriveRequestHandled";
                            notification.SalesPersonId = salesPersonId;
                        }

                        Console.WriteLine($"Updating Notification: Id={notification.NotificationId}, IsRead={notification.IsRead}, NotificationType={notification.NotificationType}, SalesPersonId={notification.SalesPersonId}");
                        if (notification.NotificationType == "ConsultationRequestHandled")
                        {
                            await _context.ConsultHistories
                                .Where(c => c.ConsultHistoryId == notification.ConsultHistoryId)
                                .ExecuteUpdateAsync(c => c
                                    .SetProperty(x => x.SalesPersonId, salesPersonId)
                                    .SetProperty(x => x.StatusConsultation, "Scheduled"));
                        }
                        else if (notification.NotificationType == "TestDriveRequestHandled")
                        {
                            await _context.TestDrives
                                .Where(t => t.TestDriveId == notification.TestDriveId)
                                .ExecuteUpdateAsync(t => t
                                    .SetProperty(x => x.SalesPersonId, salesPersonId)
                                    .SetProperty(x => x.Status, "Scheduled"));
                        }

                        await _context.SaveChangesAsync();

                        // ...existing code for email notification...
                        var dataForBodyEmailQuery = _context.Notifications
                            .Join(_context.Customers,
                                notification => notification.CustomerId,
                                customer => customer.CustomerId,
                                (notification, customer) => new
                                {
                                    notification.Message,
                                    CustomerEmail = customer.Email,
                                    CustomerName = customer.FirstName + " " + customer.LastName,
                                    DealerId = notification.DealerId,
                                    CustomerId = notification.CustomerId
                                })
                            .Join(_context.Dealers,
                                notification => notification.DealerId,
                                dealer => dealer.DealerId,
                                (notification, dealer) => new
                                {
                                    notification.CustomerEmail,
                                    notification.CustomerName,
                                    DealerId = notification.DealerId,
                                    DealerName = dealer.DealerName,
                                    DealerAddress = dealer.Address,
                                    DealerPhone = dealer.PhoneNumber,
                                    DealerProvince = dealer.Province,
                                    DealerCity = dealer.City,
                                    CustomerId = notification.CustomerId
                                })
                            .Join(_context.SalesPeople,
                                notification => notification.DealerId,
                                salesPerson => salesPerson.DealerId,
                                (notification, salesPerson) => new
                                {
                                    notification.CustomerEmail,
                                    notification.CustomerName,
                                    notification.DealerName,
                                    notification.DealerAddress,
                                    notification.DealerPhone,
                                    notification.DealerProvince,
                                    notification.DealerCity,
                                    SalesPersonName = salesPerson.FullName,
                                    CustomerId = notification.CustomerId
                                })
                            .Where(notification => notification.CustomerId == dataNotification.CustomerId);

                        var dataForBodyEmail = await dataForBodyEmailQuery.FirstOrDefaultAsync();

                        Console.WriteLine($"Data for email: {dataForBodyEmail?.CustomerEmail}, {dataForBodyEmail?.CustomerName}, {dataForBodyEmail?.DealerName}, {dataForBodyEmail?.SalesPersonName}");

                        if (dataForBodyEmail != null)
                        {
                            if (notification.NotificationType == "ConsultationRequestHandled")
                            {
                                var getConsultDate = _context.ConsultHistories
                                    .Where(c => c.CustomerId == dataForBodyEmail.CustomerId && c.ConsultHistoryId == consultHistoryId)
                                    .Select(c => c.ConsultDate)
                                    .FirstOrDefault();

                                var reqEmail = new EmailNotification
                                {
                                    ToEmail = dataForBodyEmail.CustomerEmail,
                                    Subject = "Consultation Request Handled",
                                    Body = $"<div style='font-family: Arial, sans-serif;'>" +
                                           $"<h2>Dear {dataForBodyEmail.CustomerName},</h2>" +
                                           $"<p>Your consultation request has been handled by <strong>{dataForBodyEmail.SalesPersonName}</strong> from <strong>{dataForBodyEmail.DealerName}</strong>.</p>" +
                                           $"<p>Consultation Date: {getConsultDate.ToString("MMMM dd, yyyy")}</p>" +
                                            $"<p><strong>Address:</strong> {dataForBodyEmail.DealerAddress}</p>" +
                                            $"<p><strong>Phone:</strong> {dataForBodyEmail.DealerPhone}</p>" +
                                            $"<p><strong>Province:</strong> {dataForBodyEmail.DealerProvince}</p>" +
                                            $"<p><strong>City:</strong> {dataForBodyEmail.DealerCity}</p>" +
                                            $"<br/>" +
                                            $"<p>You can meet with {dataForBodyEmail.SalesPersonName} at the dealership for further assistance.</p>" +
                                           $"<p>Thank you for choosing our service!</p>" +
                                           $"<p style='margin-bottom: 20px;'>If you have any questions, feel free to reach out to us.</p>" +
                                           $"<p>Best regards,<br />The Velora Team</p>" +
                                           $"</div>"
                                };

                                Console.WriteLine($"Sending email to: {reqEmail.ToEmail}");
                                Console.WriteLine($"Subject: {reqEmail.Subject}");
                                Console.WriteLine($"Body: {reqEmail.Body}");

                                await _emailNotification.SendEmailAsync(reqEmail.ToEmail, reqEmail.Subject, reqEmail.Body);

                            }
                            else if (notification.NotificationType == "TestDriveRequestHandled")
                            {
                                var getTestDriveDate = _context.TestDrives
                                    .Where(c => c.CustomerId == dataForBodyEmail.CustomerId && c.TestDriveId == dataNotification.TestDriveId)
                                    .Select(c => c.AppointmentDate)
                                    .FirstOrDefault();
                                Console.WriteLine($"TestDrive Appointment Date: {getTestDriveDate}");
                                if (getTestDriveDate == default)
                                {
                                    Console.WriteLine($"[WARN] No TestDrive found for Customer ID: {dataForBodyEmail.CustomerId}");
                                }

                                var reqEmail = new EmailNotification
                                {
                                    ToEmail = dataForBodyEmail.CustomerEmail,
                                    Subject = "Test Drive Request Handled",
                                    Body = $"<div style='font-family: Arial, sans-serif;'>" +
                                           $"<h2>Dear {dataForBodyEmail.CustomerName},</h2>" +
                                           $"<p>Your test drive request has been handled by <strong>{dataForBodyEmail.SalesPersonName}</strong> from <strong>{dataForBodyEmail.DealerName}</strong>.</p>" +
                                           $"<p><strong>Address:</strong> {dataForBodyEmail.DealerAddress}</p>" +
                                           $"<p><strong>Phone:</strong> {dataForBodyEmail.DealerPhone}</p>" +
                                           $"<p><strong>Province:</strong> {dataForBodyEmail.DealerProvince}</p>" +
                                           $"<p><strong>City:</strong> {dataForBodyEmail.DealerCity}</p>" +
                                           $"<p><strong>Test Drive Date:</strong> {getTestDriveDate.ToString("MMMM dd, yyyy")}</p>" +
                                           $"<br/>" +
                                           $"<p>You can meet with {dataForBodyEmail.SalesPersonName} at the dealership for further assistance.</p>" +
                                           $"<p>Thank you for choosing our service!</p>" +
                                           $"<p style='margin-bottom: 20px;'>If you have any questions, feel free to reach out to us.</p>" +
                                           $"<p>Best regards,<br />The Velora Team</p>" +
                                           $"</div>"
                                };

                                Console.WriteLine($"Sending email to: {reqEmail.ToEmail}");
                                Console.WriteLine($"Subject: {reqEmail.Subject}");
                                Console.WriteLine($"Body: {reqEmail.Body}");

                                await _emailNotification.SendEmailAsync(reqEmail.ToEmail, reqEmail.Subject, reqEmail.Body);
                            }
                        }

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