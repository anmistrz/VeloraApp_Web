using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerApi.Application.DTO;
using DealerApi.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DealerApi.API2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SimulationCreditController : ControllerBase
    {
        private readonly ICarSimulationBL _carSimulationBL;
        public SimulationCreditController(ICarSimulationBL carSimulationBL)
        {
            _carSimulationBL = carSimulationBL;
        }

        [HttpGet("GetCarSimulationCredits")]
        public async Task<IActionResult> GetCarSimulationCredits(int dealerId, int carId, double downPayment, int termMonths, float annualInterestRate)
        {
            try
            {
                var result = await _carSimulationBL.GetCarSimulationCreditsAsync(dealerId, carId, downPayment, termMonths, annualInterestRate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}