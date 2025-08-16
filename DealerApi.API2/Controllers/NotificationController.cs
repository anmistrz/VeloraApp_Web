using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerApi.Application.DTO;
using DealerApi.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DealerApi.API2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationBL _notificationBL;

        public NotificationController(INotificationBL notificationBL)
        {
            _notificationBL = notificationBL;
        }

        [HttpGet]
        [Authorize(Roles = "salesPerson")]
        [Route("GetLimitedNotifications/{limit}/{customId}")]
        public IActionResult GetLimitedNotifications(int limit, int customId)
        {
            try
            {
                var notifications = _notificationBL.GetLimitNotification(limit, customId);
                Console.WriteLine($"Fetched {notifications.Count()} notifications with limitController {limit}");
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Authorize(Roles = "salesPerson")]
        [Route("GetNotificationById/{id}/{notificationType}")]
        public IActionResult GetNotificationById(int id, string notificationType)
        {
            try
            {
                var notification = _notificationBL.GetNotificationById(id, notificationType);
                if (notification != null)
                {
                    return Ok(notification);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet]
        [Authorize(Roles = "salesPerson")]
        [Route("GetAllNotifications")]
        public IActionResult GetAllNotifications()
        {
            try
            {
                var notifications = _notificationBL.GetAllNotifications();
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet]
        [Authorize(Roles = "salesPerson")]
        [Route("GetUnreadNotifications/{customId}")]
        public IActionResult GetUnreadNotifications(int customId)
        {
            try
            {
                var notifications = _notificationBL.GetUnreadNotifications(customId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet]
        [Authorize(Roles = "salesPerson")]
        [Route("GetNotificationBySearch/{searchTerm}/{customId}")]
        public IActionResult GetNotificationBySearch(string searchTerm, int customId)
        {
            try
            {
                var notifications = _notificationBL.GetNotificationsBySearch(searchTerm, customId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }



        [HttpPut]
        [Authorize(Roles = "salesPerson")]
        [Route("UpdateReadNotificationStatus/{id}")]
        public IActionResult UpdateReadNotificationStatus(int id, [FromBody] bool isRead)
        {

            var updatedNotification = _notificationBL.UpdateReadNotificationStatus(id, isRead);
            if (updatedNotification == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut]
        [Authorize(Roles = "salesPerson")]
        [Route("UpdateRead-and-NotificationType/{salesPersonId}")]
        public IActionResult UpdateReadAndNotificationType(int salesPersonId, [FromBody] NotificationDTO notification)
        {
            var updatedNotification = _notificationBL.UpdateReadAndNotificationType(notification, salesPersonId);
            if (updatedNotification == null)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}