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
    public class SalesPersonController : ControllerBase
    {
        private readonly ISalesPersonBL _salesPersonBL;

        public SalesPersonController(ISalesPersonBL salesPersonBL)
        {
            _salesPersonBL = salesPersonBL;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSalesPersons()
        {
            var salesPersons = await _salesPersonBL.GetAllSalesPersonsAsync();
            return Ok(salesPersons);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSalesPersonById(int id)
        {
            var salesPerson = await _salesPersonBL.GetSalesPersonByIdAsync(id);
            if (salesPerson == null)
            {
                return NotFound();
            }
            return Ok(salesPerson);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSalesPerson([FromBody] SalesPersonDTO salesPerson)
        {
            if (salesPerson == null)
            {
                return BadRequest();
            }

            var createdSalesPerson = await _salesPersonBL.CreateSalesPersonAsync(salesPerson);
            return CreatedAtAction(nameof(GetSalesPersonById), new { id = createdSalesPerson.SalesPersonId }, createdSalesPerson);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSalesPerson(int id, [FromBody] SalesPersonDTO salesPerson)
        {
            if (salesPerson == null || salesPerson.SalesPersonId != id)
            {
                return BadRequest();
            }

            var updatedSalesPerson = await _salesPersonBL.UpdateSalesPersonAsync(salesPerson);
            if (updatedSalesPerson == null)
            {
                return NotFound();
            }
            return Ok(updatedSalesPerson);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalesPerson(int id)
        {
            var result = await _salesPersonBL.DeleteSalesPersonAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("by-dealer/{dealerId}")]
        public async Task<IActionResult> GetSalesPersonsByDealerId(int dealerId)
        {
            try
            {
                var salesPersons = await _salesPersonBL.GetSalesPersonsByDealerIdAsync(dealerId);
                return Ok(salesPersons);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("by-email/{email}")]
        public async Task<IActionResult> GetSalesPersonByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email cannot be null or empty");
            }

            try
            {
                var salesPersons = await _salesPersonBL.GetSalesPersonByEmailAsync(email);
                if (salesPersons == null || !salesPersons.Any())
                {
                    return NotFound("No sales person found with the provided email");
                }
                return Ok(salesPersons);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }
    }
}