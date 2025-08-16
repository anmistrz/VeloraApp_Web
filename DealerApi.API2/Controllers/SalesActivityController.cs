using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerApi.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DealerApi.API2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesActivityController : ControllerBase
    {
        private readonly ISalesActivityLogBL _salesActivityBL;

        public SalesActivityController(ISalesActivityLogBL salesActivityBL)
        {
            _salesActivityBL = salesActivityBL;
        }

        [HttpGet("all-sales-activities/{salesPersonId}/{notificationType}")]
        public async Task<IActionResult> GetSalesActivities(int salesPersonId, string notificationType)
        {
            try
            {
                var salesActivities = await _salesActivityBL.GetAllSalesActivitiesBySalesPersonAsync(salesPersonId, notificationType);
                return Ok(salesActivities);
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("update-consultation-result/{salesActivityId}")]
        [Authorize(Roles = "salesPerson")]
        public async Task<IActionResult> UpdateConsultationResult(int salesActivityId, [FromBody] string result)
        {
            try
            {
                var success = await _salesActivityBL.UpdateResultConsultationAsync(salesActivityId, result);
                if (success)
                {
                    return NoContent();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("update-test-drive-result/{salesActivityId}")]
        [Authorize(Roles = "salesPerson")]
        public async Task<IActionResult> UpdateTestDriveResult(int salesActivityId, [FromBody] string result)
        {
            try
            {
                Console.WriteLine($"Updating server test drive result for SalesActivityId: {salesActivityId} with result: {result}");
                var success = await _salesActivityBL.UpdateResultTestDriveAsync(salesActivityId, result);
                if (success)
                {
                    return NoContent();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("sales-activities-consultation/{consultationId}")]
        public async Task<IActionResult> GetSalesActivityByConsultationId(int consultationId)
        {
            try
            {
                var salesActivity = await _salesActivityBL.GetSalesActivityConsultationByIdAsync(consultationId);
                return Ok(salesActivity);
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("sales-activities-test-drive/{testDriveId}")]
        public async Task<IActionResult> GetSalesActivityByTestDriveId(int testDriveId)
        {
            try
            {
                var salesActivity = await _salesActivityBL.GetSalesActivityTestDriveByIdAsync(testDriveId);
                return Ok(salesActivity);
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, ex.Message);
            }
        }
    }
}