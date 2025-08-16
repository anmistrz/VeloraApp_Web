using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerApi.Application.DTO;
using WebPromotion.Services.DTO;

namespace WebPromotion.Business.Interface
{
    public interface ITestDriveBusiness
    {
        public Task<Models.TestDrive> InsertTestDriveGuest(TestDriveInsertGuestDTO model);
        public Task<IEnumerable<TestDriveRequestClientDTO>> GetTestDriveRequestBySalesPerson(string salesPersonId);
        public Task<bool> DeleteTestDriveAfterHandled(int consultHistoryId, DeleteTestDriveRequestClientDTO model);
        public Task<bool> DeleteTestDriveBeforeHandled(int consultHistoryId, DeleteTestDriveRequestClientDTO model);
    }
}