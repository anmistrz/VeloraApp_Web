using System;
using DealerApi.Entities.Models;

namespace DealerApi.DAL.Interfaces 
{
    public interface ITestDrive : ICrud<TestDrive>
    {
        public Task<TestDrive> CreateTestDriveGuestAsync(
            Customer dataCustomer,
            TestDrive testDrive,
            DealerCar dataDealerCar
        );

        public Task<IEnumerable<(TestDrive, Customer, Car)>> GetTestDrivesBySalesPersonIdAsync(int salesPersonId);
        public Task<bool> DeleteTestDriveAfterHandledAsync(int testDriveId, string reason);
        public Task<bool> DeleteTestDriveBeforeHandledAsync(int testDriveId, int salesPersonId, int dealerId, string reason);

    }
}