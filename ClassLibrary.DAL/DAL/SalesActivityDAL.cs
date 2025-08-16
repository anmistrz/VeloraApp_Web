using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ClassLibrary.DAL.Interfaces;
using DealerApi.DAL.Context;
using DealerApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace ClassLibrary.DAL.DAL
{
    public class SalesActivityDAL : ISalesActivityLog
    {
        private readonly DealerRndDBContext _context;
        public SalesActivityDAL(DealerRndDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<(SalesActivityLog, Customer, Car)>> GetAllSalesActivitiesBySalesPersonAsync(int salesPersonId, string ActivityType)
        {
            try
            {
                var result = await _context.SalesActivityLogs
                    .Join(_context.Customers,
                        sal => sal.CustomerId,
                        cust => cust.CustomerId,
                        (sal, cust) => new { sal, cust })
                    .Join(_context.DealerCars,
                        scn => scn.sal.DealerId,
                        car => car.DealerId,
                        (scn, car) => new { scn.sal, scn.cust, car })
                    .Join(_context.Cars,
                        scn => scn.car.CarId,
                        car => car.CarId,
                        (scn, car) => new { scn.sal, scn.cust, car })
                    .Where(sal => sal.sal.SalesPersonId == salesPersonId && sal.sal.ActivityType == ActivityType)
                    .ToListAsync();

                return result.Select(x => (x.sal, x.cust, x.car));
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                throw new Exception("Error fetching sales activities", ex);
            }
        }

        public async Task<SalesActivityLog> GetSalesActivityConsultationById(int consultationId)
        {
            try
            {
                Console.WriteLine($"ConsultationId: {consultationId}");
                var result = await _context.SalesActivityLogs
                    .Where(sal => sal.ConsultationId == consultationId)
                    .FirstOrDefaultAsync();
                Console.WriteLine($"Resulttttttttt: {JsonSerializer.Serialize(result)}");
                if (result == null)
                {
                    throw new NotFoundException($"No sales activity consultation found for ConsultationId={consultationId}.");
                }
                return result;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                throw new InvalidOperationException($"Error fetching sales activity consultation by ID: {ex.Message}", ex);
            }
        }

        public async Task<SalesActivityLog> GetSalesActivityTestDriveById(int testDriveId)
        {
            try
            {
                Console.WriteLine($"TestDriveIddddddd: {testDriveId}");
                var result = await _context.SalesActivityLogs
                    .Where(sal => sal.TestDriveId == testDriveId)
                    .FirstOrDefaultAsync();
                Console.WriteLine($"Resulttttttttt: {JsonSerializer.Serialize(result)}");

                if (result == null)
                {
                    throw new NotFoundException($"No sales activity test drive found for TestDriveId={testDriveId}.");
                }

                return result;
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                throw new InvalidOperationException("Error fetching sales activity test drive by ID", ex);
            }
        }

        public async Task<bool> UpdateResultConsultationAsync(int salesActivityId, string result)
        {
            try
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    var salesActivity = await _context.SalesActivityLogs.FirstOrDefaultAsync(sal => sal.ActivityLogId == salesActivityId && sal.ActivityType == "ConsultationRequestHandled");
                    if (salesActivity != null)
                    {
                        salesActivity.Details = result;
                        _context.SalesActivityLogs.Update(salesActivity);
                        await _context.SaveChangesAsync();

                        var getNotificationById = await _context.Notifications
                            .FirstOrDefaultAsync(n => n.NotificationId == salesActivity.NotificationId && n.NotificationType == "ConsultationRequestHandled");
                        if (getNotificationById != null)
                        {
                            if (getNotificationById.ConsultHistoryId != null)
                            {
                                var consultHistory = await _context.ConsultHistories.FirstOrDefaultAsync(ch => ch.ConsultHistoryId == getNotificationById.ConsultHistoryId);
                                if (consultHistory != null)
                                {
                                    consultHistory.StatusConsultation = "Completed";
                                    _context.ConsultHistories.Update(consultHistory);
                                    await _context.SaveChangesAsync();
                                }
                            }
                        }

                        var getSalesPerformance = await _context.SalesPersonPerformances
                            .FirstOrDefaultAsync(sp => sp.SalesPersonId == salesActivity.SalesPersonId && sp.MetricType == "ConsultationCar" && sp.MetricDate == DateOnly.FromDateTime(DateTime.UtcNow));

                        var addSalesPerformance = new SalesPersonPerformance
                        {
                            SalesPersonId = salesActivity.SalesPersonId,
                            MetricType = "ConsultationCar",
                            MetricValue = 1,
                            MetricDate = DateOnly.FromDateTime(DateTime.UtcNow),
                        };
                        if (getSalesPerformance == null)
                        {
                            _context.SalesPersonPerformances.Add(addSalesPerformance);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            getSalesPerformance.MetricValue += 1;
                            _context.SalesPersonPerformances.Update(getSalesPerformance);
                            await _context.SaveChangesAsync();
                        }

                        await transaction.CommitAsync();
                        return true;
                    }
                    throw new KeyNotFoundException("Sales activity not found or not of type 'ConsultationRequestHandled'");
                }
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                throw new Exception("Error updating sales activity", ex);
            }
        }

        public async Task<bool> UpdateResultTestDriveAsync(int salesActivityId, string result)
        {
            try
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    var salesActivity = await _context.SalesActivityLogs.FirstOrDefaultAsync(sal => sal.ActivityLogId == salesActivityId && sal.ActivityType == "TestDriveRequestHandled");
                    if (salesActivity != null)
                    {
                        salesActivity.Details = result;
                        _context.SalesActivityLogs.Update(salesActivity);
                        await _context.SaveChangesAsync();

                        var getNotificationById = await _context.Notifications
                            .FirstOrDefaultAsync(n => n.NotificationId == salesActivity.NotificationId && n.NotificationType == "TestDriveRequestHandled");

                        if (getNotificationById != null)
                        {
                            if (getNotificationById.TestDriveId != null)
                            {
                                var testDrive = await _context.TestDrives.FirstOrDefaultAsync(td => td.TestDriveId == getNotificationById.TestDriveId);
                                if (testDrive != null)
                                {
                                    testDrive.Status = "Completed";
                                    _context.TestDrives.Update(testDrive);
                                    await _context.SaveChangesAsync();
                                }
                            }
                        }

                        var getSalesPerformance = await _context.SalesPersonPerformances
                            .FirstOrDefaultAsync(sp => sp.SalesPersonId == salesActivity.SalesPersonId && sp.MetricType == "TestDriveCar" && sp.MetricDate == DateOnly.FromDateTime(DateTime.UtcNow));


                        var addSalesPerformance = new SalesPersonPerformance
                        {
                            SalesPersonId = salesActivity.SalesPersonId,
                            MetricType = "TestDriveCar",
                            MetricValue = 1,
                            MetricDate = DateOnly.FromDateTime(DateTime.UtcNow),
                        };
                        if (getSalesPerformance == null)
                        {
                            await _context.SalesPersonPerformances.AddAsync(addSalesPerformance);
                        }
                        else
                        {
                            getSalesPerformance.MetricValue += 1;
                            _context.SalesPersonPerformances.Update(getSalesPerformance);
                        }
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();
                        return true;
                    }
                    throw new KeyNotFoundException("Sales activity not found or not of type 'TestDriveRequestHandled'");
                }
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                throw new Exception("Error updating sales activity", ex);
            }
        }
    }

        public class NotFoundException : Exception
        {
            public NotFoundException(string message) : base(message) { }
        }
}