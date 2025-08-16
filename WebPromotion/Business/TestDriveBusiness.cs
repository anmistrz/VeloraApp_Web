using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerApi.Application.DTO;
using WebPromotion.Business.Interface;
using WebPromotion.Models;
using WebPromotion.Services;
using WebPromotion.Services.DTO;

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
                // Map DeleteTestDriveRequestClientDTO to DeleteTestDriveRequestDTO
                var mappedModel = new DeleteTestDriveRequestDTO
                {
                    // Map properties from the client DTO to the service DTO
                    TestDriveId = model.TestDriveId,
                    Reason = model.Reason,
                    SalesPersonId = model.SalesPersonId,
                    DealerId = model.DealerId
                };
                return _testDriveService.DeleteTestDriveAfterHandledAsync(testDriveId, mappedModel);
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed, e.g., logging
                throw new Exception("An error occurred while deleting test drive after handled.", ex);
            }
        }

        public Task<bool> DeleteTestDriveBeforeHandled(int testDriveId, DeleteTestDriveRequestClientDTO model)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentException("Model must not be null", nameof(model));
                }
                // Map DeleteTestDriveRequestClientDTO to DeleteTestDriveRequestDTO
                var mappedModel = new DeleteTestDriveRequestDTO
                {
                    TestDriveId = model.TestDriveId,
                    Reason = model.Reason,
                    SalesPersonId = model.SalesPersonId,
                    DealerId = model.DealerId
                };
                return _testDriveService.DeleteTestDriveBeforeHandledAsync(testDriveId, mappedModel);
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed, e.g., logging
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

        public Task<TestDrive> InsertTestDriveGuest(TestDriveInsertGuestDTO model)
        {
            try
            {
                return _testDriveService.CreateAsyncTestDriveGuest(model);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while inserting the test drive guest.", ex);

            }
        }
    }
}