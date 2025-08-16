using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebPromotion.Models;
using WebPromotion.Services.DTO;

namespace WebPromotion.Services.Interface
{
    public interface ISalesActivityService
    {
        public Task<List<SalesActivityLogHandledRequestClientDTO>> getSalesActivityBySalesPersonIdAsync(int salesPersonId, string activityType);
        public Task<SalesActivityLog> getSalesActivityConsultationByIdAsync(int consultationId);
        public Task<SalesActivityLog> getSalesActivityTestDriveByIdAsync(int testDriveId);
        public Task<bool> UpdateResultConsultationAsync(int salesActivityId, string result);
        public Task<bool> UpdateResultTestDriveAsync(int salesActivityId, string result);

    }
}