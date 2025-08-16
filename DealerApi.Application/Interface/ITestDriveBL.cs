using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerApi.Application.DTO;
using DealerApi.Entities.Models;

namespace DealerApi.Application.Interface
{
    public interface ITestDriveBL
    {
        public Task<TestDrive> CreateTestDriveGuestAsync(
            Customer dataCustomer,
            TestDrive testDrive,
            DealerCar dataDealerCar
        );

        public Task<IEnumerable<TestDriveRequestDTO>> GetTestDrivesBySalesPersonIdAsync(int salesPersonId);
        public Task<bool> DeleteTestDriveAfterHandledAsync(int consultHistoryId, DeleteTestDriveRequestDTO deleteTestDriveRequest);
        public Task<bool> DeleteTestDriveBeforeHandledAsync(int consultHistoryId, DeleteTestDriveRequestDTO deleteTestDriveRequest);
    }
}