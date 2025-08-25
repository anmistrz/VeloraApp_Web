using DealerApi.Application.DTO;
using DealerApi.Application.Interface;
using DealerApi.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DealerApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestDriveController : ControllerBase
    {
        private readonly ITestDriveBL _testDriveBL;

        public TestDriveController(ITestDriveBL testDriveBL)
        {
            _testDriveBL = testDriveBL;
        }

        [HttpPost("create-guest")]
        public async Task<IActionResult> CreateTestDriveGuest([FromBody] TestDriveGuestDTO testDriveGuestDto)
        {
            try
            {
                if (testDriveGuestDto == null)
                {
                    return BadRequest(new { error = "Request body cannot be null or empty" });
                }

                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value != null && x.Value.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                        );
                    return BadRequest(new { errors });
                }

                var dtCustomer = new Customer
                {
                    FirstName = testDriveGuestDto.FirstName,
                    LastName = testDriveGuestDto.LastName,
                    Email = testDriveGuestDto.Email,
                    PhoneNumber = testDriveGuestDto.PhoneNumber,
                    IsGuest = true // Assuming this is a guest
                };

                var dtTestDrive = new TestDrive
                {
                    DealerCarUnitId = testDriveGuestDto.DealerCarUnitId,
                    AppointmentDate = testDriveGuestDto.AppointmentDate,
                    Note = testDriveGuestDto.Note,
                    Status = "Pending" // Default status for guest test drives
                };

                var dtDealerCar = new DealerCar
                {
                    DealerId = testDriveGuestDto.DealerId
                };

                var result = await _testDriveBL.CreateTestDriveGuestAsync(dtCustomer, dtTestDrive, dtDealerCar);

                if (result == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create test drive.");
                }

                var response = new
                {
                    message = "Test drive created successfully",
                    data = result
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }

        }


        [HttpGet("by-salesperson/{salesPersonId}")]
        public async Task<IActionResult> GetTestDrivesBySalesPersonId(int salesPersonId)
        {
            try
            {
                var result = await _testDriveBL.GetTestDrivesBySalesPersonIdAsync(salesPersonId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("delete-after-handled/{testDriveId}")]
        [Authorize(Roles = "salesPerson")]
        public async Task<IActionResult> DeleteTestDriveAfterHandled(int testDriveId, [FromBody] DeleteTestDriveRequestDTO deleteTestDriveRequest)
        {
            try
            {
                if (deleteTestDriveRequest == null)
                {
                    return BadRequest(new { error = "Request body cannot be null or empty" });
                }

                var result = await _testDriveBL.DeleteTestDriveAfterHandledAsync(testDriveId, deleteTestDriveRequest);
                if (!result)
                {
                    return NotFound(new { error = "Test drive not found or could not be deleted" });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpPost("delete-before-handled/{testDriveId}")]
        [Authorize(Roles = "salesPerson")]
        public async Task<IActionResult> DeleteTestDriveBeforeHandled(int testDriveId, [FromBody] DeleteTestDriveRequestDTO deleteTestDriveRequest)
        {
            try
            {
                if (deleteTestDriveRequest == null)
                {
                    return BadRequest(new { error = "Request body cannot be null or empty" });
                }

                Console.WriteLine($"Deleting test drive with ID: {testDriveId}, SalesPersonId: {deleteTestDriveRequest.SalesPersonId}, DealerId: {deleteTestDriveRequest.DealerId}, Reason: {deleteTestDriveRequest.Reason}");

                Console.WriteLine($"ModelState is valid: {ModelState.IsValid}");

                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value != null && x.Value.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                        );
                    return BadRequest(new { errors });
                }

                Console.WriteLine($"DeleteTestDriveBeforeHandled: SalesPersonId: {deleteTestDriveRequest.SalesPersonId}, DealerId: {deleteTestDriveRequest.DealerId}, Reason: {deleteTestDriveRequest.Reason}");

                var result = await _testDriveBL.DeleteTestDriveBeforeHandledAsync(testDriveId, deleteTestDriveRequest);
                if (!result)
                {
                    return NotFound(new { error = "Test drive not found or could not be deleted" });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error", message = ex.Message });
            }
        }
    }
}
