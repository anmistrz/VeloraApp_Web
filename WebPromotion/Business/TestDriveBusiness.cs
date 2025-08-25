using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DealerApi.Application.DTO;
using WebPromotion.Business.Interface;
using WebPromotion.Models;
using WebPromotion.Services;
using WebPromotion.Services.DTO;
using WebPromotion.ViewModels.TestDriveView.TestDriveGuestViewModel;

namespace WebPromotion.Business
{
    public class TestDriveBusiness : ITestDriveBusiness
    {
        private readonly ITestDriveService _testDriveService;

        public TestDriveBusiness(ITestDriveService testDriveService)
        {
            _testDriveService = testDriveService;
        }

        public Task<bool> DeleteTestDriveAfterHandled(int testDriveId, DeleteTestDriveRequestClientDTO model)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentException("Model must not be null", nameof(model));
                }
                var mappedModel = new DeleteTestDriveRequestDTO
                {
                    TestDriveId = model.TestDriveId,
                    Reason = model.Reason,
                    SalesPersonId = model.SalesPersonId,
                    DealerId = model.DealerId
                };
                var resultTask = _testDriveService.DeleteTestDriveAfterHandledAsync(testDriveId, mappedModel);
                resultTask.Wait();
                if (!resultTask.Result)
                {
                    // Not found or could not be deleted
                    return Task.FromResult(false);
                }
                return Task.FromResult(true);
            }
            catch (InvalidOperationException ex)
            {
                // Unexpected error
                throw new Exception("An error occurred while deleting test drive after handled.", ex);
            }
        }

        public async Task<bool> DeleteTestDriveBeforeHandled(int testDriveId, DeleteTestDriveRequestClientDTO model)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentException("Model must not be null", nameof(model));
                }
                Console.WriteLine($"De bUSINESS leting test drive before handled with ID: {testDriveId}, SalesPersonId: {model.SalesPersonId}, DealerId: {model.DealerId}, Reason: {model.Reason}");
                var mappedModel = new DeleteTestDriveRequestDTO
                {
                    TestDriveId = model.TestDriveId,
                    Reason = model.Reason,
                    SalesPersonId = model.SalesPersonId,
                    DealerId = model.DealerId
                };
                var resultTask = await _testDriveService.DeleteTestDriveBeforeHandledAsync(testDriveId, mappedModel);
                if (!resultTask)
                {
                    // Not found or could not be deleted
                    return false;
                }
                return true;
            }
            catch (InvalidOperationException ex)
            {
                // Unexpected error
                throw new Exception("An error occurred while deleting test drive before handled.", ex);
            }
        }

        public async Task<IEnumerable<TestDriveRequestClientDTO>> GetTestDriveRequestBySalesPerson(string salesPersonId)
        {
            try
            {
                var result = await _testDriveService.GetTestDriveRequestBySalesPersonAsync(salesPersonId);
                var convertDTO = new List<TestDriveRequestClientDTO>();
                foreach (var item in result)
                {
                    convertDTO.Add(new TestDriveRequestClientDTO
                    {
                        TestDriveId = item.TestDriveId,
                        CustomerName = item.CustomerName,
                        CarName = item.CarName,
                        AppointmentDate = item.AppointmentDate,
                        Note = item.Note,
                        Status = item.Status
                    });
                }
                return convertDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving test drive requests by sales person.", ex);
            }
        }

        public Task<TestDrive> InsertTestDriveGuest(TestDriveGuestViewModels model, int carId)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentException("Model must not be null", nameof(model));
                }

                var dataBody = new TestDriveInsertGuestDTO
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    AppointmentDate = model.ConsultDate,
                    Note = model.Note,
                    DealerId = model.DealerId,
                    DealerCarUnitId = int.Parse(model.DealerCarUnitId),
                    CarId = carId
                };
                Console.WriteLine($"Data TEST DRIVE to be sent business: {JsonSerializer.Serialize(dataBody)}");
                return _testDriveService.CreateAsyncTestDriveGuest(dataBody);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while inserting the test drive guest.", ex);

            }
        }
    }
}