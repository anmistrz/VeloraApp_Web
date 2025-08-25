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
    public class ConsultHistoryController : ControllerBase
    {
        private readonly IConsultHistoryBL _consultHistoryBL;

        public ConsultHistoryController(IConsultHistoryBL consultHistoryBL)
        {
            _consultHistoryBL = consultHistoryBL;
        }

        [HttpPost("create-guest")]
        public async Task<IActionResult> CreateConsultHistoryGuest([FromBody] ConsultHistoryGuestDTO consultHistoryGuestDto)
        {
            try
            {
                if (consultHistoryGuestDto == null)
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
                    FirstName = consultHistoryGuestDto.FirstName,
                    LastName = consultHistoryGuestDto.LastName,
                    Email = consultHistoryGuestDto.Email,
                    PhoneNumber = consultHistoryGuestDto.PhoneNumber,
                    IsGuest = true // Assuming this is a guest
                };

                var dtConsultHistory = new ConsultHistory
                {
                    DealerCarUnitId = consultHistoryGuestDto.DealerCarUnitId,
                    ConsultDate = consultHistoryGuestDto.ConsultDate,
                    Note = consultHistoryGuestDto.Note,
                    SalesPersonId = consultHistoryGuestDto.SalesPersonId,
                    Budget = consultHistoryGuestDto.Budget
                };

                var dtDealerCar = new DealerCar
                {
                    DealerId = consultHistoryGuestDto.DealerId,
                    CarId = consultHistoryGuestDto.DealerCarUnitId // Assuming DealerCarUnitId maps to CarId
                };

                var result = await _consultHistoryBL.CreateAsyncConsultHistoryGuest(
                    dtCustomer,
                    dtConsultHistory,
                    dtDealerCar
                );
                return CreatedAtAction(nameof(CreateConsultHistoryGuest), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpGet("salesperson/{salesPersonId}")]
        [Authorize(Roles = "salesPerson")]
        public async Task<IActionResult> GetConsultHistoryBySalesPersonId(int salesPersonId)
        {
            try
            {
                var result = await _consultHistoryBL.GetConsultHistoryBySalesPersonIdAsync(salesPersonId);
                if (result == null || !result.Any())
                {
                    return NotFound(new { error = "No consult history found for the specified sales person" });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpPost("delete-after-handled/{id}")]
        [Authorize(Roles = "salesPerson")]
        public async Task<IActionResult> DeleteConsultHistoryAfterHandled(int id, [FromBody] DeleteConsultRequestDTO deleteRequest)
        {
            try
            {
                if (deleteRequest == null)
                {
                    return BadRequest(new { error = "Request body cannot be null or empty" });
                }

                var result = await _consultHistoryBL.DeleteConsultHistoryAfterHandledAsync(id, deleteRequest);
                if (!result)
                {
                    return NotFound(new { error = "Consult history not found or could not be deleted" });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpPost("delete-before-handled/{id}")]
        [Authorize(Roles = "salesPerson")]
        public async Task<IActionResult> DeleteConsultHistoryBeforeHandled(int id, [FromBody] DeleteConsultRequestDTO deleteRequest)
        {
            try
            {
                if (deleteRequest == null)
                {
                    return BadRequest(new { error = "Request body cannot be null or empty" });
                }

                var result = await _consultHistoryBL.DeleteConsultHistoryBeforeHandledAsync(id, deleteRequest);
                if (!result)
                {
                    return NotFound(new { error = "Consult history not found or could not be deleted" });
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
