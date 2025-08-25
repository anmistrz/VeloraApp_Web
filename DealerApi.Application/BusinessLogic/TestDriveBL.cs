using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DealerApi.Application.DTO;
using DealerApi.Application.Interface;
using DealerApi.DAL.Interfaces;
using DealerApi.Entities.Models;

namespace DealerApi.Application.BusinessLogic
{
    public class TestDriveBL : ITestDriveBL
    {
        private readonly ITestDrive _testDriveDAL;

        public TestDriveBL(ITestDrive testDriveDAL)
        {
            _testDriveDAL = testDriveDAL ?? throw new ArgumentNullException(nameof(testDriveDAL), "TestDrive DAL cannot be null");
        }

        public Task<TestDrive> CreateTestDriveGuestAsync(Customer dataCustomer, TestDrive dataTestDrive, DealerCar dataDealerCar)
        {
            try
            {
                if (dataCustomer == null)
                {
                    throw new ArgumentNullException(nameof(dataCustomer), "Customer cannot be null");
                }

                // Validate required fields
                if (string.IsNullOrWhiteSpace(dataCustomer.FirstName))
                    throw new ArgumentException("FirstName is required");
                if (string.IsNullOrWhiteSpace(dataCustomer.LastName))
                    throw new ArgumentException("LastName is required");
                if (string.IsNullOrWhiteSpace(dataCustomer.Email))
                    throw new ArgumentException("Email is required");
                if (string.IsNullOrWhiteSpace(dataCustomer.PhoneNumber))
                    throw new ArgumentException("PhoneNumber is required");
                if (dataDealerCar.DealerId <= 0)
                    throw new ArgumentException("DealerId must be greater than 0");

                var customer = new Customer
                {
                    FirstName = dataCustomer.FirstName,
                    LastName = dataCustomer.LastName,
                    Email = dataCustomer.Email,
                    PhoneNumber = dataCustomer.PhoneNumber,
                    UserName = "Guest",
                    IsGuest = true
                };

                var testDrive = new TestDrive
                {
                    Customer = customer,
                    DealerCarUnitId = dataTestDrive.DealerCarUnitId,
                    AppointmentDate = dataTestDrive.AppointmentDate,
                    Note = dataTestDrive.Note,
                    Status = "Pending"
                };

                Console.WriteLine($"TestDriveServices: Processing request for {dataCustomer.Email}");
                Console.WriteLine($"TestDriveServices: Customer Name: {dataCustomer.FirstName} {dataCustomer.LastName}");
                Console.WriteLine($"TestDriveServices: DealerCarId: {dataDealerCar.DealerCarId}, DealerId: {dataDealerCar.DealerId}");

                var dealerCar = new DealerCar
                {
                    DealerCarId = dataDealerCar.DealerCarId,
                    DealerId = dataDealerCar.DealerId
                };

                var result = _testDriveDAL.CreateTestDriveGuestAsync(customer, testDrive, dealerCar);

                if (result == null)
                {
                    throw new Exception("Failed to create test drive.");
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error creating test drive", ex);
            }
        }

        public async Task<bool> DeleteTestDriveAfterHandledAsync(int testDriveId, DeleteTestDriveRequestDTO deleteTestDriveRequest)
        {
            try
            {
                var body = new DeleteTestDriveRequestDTO
                {
                    SalesPersonId = deleteTestDriveRequest.SalesPersonId,
                    Reason = deleteTestDriveRequest.Reason,
                    TestDriveId  = testDriveId,
                    DealerId = deleteTestDriveRequest.DealerId
                };
                Console.WriteLine($"TestDriveServices: Deleting test drive with ID {testDriveId} for SalesPerson ID {body.SalesPersonId} at Dealer ID {body.DealerId} for reason: {body.Reason}");
                var result = await _testDriveDAL.DeleteTestDriveAfterHandledAsync(testDriveId, body.Reason);
                return result;
            } catch (Exception ex)
            {
                throw new InvalidOperationException("Error deleting test drive after handled", ex);
            }
        }

        public Task<bool> DeleteTestDriveBeforeHandledAsync(int testDriveId, DeleteTestDriveRequestDTO deleteTestDriveRequest)
        {
            try
            {
                Console.WriteLine($"TestDriveServices: Deleting test drive before handled with ID {testDriveId}, SalesPersonId {deleteTestDriveRequest.SalesPersonId}, DealerId {deleteTestDriveRequest.DealerId}, Reason: {deleteTestDriveRequest.Reason}");
                var body = new DeleteTestDriveRequestDTO
                {
                    SalesPersonId = deleteTestDriveRequest.SalesPersonId,
                    Reason = deleteTestDriveRequest.Reason,
                    TestDriveId = testDriveId,
                    DealerId = deleteTestDriveRequest.DealerId
                };

                Console.WriteLine("bodyyyyyy:", JsonSerializer.Serialize(body));

                var result = _testDriveDAL.DeleteTestDriveBeforeHandledAsync(testDriveId, body.SalesPersonId, body.DealerId, body.Reason);
                return result;
            } catch (Exception ex)
            {
                throw new InvalidOperationException("Error deleting test drive before handled", ex);
            }
        }

        public async Task<IEnumerable<TestDriveRequestDTO>> GetTestDrivesBySalesPersonIdAsync(int salesPersonId)
        {
            try
            {
                var result = await _testDriveDAL.GetTestDrivesBySalesPersonIdAsync(salesPersonId);
                if (result == null || !result.Any())
                {
                    throw new KeyNotFoundException("No test drives found for the specified sales person");
                }
                var convertDTO = result.Select(value => new TestDriveRequestDTO
                {
                    TestDriveId = value.Item1.TestDriveId,
                    CustomerName = $"{value.Item2.FirstName} {value.Item2.LastName}",
                    CarName = value.Item3.CarModel,
                    AppointmentDate = value.Item1.AppointmentDate,
                    Note = value.Item1.Note,
                    Status = value.Item1.Status
                });
                return convertDTO.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error retrieving test drives by sales person ID", ex);
            }
        }
    }
}