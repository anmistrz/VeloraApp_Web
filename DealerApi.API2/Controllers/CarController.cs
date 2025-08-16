using Microsoft.AspNetCore.Mvc;
using DealerApi.Application.Interface;

namespace DealerApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {

        private readonly ICarBL _carBL;
        public CarController(ICarBL carBL)
        {
            _carBL = carBL;
        }



        [HttpGet]
        [Route("GetCarOptions")]
        public async Task<IActionResult> GetCarOptions()
        {
            try
            {
                var carOptions = await _carBL.GetCarOptions();
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

