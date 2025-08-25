using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration.Annotations;
using ClassLibrary.DAL.Interfaces;
using DealerApi.Application.DTO;
using DealerApi.Application.Interface;

namespace DealerApi.Application.BusinessLogic
{
    public class DashboardSalesPersonBL : IDashboardSalesPersonBL
    {
        private readonly IDashboardSalesPerson _dashboardSalesPerson;
        public DashboardSalesPersonBL(IDashboardSalesPerson dashboardSalesPerson)
        {
            _dashboardSalesPerson = dashboardSalesPerson;
        }

        public async Task<IEnumerable<SalesActivityThisMonthDTO>> GetDetailActivitySalesPerformanceByDayInThisMonthAsync(int salesPersonId)
        {
            try
            {
                var result = await _dashboardSalesPerson.GetDetailActivitySalesPerformanceByDayInThisMonthAsync(salesPersonId);
                var convertDTO = result.Select(x => new SalesActivityThisMonthDTO
                {
                    day = x.Day,
                    totalCount = x.TotalCount
                });
                return convertDTO;
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new Exception("Error fetching sales performance by day in this month", ex);
            }
        }

        public Task<double> GetSalesPerformanceReviewAsync(int salesPersonId)
        {
            try
            {
                var result = _dashboardSalesPerson.GetSalesPerformanceReviewAsync(salesPersonId);
                return result;
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new Exception("Error fetching sales performance review", ex);
            }
        }

        public Task<double> GetSalesRatingAsync(int salesPersonId)
        {
            try
            {
                var result = _dashboardSalesPerson.GetSalesRatingAsync(salesPersonId);
                return result;
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new Exception("Error fetching sales rating", ex);
            }
        }

        public async Task<int> GetTotalConsultationHandledSalesPersonAsync(int salesPersonId)
        {
           try
           {
               var result = await _dashboardSalesPerson.GetTotalConsultationHandledSalesPersonAsync(salesPersonId);
               return result;
           }
           catch (Exception ex)
           {
               // Handle exceptions as needed
               throw new Exception("Error fetching total consultations handled", ex);
           }
        }

        public async Task<IEnumerable<SalesActivitySummaryDTO>> GetTotalSalesActivityConsultationByMonthAsync(int salesPersonId)
        {
            try
            {
                var result = await _dashboardSalesPerson.GetTotalSalesActivityConsultationByMonthAsync(salesPersonId);
                var convertDTO = result.Select(x => new SalesActivitySummaryDTO
                {
                    Month = x.Month,
                    TotalCount = x.TotalCount,
                });
                return convertDTO;
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new Exception("Error fetching sales activity consultation by month", ex);
            }
        }

        public async Task<IEnumerable<SalesActivitySummaryDTO>> GetTotalSalesActivityTestDriveByMonthAsync(int salesPersonId)
        {
            try
            {
                var result = await _dashboardSalesPerson.GetTotalSalesActivityTestDriveByMonthAsync(salesPersonId);
                var convertDTO = result.Select(x => new SalesActivitySummaryDTO
                {
                    Month = x.Month,
                    TotalCount = x.TotalCount,
                });
                return convertDTO;
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new Exception("Error fetching sales activity test drive by month", ex);
            }
        }

        public async Task<IEnumerable<SalesActivityPersonPerformanceDTO>> GetTotalTargetConsultationHandledSalesPersonAsync(int salesPersonId)
        {
            try
            {
                var result = await _dashboardSalesPerson.GetTotalTargetConsultationHandledSalesPersonAsync(salesPersonId);
                var convertDTO = result.Select(x => new SalesActivityPersonPerformanceDTO
                {
                    Title = x.Description,
                    Value = x.Target
                });
                return convertDTO;
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new Exception("Error fetching total target consultations handled", ex);
            }
        }

        public async Task<IEnumerable<SalesActivityPersonPerformanceDTO>> GetTotalTargetTestDriveHandledSalesPersonAsync(int salesPersonId)
        {
            try
            {
                var result = await _dashboardSalesPerson.GetTotalTargetTestDriveHandledSalesPersonAsync(salesPersonId);
                var convertDTO = result.Select(x => new SalesActivityPersonPerformanceDTO
                {
                    Title = x.Description,
                    Value = x.Target
                });
                return convertDTO;
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new Exception("Error fetching total target test drives handled", ex);
            }
        }

        public async Task<int> GetTotalTestDriveHandledSalesPersonAsync(int salesPersonId)
        {
            try
            {
                var result = await _dashboardSalesPerson.GetTotalTestDriveHandledSalesPersonAsync(salesPersonId);
                return result;
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new Exception("Error fetching total test drives handled", ex);
            }
        }
    }
}