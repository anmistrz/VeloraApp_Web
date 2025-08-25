using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassLibrary.DAL.Interfaces
{
    public class SalesActivitySummary
    {
        public string Month { get; set; }
        public int TotalCount { get; set; }
    }

    public class SalesActivityThisMonth
    {
        public string Day { get; set; }
        public int TotalCount { get; set; }
    }

    public interface IDashboardSalesPerson
    {
        Task<int> GetTotalConsultationHandledSalesPersonAsync(int salesPersonId);
        Task<int> GetTotalTestDriveHandledSalesPersonAsync(int salesPersonId);
        Task<IEnumerable<(int Target, string Description)>> GetTotalTargetConsultationHandledSalesPersonAsync(int salesPersonId);
        Task<IEnumerable<(int Target, string Description)>> GetTotalTargetTestDriveHandledSalesPersonAsync(int salesPersonId);
        Task<IEnumerable<SalesActivitySummary>> GetTotalSalesActivityConsultationByMonthAsync(int salesPersonId);
        Task<IEnumerable<SalesActivitySummary>> GetTotalSalesActivityTestDriveByMonthAsync(int salesPersonId);
        Task<double> GetSalesPerformanceReviewAsync(int salesPersonId);
        Task<double> GetSalesRatingAsync(int salesPersonId);

        Task<IEnumerable<SalesActivityThisMonth>> GetDetailActivitySalesPerformanceByDayInThisMonthAsync(int salesPersonId);
    }
}