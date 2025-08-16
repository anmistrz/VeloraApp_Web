using System;
using DealerApi.Application.DTO;
using WebPromotion.Models;
using WebPromotion.Services.DTO;

namespace WebPromotion.Services
{
    public interface ITestDriveService : ICrud<Models.TestDrive>
    {
        public Task<Models.TestDrive> CreateAsyncTestDriveGuest(TestDriveInsertGuestDTO model);
        public Task<IEnumerable<TestDriveRequestClientDTO>> GetTestDriveRequestBySalesPersonAsync(string salesPersonId);
        public Task<bool> DeleteTestDriveAfterHandledAsync(int consultHistoryId, DeleteTestDriveRequestDTO model);
        public Task<bool> DeleteTestDriveBeforeHandledAsync(int consultHistoryId, DeleteTestDriveRequestDTO model);
    }
}