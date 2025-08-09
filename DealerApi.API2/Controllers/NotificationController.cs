using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerApi.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DealerApi.API2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationServices _notificationServices;

        public NotificationController(INotificationServices notificationServices)
        {
            _notificationServices = notificationServices;
        }

        [HttpGet]
        [Authorize(Roles = "salesPerson")]
        [Route("GetLimitedNotifications/{limit}")]
        public IActionResult GetLimitedNotifications(int limit)
        {
            try
            {
                var notifications = _notificationServices.GetLimitNotification(limit);
                Console.WriteLine($"Fetched {notifications.Count()} notifications with limitController {limit}");
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }
    }
}