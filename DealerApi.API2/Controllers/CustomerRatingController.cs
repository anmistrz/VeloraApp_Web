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
    public class CustomerRatingController : ControllerBase
    {
        private readonly ICustomerRatingBL _customerRatingBL;

        public CustomerRatingController(ICustomerRatingBL customerRatingBL)
        {
            _customerRatingBL = customerRatingBL;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCustomerRating([FromBody] CustomerRatingDTO customerRating)
        {
            try
            {
                var result = await _customerRatingBL.CreateCustomerRatingAsync(customerRating);
                return CreatedAtAction(nameof(CreateCustomerRating), result);
            }
            catch (Exception ex)
            {
                // Handle exception
                return StatusCode(500, "An error occurred while creating the customer rating.");
            }
        }
    }
}
