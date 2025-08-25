using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebPromotion.Services.DTO;

namespace WebPromotion.Business.Interface
{
    public interface IDashboardSalesPersonBusiness
    {
        public Task<int> GetTotalConsultationHandledSalesPersonAsync(int salesPersonId);
        public Task<int> GetTotalTestDriveHandledSalesPersonAsync(int salesPersonId);
        public Task<List<SalesActivityPersonPerformanceClientDTO>> GetTotalTargetConsultationHandledSalesPersonAsync(int salesPersonId);
        public Task<List<SalesActivityPersonPerformanceClientDTO>> GetTotalTargetTestDriveHandledSalesPersonAsync(int salesPersonId);
        public Task<List<SalesActivitySummaryClientDTO>> GetTotalSalesActivityConsultationByMonthAsync(int salesPersonId);
        public Task<List<SalesActivitySummaryClientDTO>> GetTotalSalesActivityTestDriveByMonthAsync(int salesPersonId);
        public Task<double> GetSalesPerformanceReviewAsync(int salesPersonId);
        public Task<double> GetSalesRatingAsync(int salesPersonId);
        public Task<List<SalesActivityThisMonthClientDTO>> GetDetailActivitySalesPerformanceByDayInThisMonthAsync(int salesPersonId);
    }
}