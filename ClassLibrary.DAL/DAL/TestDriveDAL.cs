using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DealerApi.DAL.Context;
using DealerApi.DAL.Interfaces;
using DealerApi.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DealerApi.DAL.DAL
{
    public class TestDriveDAL : ITestDrive
    {
        private readonly DealerRndDBContext _context;

        public TestDriveDAL(DealerRndDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<TestDrive> CreateAsync(TestDrive entity)
        {
            throw new NotImplementedException();
        }

        public async Task<TestDrive> CreateTestDriveGuestAsync(Customer dataCustomer, TestDrive testDrive, DealerCar dataDealerCar)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                Console.WriteLine($"TestDriveDAL: Processing request for {dataCustomer.Email}");
                var validateCustomer = _context.Customers
                    .FirstOrDefault(c => c.Email == dataCustomer.Email && c.IsGuest);

                Console.WriteLine($"TestDriveDAL: Validating customer: {dataCustomer.Email}");
                Console.WriteLine($"TestDriveDAL: Found customer: {validateCustomer?.Email}");
                Console.WriteLine($"TestDriveDAL: Customer details: {validateCustomer?.CustomerId}, {validateCustomer?.FirstName}, {validateCustomer?.LastName}");

                if (validateCustomer == null)
                {
                    var customer = new CustomerTemp
                    {
                        FirstName = dataCustomer.FirstName,
                        LastName = dataCustomer.LastName,
                        Email = dataCustomer.Email,
                        PhoneNumber = dataCustomer.PhoneNumber,
                        UserName = "Guest",
                    };

                    Console.WriteLine($"Customer created: {customer.FirstName} {customer.LastName} - {customer.Email}");

                    _context.CustomerTemps.Add(customer);
                    await _context.SaveChangesAsync();

                    var findCustomer = await _context.CustomerTemps
                        .FirstOrDefaultAsync(c => c.Email == dataCustomer.Email);
                    Console.WriteLine($"TestDriveDAL: Found customer: {findCustomer?.Email}");

                    if (findCustomer == null)
                    {
                        throw new Exception("Failed to create customer.");
                    }
                    dataCustomer.CustomerId = findCustomer.CustomerTempId;
                }

                var dtTestDrive = new TestDrive
                {
                    CustomerId = validateCustomer != null ? validateCustomer.CustomerId : dataCustomer.CustomerId,
                    DealerCarUnitId = testDrive.DealerCarUnitId,
                    Status = testDrive.Status ?? "Pending",
                    Note = testDrive.Note,
                    AppointmentDate = testDrive.AppointmentDate
                };

                Console.WriteLine($"Creating TestDrive: {dtTestDrive.DealerCarUnitId}, CustomerId: {dtTestDrive.CustomerId}, Status: {dtTestDrive.Status}");

                _context.TestDrives.Add(dtTestDrive);
                await _context.SaveChangesAsync();

                if (dtTestDrive.TestDriveId <= 0)
                {
                    throw new Exception("Failed to create test drive.");
                }


                var dtNotification = new Notification
                {
                    CustomerId = validateCustomer != null ? validateCustomer.CustomerId : dataCustomer.CustomerId,
                    NotificationType = "TestDriveRequest",
                    DealerId = dataDealerCar.DealerId,
                    TestDriveId = dtTestDrive.TestDriveId,
                    Message = $"Test drive scheduled for {dataDealerCar.CarId} on {dtTestDrive.AppointmentDate}",
                    IsRead = false,
                    Priority = 1,
                    CreatedAt = DateTime.Now
                };

                _context.Notifications.Add(dtNotification);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                var result = new TestDrive
                {
                    TestDriveId = dtTestDrive.TestDriveId,
                    CustomerId = dtTestDrive.CustomerId,
                    DealerCarUnitId = dtTestDrive.DealerCarUnitId,
                    Status = dtTestDrive.Status,
                    Note = dtTestDrive.Note,
                    AppointmentDate = dtTestDrive.AppointmentDate
                };

                return result;
            }
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TestDrive>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TestDrive> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<TestDrive> UpdateAsync(TestDrive entity)
        {
            throw new NotImplementedException();
        }
    }
}