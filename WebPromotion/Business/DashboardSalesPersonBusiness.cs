using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WebPromotion.Business.Interface;
using WebPromotion.Services.DTO;
using WebPromotion.Services.Interface;

namespace WebPromotion.Business
{
    public class DashboardSalesPersonBusiness : IDashboardSalesPersonBusiness
    {
        private readonly IDashboardSalesPersonServices _dashboardSalesPersonServices;

        public DashboardSalesPersonBusiness(IDashboardSalesPersonServices dashboardSalesPersonServices)
        {
            _dashboardSalesPersonServices = dashboardSalesPersonServices;
        }

        public async Task<List<SalesActivityThisMonthClientDTO>> GetDetailActivitySalesPerformanceByDayInThisMonthAsync(int salesPersonId)
        {
            try
            {
                return await _dashboardSalesPersonServices.GetDetailActivitySalesPerformanceByDayInThisMonthAsync(salesPersonId);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new InvalidOperationException($"An error occurred while fetching sales performance details by day in this month: {ex.Message}", ex);
            }
        }

        public async Task<double> GetSalesPerformanceReviewAsync(int salesPersonId)
        {
            try
            {
                return await _dashboardSalesPersonServices.GetSalesPerformanceReviewAsync(salesPersonId);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new InvalidOperationException($"An error occurred while fetching sales performance review: {ex.Message}", ex);
            }
        }

        public async  Task<double> GetSalesRatingAsync(int salesPersonId)
        {
            try
            {
                return await _dashboardSalesPersonServices.GetSalesRatingAsync(salesPersonId);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new InvalidOperationException($"An error occurred while fetching sales rating: {ex.Message}", ex);
            }
        }

        public async Task<int> GetTotalConsultationHandledSalesPersonAsync(int salesPersonId)
        {
            try
            {
                return await _dashboardSalesPersonServices.GetTotalConsultationHandledSalesPersonAsync(salesPersonId);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new InvalidOperationException($"An error occurred while fetching total consultation handled: {ex.Message}", ex);
            }
        }

        public async Task<List<SalesActivitySummaryClientDTO>> GetTotalSalesActivityConsultationByMonthAsync(int salesPersonId)
        {
            try
            {
                return await _dashboardSalesPersonServices.GetTotalSalesActivityConsultationByMonthAsync(salesPersonId);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new InvalidOperationException($"An error occurred while fetching total sales activity consultation by month: {ex.Message}", ex);
            }
        }

        public async Task<List<SalesActivitySummaryClientDTO>> GetTotalSalesActivityTestDriveByMonthAsync(int salesPersonId)
        {
            try
            {
                return await _dashboardSalesPersonServices.GetTotalSalesActivityTestDriveByMonthAsync(salesPersonId);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new InvalidOperationException($"An error occurred while fetching total sales activity test drive by month: {ex.Message}", ex);
            }
        }   
    
        public async Task<List<SalesActivityPersonPerformanceClientDTO>> GetTotalTargetConsultationHandledSalesPersonAsync(int salesPersonId)
        {
            try
            {
                var result = await _dashboardSalesPersonServices.GetTotalTargetConsultationHandledSalesPersonAsync(salesPersonId);
                return result;
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new InvalidOperationException($"An error occurred while fetching total target consultation handled: {ex.Message}", ex);
            }
        }

        public async Task<List<SalesActivityPersonPerformanceClientDTO>> GetTotalTargetTestDriveHandledSalesPersonAsync(int salesPersonId)
        {
            try
            {
                var result = await _dashboardSalesPersonServices.GetTotalTargetTestDriveHandledSalesPersonAsync(salesPersonId);
                Console.WriteLine($"Result from API BUSINESS: {JsonSerializer.Serialize(result)}");
                return result;
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new InvalidOperationException($"An error occurred while fetching total target test drive handled: {ex.Message}", ex);
            }
        }

        public async Task<int> GetTotalTestDriveHandledSalesPersonAsync(int salesPersonId)
        {
            try
            {
                return await _dashboardSalesPersonServices.GetTotalTestDriveHandledSalesPersonAsync(salesPersonId);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new InvalidOperationException($"An error occurred while fetching total test drive handled: {ex.Message}", ex);
            }
        }
    }
}