using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ClassLibrary.BO.ModelNotConnectDB;
using ClassLibrary.DAL.Interfaces;
using DealerApi.DAL.Context;
using DealerApi.DAL.Interfaces;
using DealerApi.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DealerApi.DAL.DAL
{
    public class TestDriveDAL : ITestDrive
    {
        private readonly DealerRndDBContext _context;
        private readonly IEmailNotification _emailNotification;

        public TestDriveDAL(DealerRndDBContext context, IEmailNotification emailNotification)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _emailNotification = emailNotification ?? throw new ArgumentNullException(nameof(emailNotification));
        }

        public Task<TestDrive> CreateAsync(TestDrive entity)
        {
            throw new NotImplementedException();
        }

        public async Task<TestDrive> CreateTestDriveGuestAsync(Customer dataCustomer, TestDrive testDrive, DealerCar dataDealerCar)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                Console.WriteLine($"TestDriveDAL: Processing request for {dataCustomer.Email}");
                var validateCustomer = _context.Customers
                    .FirstOrDefault(c => c.Email == dataCustomer.Email && c.IsGuest);

                Console.WriteLine($"TestDriveDAL: Validating customer: {dataCustomer.Email}");
                Console.WriteLine($"TestDriveDAL: Found customer: {validateCustomer?.Email}");
                Console.WriteLine($"TestDriveDAL: Customer details: {validateCustomer?.CustomerId}, {validateCustomer?.FirstName}, {validateCustomer?.LastName}");

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

                    _context.Customers.Add(customer);
                    await _context.SaveChangesAsync();

                    var findCustomer = await _context.Customers
                        .FirstOrDefaultAsync(c => c.Email == dataCustomer.Email);
                    Console.WriteLine($"TestDriveDAL: Found customer: {findCustomer?.Email}");

                    if (findCustomer == null)
                    {
                        throw new Exception("Failed to create customer.");
                    }
                    dataCustomer.CustomerId = findCustomer.CustomerId;
                }

                Console.WriteLine($"Appointment Date: {testDrive.AppointmentDate}");

                var dtTestDrive = new TestDrive
                {
                    CustomerId = validateCustomer != null ? validateCustomer.CustomerId : dataCustomer.CustomerId,
                    DealerCarUnitId = testDrive.DealerCarUnitId,
                    Status = testDrive.Status ?? "Pending",
                    Note = testDrive.Note,
                    AppointmentDate = testDrive.AppointmentDate
                };

                Console.WriteLine($"Creating TestDrive: {dtTestDrive.DealerCarUnitId}, CustomerId: {dtTestDrive.CustomerId}, Status: {dtTestDrive.Status}");

                _context.TestDrives.Add(dtTestDrive);
                await _context.SaveChangesAsync(); 

                if (dtTestDrive.TestDriveId <= 0)
                {
                    throw new Exception("Failed to create test drive.");
                }

                var dataDealer = await _context.Dealers.FindAsync(dataDealerCar.DealerId);

                var dtNotification = new Notification
                {
                    CustomerId = validateCustomer != null ? validateCustomer.CustomerId : dataCustomer.CustomerId,
                    NotificationType = "TestDriveRequest",
                    DealerId = dataDealerCar.DealerId,
                    TestDriveId = dtTestDrive.TestDriveId,
                    Message = $"Test drive scheduled for {dataCustomer.FirstName} {dataCustomer.LastName} on {dtTestDrive.AppointmentDate.ToString("MMMM dd, yyyy")} at {dataDealer.DealerName}.",
                    IsRead = false,
                    Priority = 1,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Notifications.Add(dtNotification);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                var result = new TestDrive
                {
                    TestDriveId = dtTestDrive.TestDriveId,
                    CustomerId = dtTestDrive.CustomerId,
                    DealerCarUnitId = dtTestDrive.DealerCarUnitId,
                    Status = dtTestDrive.Status,
                    Note = dtTestDrive.Note,
                    AppointmentDate = dtTestDrive.AppointmentDate
                };

                return result;
            }
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteTestDriveAfterHandledAsync(int testDriveId, string reason)
        {
            try
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    var testDrive = await _context.TestDrives.FindAsync(testDriveId);
                    if (testDrive == null)
                    {
                        throw new Exception("Test drive not found.");
                    }

                    testDrive.Status = "Canceled";
                    _context.TestDrives.Update(testDrive);
                    await _context.SaveChangesAsync();


                    // Find related notification
                    var notification = await _context.Notifications
                        .Where(n => n.TestDriveId == testDriveId && n.CustomerId == testDrive.CustomerId)
                        .FirstOrDefaultAsync();

                    // Remove related SalesActivityLog(s) referencing this test drive (by TestDriveId)
                    var salesActivities = await _context.SalesActivityLogs
                        .Where(s => s.TestDriveId == testDriveId)
                        .FirstOrDefaultAsync();

                    if (salesActivities != null)
                    {
                        salesActivities.Details = reason;
                        salesActivities.ActivityType = "TestDriveCanceled";
                        _context.SalesActivityLogs.Update(salesActivities);
                        await _context.SaveChangesAsync();
                    }

                    
                    Console.WriteLine($"Test drive with ID {testDriveId} has been canceled and related notification removed.");
                    Console.WriteLine($"Customer ID: {testDrive.CustomerId}"); 
                    var dataForBodyEmail = await _context.Notifications
                        .Where(n => n.TestDriveId == testDriveId)
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
                        Console.WriteLine($"[WARN] No notification found for the consult history ID: {testDriveId} for email sending.");
                    }

                    // Now remove the notification
                    if (notification != null)
                    {
                        _context.Notifications.Remove(notification);
                        await _context.SaveChangesAsync();
                    }

                    if (dataForBodyEmail == null)
                    {
                        Console.WriteLine($"[WARN] No notification found for the consult history ID: {testDriveId} for email sending.");
                    }

                    var reqEmail = new EmailNotification
                    {
                        ToEmail = dataForBodyEmail?.CustomerEmail,
                        Subject = "Test Drive Request Canceled",
                        Body = $"<div style='font-family :Arial, sans-serif;'>" +
                                $"<h2>Dear {dataForBodyEmail?.CustomerName},</h2>" +
                                $"<p>Your Test Drive has been Canceled.</p>" +
                                $"<p>Thank you for choosing our service!</p>" +
                                $"<p style='margin-bottom: 20px;'>If you have any questions, feel free to reach out to us.</p>" +
                                $"<p>Best regards,<br />The Velora Team</p>" +
                                $"</div>"
                    };
                    Console.WriteLine($"Sending email to: {reqEmail.ToEmail}");
                    if (reqEmail.ToEmail != null)
                    {
                        await _emailNotification.SendEmailAsync(reqEmail.ToEmail, reqEmail.Subject, reqEmail.Body);
                    }

                    Console.WriteLine($"Deleting sales activity for test drive ID: {testDriveId}");
                    await transaction.CommitAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting test drive after handled", ex);
            }
        }

        public async Task<bool> DeleteTestDriveBeforeHandledAsync(int testDriveId, int salesPersonId, int dealerId, string reason)
        {
            try
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    var testDrive = await _context.TestDrives.FindAsync(testDriveId);
                    if (testDrive == null)
                    {
                        throw new Exception("Test drive not found.");
                    }

                    testDrive.Status = "Canceled";
                    _context.TestDrives.Update(testDrive);
                    await _context.SaveChangesAsync();

                    var notifications = _context.Notifications
                        .Where(n => n.TestDriveId == testDriveId && n.CustomerId == testDrive.CustomerId)
                        .ToList();
                    if (notifications.Count > 0)
                    {
                        foreach (var notification in notifications)
                        {
                            _context.Notifications.Remove(notification);
                        }
                        await _context.SaveChangesAsync();
                    }

                    var salesActivity = new SalesActivityLog
                    {
                        TestDriveId = testDriveId,
                        ActivityType = "TestDriveCanceled",
                        Details = reason,
                        SalesPersonId = salesPersonId,
                        CustomerId = testDrive.CustomerId,
                        DealerId = dealerId,
                        ActivityDate = DateTime.UtcNow
                    };

                    _context.SalesActivityLogs.Add(salesActivity);
                    await _context.SaveChangesAsync();

                    
                    var dataForBodyEmail = await _context.Notifications
                        .Where(n => n.ConsultHistoryId == testDriveId && n.CustomerId == testDrive.CustomerId)
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
                        throw new Exception("No notification found for the consult history.");
                    }

                    var reqEmail = new EmailNotification
                    {
                        ToEmail = dataForBodyEmail.CustomerEmail,
                        Subject = "Test Drive Request Canceled",
                        Body = $"<div style='font-family: Arial, sans-serif;'>" +
                                $"<h2>Dear {dataForBodyEmail.CustomerName},</h2>" +
                                $"<p>Your Test Drive has been Canceled.</p>" +
                                $"<p>Thank you for choosing our service!</p>" +
                                $"<p style='margin-bottom: 20px;'>If you have any questions, feel free to reach out to us.</p>" +
                                $"<p>Best regards,<br />The Velora Team</p>" +
                                $"</div>"
                    };
                    await _emailNotification.SendEmailAsync(reqEmail.ToEmail, reqEmail.Subject, reqEmail.Body);

                    await transaction.CommitAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting test drive before handled", ex);
            }
        }

        public async Task<IEnumerable<TestDrive>> GetAllAsync()
        {
            try
            {
                return await _context.TestDrives.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving all test drives", ex);
            }
        }

        public Task<TestDrive> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<(TestDrive, Customer, Car)>> GetTestDrivesBySalesPersonIdAsync(int salesPersonId)
        {
            try
            {
                var result = await _context.TestDrives
                    .Join(_context.Customers,
                        td => td.CustomerId,
                        c => c.CustomerId,
                        (td, c) => new { td, c })
                    .Join(_context.DealerCarUnits,
                        td => td.td.DealerCarUnitId,
                        dcu => dcu.DealerCarUnitId,
                        (tdc, dcu) => new { tdc.td, tdc.c, dcu })
                    .Join(_context.DealerCars,
                        tdc => tdc.dcu.DealerCarId,
                        dc => dc.DealerCarId,
                        (tdc, dc) => new { tdc.td, tdc.c, tdc.dcu, dc })
                    .Join(_context.Cars,
                        tdc => tdc.dc.CarId,
                        car => car.CarId,
                        (tdc, car) => new { tdc.td, tdc.c, car })
                    .Where(tdc => tdc.td.SalesPersonId == salesPersonId)
                    .ToListAsync();
                    
                return result.Select(x => (x.td, x.c, x.car));
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving test drives", ex);
            }
        }

        public Task<TestDrive> UpdateAsync(TestDrive entity)
        {
            throw new NotImplementedException();
        }
    }
}