using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerApi.Application.DTO;
using DealerApi.Entities.Models;

namespace DealerApi.Application.Interface
{
    public interface ISalesActivityLogBL
    {
        public Task<IEnumerable<SalesActivityLogHandledRequestDTO>> GetAllSalesActivitiesBySalesPersonAsync(int salesPersonId, string NotificationType);
        public Task<bool> UpdateResultConsultationAsync(int salesActivityId, string result);
        public Task<bool> UpdateResultTestDriveAsync(int salesActivityId, string result);
        public Task<SalesActivityDTO> GetSalesActivityConsultationByIdAsync(int consultationId);
        public Task<SalesActivityDTO> GetSalesActivityTestDriveByIdAsync(int testDriveId);
    }
}