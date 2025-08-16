using DealerApi.Application;
using DealerApi.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DealerApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealerController : ControllerBase
    {
        private readonly IDealerBL _dealerBL;

        public DealerController(IDealerBL dealerBL)
        {
            _dealerBL = dealerBL;
        }

        [HttpGet]
        [Route("GetDealerOptions")]
        public async Task<IActionResult> GetCarOptions()
        {
            try
            {
                var carOptions = await _dealerBL.GetDealerOptions();
                return Ok(carOptions);
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
