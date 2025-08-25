using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerApi.Application.DTO;

namespace DealerApi.Application.Interface
{
    public interface IDashboardSalesPersonBL
    {
        public Task<int> GetTotalConsultationHandledSalesPersonAsync(int salesPersonId);
        public Task<int> GetTotalTestDriveHandledSalesPersonAsync(int salesPersonId);
        public Task<IEnumerable<SalesActivityPersonPerformanceDTO>> GetTotalTargetConsultationHandledSalesPersonAsync(int salesPersonId);
        public Task<IEnumerable<SalesActivityPersonPerformanceDTO>> GetTotalTargetTestDriveHandledSalesPersonAsync(int salesPersonId);
        public Task<IEnumerable<SalesActivitySummaryDTO>> GetTotalSalesActivityConsultationByMonthAsync(int salesPersonId);
        public Task<IEnumerable<SalesActivitySummaryDTO>> GetTotalSalesActivityTestDriveByMonthAsync(int salesPersonId);
        public Task<double> GetSalesPerformanceReviewAsync(int salesPersonId);
        public Task<double> GetSalesRatingAsync(int salesPersonId);
        public Task<IEnumerable<SalesActivityThisMonthDTO>> GetDetailActivitySalesPerformanceByDayInThisMonthAsync(int salesPersonId);

    }
}