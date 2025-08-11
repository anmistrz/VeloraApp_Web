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
        private readonly IEmailNotificationServices _emailNotificationServices;

        public EmailNotificationController(IEmailNotificationServices emailNotificationServices)
        {
            _emailNotificationServices = emailNotificationServices;
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
                var result = await _emailNotificationServices.SendEmail(emailDto.ToEmail, emailDto.Subject, emailDto.Body);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log them)
                return StatusCode(500, "Error sending email");
            }
        }
    }
}