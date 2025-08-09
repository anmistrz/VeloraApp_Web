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
    public class DealerCarUnitController : ControllerBase
    {
        private readonly IDealerCarUnitServices _dealerCarUnitDAL;

        public DealerCarUnitController(IDealerCarUnitServices dealerCarUnitDAL)
        {
            _dealerCarUnitDAL = dealerCarUnitDAL;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDealerCarUnit([FromBody] DealerCarUnitDTO dealerCarUnit)
        {
            if (dealerCarUnit == null)
            {
                return BadRequest("Invalid dealer car unit data.");
            }

            var createdUnit = await _dealerCarUnitDAL.CreateDealerCarUnit(dealerCarUnit);
            return CreatedAtAction(nameof(GetDealerCarUnitById), new { id = createdUnit.DealerCarUnitId }, createdUnit);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDealerCarUnitById(int id)
        {
            var dealerCarUnit = await _dealerCarUnitDAL.GetDealerCarUnitById(id);
            if (dealerCarUnit == null)
            {
                return NotFound();
            }
            return Ok(dealerCarUnit);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDealerCarUnit(int id, [FromBody] DealerCarUnitDTO dealerCarUnit)
        {
            if (id != dealerCarUnit.DealerCarUnitId)
            {
                return BadRequest("ID mismatch.");
            }

            var updatedUnit = await _dealerCarUnitDAL.UpdateDealerCarUnit(dealerCarUnit);
            if (updatedUnit == null)
            {
                return NotFound();
            }
            return Ok(updatedUnit);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDealerCarUnit(int id)
        {
            var result = await _dealerCarUnitDAL.DeleteDealerCarUnit(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("DeleteWithConsultHistory/{id}")]
        public async Task<IActionResult> DeleteDealerCarUnitWithConsultHistory(int id)
        {
            var result = await _dealerCarUnitDAL.DeleteDealerCarUnitWithConsultHistory(id);
            if (result == 0)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}