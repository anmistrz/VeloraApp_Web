using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary.BO.ModelNotConnectDB;
using ClassLibrary.DAL.Interfaces;
using DealerApi.DAL.Context;
using DealerApi.DAL.Interfaces;
using DealerApi.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DealerApi.DAL.DAL
{
    public class ConsultHistoryDAL : IConsultHistory
    {
        private readonly DealerRndDBContext _context;
        private readonly IEmailNotification _emailNotification;

        public ConsultHistoryDAL(DealerRndDBContext context, IEmailNotification emailNotification)
        {
            _emailNotification = emailNotification;
            _context = context;
        }


        public async Task<ConsultHistory> CreateAsync(ConsultHistory entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ConsultHistory> CreateConsultHistoryGuestAsync(Customer dataCustomer, ConsultHistory dataConsultHistory, DealerCar dataDealerCar)
        {
            try
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    Console.WriteLine($"ConsultHistoryDAL: Processing request for {dataCustomer.Email}");
                    var validateCustomer = _context.Customers
                        .FirstOrDefault(c => c.Email == dataCustomer.Email && c.IsGuest);
                        
                    Console.WriteLine($"ConsultHistoryDAL: Validating customer: {dataCustomer.Email}");

                    if (validateCustomer == null)
                    {
                        var customer = new Customer
                        {
                            FirstName = dataCustomer.FirstName,
                            LastName = dataCustomer.LastName,
                            Email = dataCustomer.Email,
                            PhoneNumber = dataCustomer.PhoneNumber,
                            UserName = "Guest",
                            IsGuest = true
                        };

                        Console.WriteLine($"Customer created: {customer.FirstName} {customer.LastName} - {customer.Email}");

                        await _context.Customers.AddAsync(customer);
                        await _context.SaveChangesAsync();

                        var findCustomer = await _context.Customers
                            .FirstOrDefaultAsync(c => c.Email == dataCustomer.Email);

                        if (findCustomer == null)
                        {
                            throw new Exception("Failed to create customer.");
                        }
                        dataCustomer.CustomerId = findCustomer.CustomerId;
                    }
                    else
                    {
                        // Use existing customer
                        dataCustomer.CustomerId = validateCustomer.CustomerId;
                        Console.WriteLine($"Using existing customer: {validateCustomer.FirstName} {validateCustomer.LastName} - {validateCustomer.Email}");
                    }

                    var consultHistory = new ConsultHistory
                    {
                        CustomerId = dataCustomer.CustomerId,
                        DealerCarUnitId = dataConsultHistory.DealerCarUnitId,
                        SalesPersonId = dataConsultHistory.SalesPersonId == 0 ? null : dataConsultHistory.SalesPersonId,
                        Budget = dataConsultHistory.Budget,
                        ConsultDate = dataConsultHistory.ConsultDate,
                        Note = dataConsultHistory.Note
                    };

                    Console.WriteLine($"Creating ConsultHistory: {consultHistory.DealerCarUnitId}, CustomerId: {consultHistory.CustomerId}, SalesPersonId: {consultHistory.SalesPersonId}, Budget: {consultHistory.Budget}, ConsultDate: {consultHistory.ConsultDate}");

                    await _context.ConsultHistories.AddAsync(consultHistory);
                    await _context.SaveChangesAsync();

                    var dataDealer = await _context.Dealers.FindAsync(dataDealerCar.DealerId);

                    var notification = new Notification
                    {
                        CustomerId = dataCustomer.CustomerId,
                        DealerId = dataDealerCar.DealerId,
                        ConsultHistoryId = consultHistory.ConsultHistoryId,
                        NotificationType = "ConsultationRequest",
                        Message = $"New consultation request from {dataCustomer.FirstName} {dataCustomer.LastName} on {consultHistory.ConsultDate.ToString("MMMM dd, yyyy")} at dealer {dataDealer.DealerName}.",
                        IsRead = false,
                        Priority = 1,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _context.Notifications.AddAsync(notification);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    var result = new ConsultHistory
                    {
                        CustomerId = consultHistory.CustomerId,
                        DealerCarUnitId = consultHistory.DealerCarUnitId,
                        SalesPersonId = consultHistory.SalesPersonId,
                        Budget = consultHistory.Budget,
                        ConsultDate = consultHistory.ConsultDate,
                        Note = consultHistory.Note,
                        ConsultHistoryId = consultHistory.ConsultHistoryId
                    };
                    return result;
                }
           } 
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log them)
                throw new Exception("Error creating consultation history guest", ex);
            }
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteConsultHistoryAfterHandledAsync(int consultHistoryId, string reason)
        {
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    var consultHistory = _context.ConsultHistories.Find(consultHistoryId);
                    if (consultHistory == null)
                    {
                        Console.WriteLine($"[WARN] Consult history not found for ID: {consultHistoryId}");
                        return false;
                    }

                    Console.WriteLine($"Deleting consult history with ID: {consultHistoryId}, Reason: {reason}");

                    var getNotificationsById = await _context.Notifications
                        .Where(n => n.ConsultHistoryId == consultHistoryId && n.CustomerId == consultHistory.CustomerId)
                        .ToListAsync();

                    if (getNotificationsById.Count == 0)
                    {
                        Console.WriteLine($"[WARN] No notifications found for the consult history ID: {consultHistoryId}");
                    }
                    else
                    {
                        Console.WriteLine($"Found {getNotificationsById.Count} notifications for consult history ID: {consultHistoryId}");
                        foreach (var notification in getNotificationsById)
                        {
                            try
                            {
                                var relatedSalesActivities = _context.SalesActivityLogs.Where(s => s.NotificationId == notification.NotificationId).ToList();
                                if (relatedSalesActivities.Count == 0)
                                {
                                    Console.WriteLine($"[INFO] No SalesActivityLog records reference NotificationId {notification.NotificationId}");
                                }
                                foreach (var activity in relatedSalesActivities)
                                {
                                    Console.WriteLine($"[INFO] Setting NotificationId to NULL for SalesActivityLog.ActivityLogId={activity.ActivityLogId}");
                                    activity.NotificationId = null;
                                    _context.SalesActivityLogs.Update(activity);
                                }
                                await _context.SaveChangesAsync();
                                _context.Notifications.Remove(notification);
                                Console.WriteLine($"[INFO] Deleted NotificationId {notification.NotificationId}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"[ERROR] Failed to update SalesActivityLog or delete NotificationId {notification.NotificationId}: {ex.Message}\n{ex.StackTrace}");
                                throw;
                            }
                        }
                        await _context.SaveChangesAsync();
                    }

                    var salesActivity = _context.SalesActivityLogs
                        .Where(s => s.ConsultationId == consultHistoryId)
                        .FirstOrDefault();
                    if (salesActivity == null)
                    {
                        Console.WriteLine($"[WARN] No sales activity found for the consult history ID: {consultHistoryId}");
                    }
                    else
                    {
                        Console.WriteLine($"Deleting sales activity for consult history ID: {consultHistoryId}");
                        salesActivity.Details = reason;
                        salesActivity.ActivityType = "ConsultationCanceled";
                        _context.SalesActivityLogs.Update(salesActivity);
                        await _context.SaveChangesAsync();
                    }

                    consultHistory.StatusConsultation = "Canceled";
                    _context.ConsultHistories.Update(consultHistory);
                    await _context.SaveChangesAsync();

                    var dataForBodyEmail = await _context.Notifications
                        .Where(n => n.ConsultHistoryId == consultHistoryId)
                        .Join(_context.Customers,
                            n => n.CustomerId,
                            c => c.CustomerId,
                            (n, c) => new
                            {
                                CustomerName = $"{c.FirstName} {c.LastName}",
                                CustomerEmail = c.Email
                            })
                        .FirstOrDefaultAsync();

                    if (dataForBodyEmail == null)
                    {
                        Console.WriteLine($"[WARN] No notification found for the consult history ID: {consultHistoryId} for email sending.");
                    }
                    else
                    {
                        var reqEmail = new EmailNotification
                        {
                            ToEmail = dataForBodyEmail.CustomerEmail,
                            Subject = "Consultation Request Canceled",
                            Body = $"<div style='font-family: Arial, sans-serif;'>" +
                                    $"<h2>Dear {dataForBodyEmail.CustomerName},</h2>" +
                                    $"<p>Your Consultation has been Canceled.</p>" +
                                    $"<p>Thank you for choosing our service!</p>" +
                                    $"<p style='margin-bottom: 20px;'>If you have any questions, feel free to reach out to us.</p>" +
                                    $"<p>Best regards,<br />The Velora Team</p>" +
                                    $"</div>"
                        };
                        await _emailNotification.SendEmailAsync(reqEmail.ToEmail, reqEmail.Subject, reqEmail.Body);
                        await _context.SaveChangesAsync();
                    }

                    transaction.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Exception in DeleteConsultHistoryAfterHandledAsync: {ex.Message}\n{ex.StackTrace}");
                throw new Exception("Error deleting consultation history after handled", ex);
            }
        }

        public async Task<bool> DeleteConsultHistoryBeforeHandledAsync(int consultHistoryId, int salesPersonId, int dealerId, string reason)
        {
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    var consultHistory = await _context.ConsultHistories.FindAsync(consultHistoryId);

                    consultHistory.StatusConsultation = "Canceled";
                    _context.ConsultHistories.Update(consultHistory);
                    await _context.SaveChangesAsync();

                    var salesActivity = new SalesActivityLog
                    {
                        ConsultationId = consultHistoryId,
                        ActivityType = "ConsultationCanceled",
                        Details = reason,
                        SalesPersonId = salesPersonId,
                        CustomerId = consultHistory.CustomerId ?? 0,
                        DealerId = dealerId,
                        ActivityDate = DateTime.UtcNow,
                    };

                    _context.SalesActivityLogs.Add(salesActivity);
                    await _context.SaveChangesAsync();

                    var notification = await _context.Notifications
                        .Where(n => n.ConsultHistoryId == consultHistoryId)
                        .FirstOrDefaultAsync();

                    if (notification != null)
                    {
                        _context.Notifications.Remove(notification);
                        await _context.SaveChangesAsync();
                    }

                        var dataForBodyEmail = await _context.Notifications
                        .Where(n => n.ConsultHistoryId == consultHistoryId)
                        .Join(_context.Customers,
                            n => n.CustomerId,
                            c => c.CustomerId,
                            (n, c) => new
                            {
                                CustomerName = $"{c.FirstName} {c.LastName}",
                                CustomerEmail = c.Email
                            })
                        .FirstOrDefaultAsync();

                    var reqEmail = new EmailNotification
                    {
                        ToEmail = dataForBodyEmail.CustomerEmail,
                        Subject = "Consultation Request Canceled",
                        Body = $"<div style='font-family: Arial, sans-serif;'>" +
                                $"<h2>Dear {dataForBodyEmail.CustomerName},</h2>" +
                                $"<p>Your Consultation has been Canceled.</p>" +
                                $"<p>Thank you for choosing our service!</p>" +
                                $"<p style='margin-bottom: 20px;'>If you have any questions, feel free to reach out to us.</p>" +
                                $"<p>Best regards,<br />The Velora Team</p>" +
                                $"</div>"
                    };
                    await _emailNotification.SendEmailAsync(reqEmail.ToEmail, reqEmail.Subject, reqEmail.Body);
                    await _context.SaveChangesAsync();

                    transaction.Commit();
                    return true;
                }
            } catch (Exception ex)
            {
                // Handle exceptions (e.g., log them)
                throw new Exception("Error deleting consultation history before handled", ex);
            }
        }

        public Task<IEnumerable<ConsultHistory>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ConsultHistory>> GetAllConsultHistoryAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ConsultHistory> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<(ConsultHistory, Customer, Car)>> GetConsultHistoryBySalesPersonIdAsync(int salesPersonId)
        {
            try
            {
                var result = await _context.ConsultHistories
                    .Join(_context.DealerCarUnits,
                        ch => ch.DealerCarUnitId,
                        dcu => dcu.DealerCarUnitId,
                        (ch, dcu) => new { ch, dcu })
                    .Join(_context.DealerCars,
                        x => x.dcu.DealerCarId,
                        dc => dc.DealerCarId,
                        (x, dc) => new { x.ch, x.dcu, dc })
                    .Join(_context.Customers,
                        x => x.ch.CustomerId,
                        c => c.CustomerId,
                        (x, c) => new { x.ch, x.dcu, x.dc, c })
                    .Join(_context.Cars,
                        x => x.dc.CarId,
                        car => car.CarId,
                        (x, car) => new { x.ch, x.dcu, x.dc, x.c, car })
                    .Where(x => x.ch.SalesPersonId == salesPersonId)
                    .ToListAsync();
                return result.Select(x => (x.ch, x.c, x.car));
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log them)
                throw new Exception("Error retrieving consultation history by sales person ID", ex);
            }
        }

        public Task<ConsultHistory> UpdateAsync(ConsultHistory entity)
        {
            throw new NotImplementedException();
        }
    }
}