using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerApi.Entities.Models;

namespace ClassLibrary.DAL.Interfaces
{
    public interface ISalesActivityLog
    {
        public Task<IEnumerable<(SalesActivityLog, Customer, Car)>> GetAllSalesActivitiesBySalesPersonAsync(int salesPersonId, string NotificationType);
        public Task<bool> UpdateResultConsultationAsync(int salesActivityId, string result);
        public Task<bool> UpdateResultTestDriveAsync(int salesActivityId, string result);
        public Task<SalesActivityLog> GetSalesActivityConsultationById(int consultationId);
        public Task<SalesActivityLog> GetSalesActivityTestDriveById(int testDriveId);
    }
}