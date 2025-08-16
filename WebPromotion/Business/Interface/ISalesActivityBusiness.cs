using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebPromotion.Services.DTO;

namespace WebPromotion.Business.Interface
{
    public interface ISalesActivityBusiness
    {
        public Task<IEnumerable<SalesActivityLogHandledRequestClientDTO>> getSalesActivityBySalesPerson(int id, string activityType);
        public Task<SalesActivityClientDTO> getSalesActivityConsultationById(int id);
        public Task<SalesActivityClientDTO> getSalesActivityTestDriveById(int id);
        public Task<bool> UpdateResultConsultationAsync(int salesActivityId, string result);
        public Task<bool> UpdateResultTestDriveAsync(int salesActivityId, string result);
    }
}