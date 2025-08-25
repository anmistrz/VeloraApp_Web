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

        [HttpGet]
        [Route("ListMostCarTestDrivesTop")]
        public async Task<IActionResult> ListMostCarTestDrivesTop(int top)
        {
            try
            {
                var result = await _carBL.ListMostCarTestDrivesTop(top);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet]
        [Route("GetCarSelectListItems")]
        public async Task<IActionResult> GetCarSelectListItems()
        {
            try
            {
                var carSelectListItems = await _carBL.GetCarSelectListItems();
                return Ok(carSelectListItems);
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}

