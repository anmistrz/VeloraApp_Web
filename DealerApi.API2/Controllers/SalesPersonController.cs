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
        private readonly ISalesPersonServices _salesPersonServices;

        public SalesPersonController(ISalesPersonServices salesPersonServices)
        {
            _salesPersonServices = salesPersonServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSalesPersons()
        {
            var salesPersons = await _salesPersonServices.GetAllSalesPersonsAsync();
            return Ok(salesPersons);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSalesPersonById(int id)
        {
            var salesPerson = await _salesPersonServices.GetSalesPersonByIdAsync(id);
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

            var createdSalesPerson = await _salesPersonServices.CreateSalesPersonAsync(salesPerson);
            return CreatedAtAction(nameof(GetSalesPersonById), new { id = createdSalesPerson.SalesPersonId }, createdSalesPerson);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSalesPerson(int id, [FromBody] SalesPersonDTO salesPerson)
        {
            if (salesPerson == null || salesPerson.SalesPersonId != id)
            {
                return BadRequest();
            }

            var updatedSalesPerson = await _salesPersonServices.UpdateSalesPersonAsync(salesPerson);
            if (updatedSalesPerson == null)
            {
                return NotFound();
            }
            return Ok(updatedSalesPerson);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalesPerson(int id)
        {
            var result = await _salesPersonServices.DeleteSalesPersonAsync(id);
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
                var salesPersons = await _salesPersonServices.GetSalesPersonsByDealerIdAsync(dealerId);
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