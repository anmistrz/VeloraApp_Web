using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary.DAL.Interfaces;
using DealerApi.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.DAL.DAL
{
    public class DashboardSalesPersonDAL : IDashboardSalesPerson
    {
        private readonly DealerRndDBContext _context;

        public DashboardSalesPersonDAL(DealerRndDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SalesActivityThisMonth>> GetDetailActivitySalesPerformanceByDayInThisMonthAsync(int salesPersonId)
        {
            try
            {
                return await _context.SalesActivityLogs
                    .Where(c => c.SalesPersonId == salesPersonId && c.ActivityDate.Month == DateTime.Now.Month && c.ActivityDate.Year == DateTime.Now.Year)
                    .GroupBy(c => c.ActivityDate.Day)
                    .Select(g => new SalesActivityThisMonth
                    {
                        Day = new DateTime(DateTime.Now.Year, DateTime.Now.Month, g.Key).ToString("dd MMMM"),
                        TotalCount = g.Count()
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Handle exceptions
                throw new Exception("Error fetching daily sales performance", ex);
            }
        }

        public async Task<double> GetSalesPerformanceReviewAsync(int salesPersonId)
        {
            try
            {
                var totalConsultations = await GetTotalConsultationHandledSalesPersonAsync(salesPersonId);
                var totalTestDrives = await GetTotalTestDriveHandledSalesPersonAsync(salesPersonId);
                var consultationTargets = await _context.SalesPersonPerformances
                    .Where(t => t.SalesPersonId == salesPersonId && t.MetricType == "ConsultationCar")
                    .FirstAsync();
                var testDriveTargets = await _context.SalesPersonPerformances
                    .Where(t => t.SalesPersonId == salesPersonId && t.MetricType == "TestDriveCar")
                    .FirstAsync();

                // Calculate performance metrics
                var performanceReview = (totalConsultations + totalTestDrives) / (float)(consultationTargets.MetricValue + testDriveTargets.MetricValue);
                Console.WriteLine($"Sales Performance Review for SalesPersonId {salesPersonId}: {performanceReview}");
                // convert to percentage
                performanceReview *= 100;
                return performanceReview;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                throw new Exception("Error calculating sales performance review", ex);
            }
        }

        public async Task<double> GetSalesRatingAsync(int salesPersonId)
        {
           try
           {
               var result = await _context.CustomerRatings
                   .Where(cr => cr.SalesPersonId == salesPersonId && cr.RatingType == "SalespersonService")
                   .AverageAsync(cr => cr.RatingValue);

               return result;
           }
           catch (Exception ex)
           {
               // Handle exceptions
               throw new Exception("Error fetching sales rating", ex);
           }
        }

        public async Task<int> GetTotalConsultationHandledSalesPersonAsync(int salesPersonId)
        {
            var result = await _context.SalesActivityLogs
                .CountAsync(c => c.SalesPersonId == salesPersonId && c.ActivityType == "ConsultationCompleted");
            return result;
        }

        public async Task<IEnumerable<SalesActivitySummary>> GetTotalSalesActivityConsultationByMonthAsync(int salesPersonId)
        {
            var result = await _context.SalesActivityLogs
                .Where(c => c.SalesPersonId == salesPersonId && c.ActivityType == "ConsultationCompleted")
                .GroupBy(c => new { c.ActivityDate.Year, c.ActivityDate.Month })
                .Select(g => new SalesActivitySummary
                {
                    Month = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month),
                    TotalCount = g.Count()
                })
                .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<SalesActivitySummary>> GetTotalSalesActivityTestDriveByMonthAsync(int salesPersonId)
        {
            var result = await _context.SalesActivityLogs
                .Where(c => c.SalesPersonId == salesPersonId && c.ActivityType == "TestDriveCompleted")
                .GroupBy(c => new { c.ActivityDate.Year, c.ActivityDate.Month })
                .Select(g => new SalesActivitySummary
                {
                    Month = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month),
                    TotalCount = g.Count()
                })
                .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<(int Target, string Description)>> GetTotalTargetConsultationHandledSalesPersonAsync(int salesPersonId)
        {
            var result = await _context.SalesActivityLogs
                .Where(c => c.SalesPersonId == salesPersonId && c.ActivityType == "ConsultationCompleted" && c.ActivityDate.Month == DateTime.Now.Month && c.ActivityDate.Year == DateTime.Now.Year)
                .GroupBy(c => new { c.ActivityDate.Year, c.ActivityDate.Month })
                .Select(g => new
                {
                    Target = g.Count(),
                    Description = "Monthly Consultation Target"
                })
                .ToListAsync();

            return result.Select(r => (r.Target, r.Description));
        }

        public async Task<IEnumerable<(int Target, string Description)>> GetTotalTargetTestDriveHandledSalesPersonAsync(int salesPersonId)
        {
            // This Month 
            var result = await _context.SalesActivityLogs
                .Where(c => c.SalesPersonId == salesPersonId && c.ActivityType == "TestDriveCompleted" && c.ActivityDate.Month == DateTime.Now.Month && c.ActivityDate.Year == DateTime.Now.Year)
                .GroupBy(c => new { c.ActivityDate.Year, c.ActivityDate.Month })
                .Select(g => new
                {
                    Target = g.Count(),
                    Description = "Monthly Test Drive Target"
                })
                .ToListAsync();

            return result.Select(r => (r.Target, r.Description));
        }

        public async Task<int> GetTotalTestDriveHandledSalesPersonAsync(int salesPersonId)
        {
            var result = await _context.SalesActivityLogs
                .CountAsync(c => c.SalesPersonId == salesPersonId && c.ActivityType == "TestDriveCompleted");
            return result;
        }
    }
        
}