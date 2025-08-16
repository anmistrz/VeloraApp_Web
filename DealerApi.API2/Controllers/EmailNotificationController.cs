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
    public class EmailNotificationController : ControllerBase
    {
        private readonly IEmailNotificationBL _emailNotificationBL;

        public EmailNotificationController(IEmailNotificationBL emailNotificationBL)
        {
            _emailNotificationBL = emailNotificationBL;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailNotificationDTO emailDto)
        {
            if (emailDto == null)
            {
                return BadRequest("Invalid email data");
            }

            try
            {
                var result = await _emailNotificationBL.SendEmail(emailDto.ToEmail, emailDto.Subject, emailDto.Body);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error sending email: " + ex.Message);
            }
        }
    }
}