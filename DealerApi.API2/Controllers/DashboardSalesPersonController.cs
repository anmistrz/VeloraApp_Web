using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DealerApi.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DealerApi.API2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardSalesPersonController : ControllerBase
    {
        private readonly IDashboardSalesPersonBL _dashboardSalesPersonBL;

        public DashboardSalesPersonController(IDashboardSalesPersonBL dashboardSalesPersonBL)
        {
            _dashboardSalesPersonBL = dashboardSalesPersonBL;
        }

        [HttpGet("total-consultation-handled/{salesPersonId}")]
        public async Task<IActionResult> GetTotalConsultationHandledSalesPersonAsync(int salesPersonId)
        {
            try
            {
                var result = await _dashboardSalesPersonBL.GetTotalConsultationHandledSalesPersonAsync(salesPersonId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                return StatusCode(500, "Error fetching total consultations handled");
            }
        }


        [HttpGet("total-sales-activity-consultation-by-month/{salesPersonId}")]
        public async Task<IActionResult> GetTotalSalesActivityConsultationByMonthAsync(int salesPersonId)
        {
            try
            {
                var result = await _dashboardSalesPersonBL.GetTotalSalesActivityConsultationByMonthAsync(salesPersonId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                return StatusCode(500, "Error fetching sales activity consultation by month");
            }
        }

        [HttpGet("total-sales-activity-test-drive-by-month/{salesPersonId}")]
        public async Task<IActionResult> GetTotalSalesActivityTestDriveByMonthAsync(int salesPersonId)
        {
            try
            {
                var result = await _dashboardSalesPersonBL.GetTotalSalesActivityTestDriveByMonthAsync(salesPersonId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                return StatusCode(500, "Error fetching sales activity test drive by month");
            }
        }

        [HttpGet("total-target-consultation-handled/{salesPersonId}")]
        public async Task<IActionResult> GetTotalTargetConsultationHandledSalesPersonAsync(int salesPersonId)
        {
            try
            {
                var result = await _dashboardSalesPersonBL.GetTotalTargetConsultationHandledSalesPersonAsync(salesPersonId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                return StatusCode(500, "Error fetching total target consultations handled");
            }
        }

        [HttpGet("total-target-test-drive-handled/{salesPersonId}")]
        public async Task<IActionResult> GetTotalTargetTestDriveHandledSalesPersonAsync(int salesPersonId)
        {
            try
            {
                var result = await _dashboardSalesPersonBL.GetTotalTargetTestDriveHandledSalesPersonAsync(salesPersonId);
                Console.WriteLine($"Result from APIIII: {JsonSerializer.Serialize(result)}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                return StatusCode(500, "Error fetching total target test drives handled");
            }
        }

        [HttpGet("total-test-drive-handled/{salesPersonId}")]
        public async Task<IActionResult> GetTotalTestDriveHandledSalesPersonAsync(int salesPersonId)
        {
            try
            {
                var result = await _dashboardSalesPersonBL.GetTotalTestDriveHandledSalesPersonAsync(salesPersonId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                return StatusCode(500, "Error fetching total test drives handled");
            }
        }

        [HttpGet("sales-performance-review/{salesPersonId}")]
        public async Task<IActionResult> GetSalesPerformanceReviewAsync(int salesPersonId)
        {
            try
            {
                var result = await _dashboardSalesPersonBL.GetSalesPerformanceReviewAsync(salesPersonId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                return StatusCode(500, "Error fetching sales performance review");
            }
        }

        [HttpGet("sales-rating/{salesPersonId}")]
        public async Task<IActionResult> GetSalesRatingAsync(int salesPersonId)
        {
            try
            {
                var result = await _dashboardSalesPersonBL.GetSalesRatingAsync(salesPersonId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                return StatusCode(500, "Error fetching sales rating");
            }
        }

        [HttpGet("detail-activity-sales-performance-by-day-in-this-month/{salesPersonId}")]
        public async Task<IActionResult> GetDetailActivitySalesPerformanceByDayInThisMonthAsync(int salesPersonId)
        {
            try
            {
                var result = await _dashboardSalesPersonBL.GetDetailActivitySalesPerformanceByDayInThisMonthAsync(salesPersonId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                return StatusCode(500, "Error fetching detail activity sales performance by day in this month");
            }
        }

    }
}