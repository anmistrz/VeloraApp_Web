using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebPromotion.Business.Interface;
using WebPromotion.Services.DTO;
using WebPromotion.Services.Interface;
using WebPromotion.ViewModels.Modal;

namespace WebPromotion.Controllers
{
    [Route("[controller]")]
    public class SalesPersonController : Controller
    {
        private readonly ILogger<SalesPersonController> _logger;
        private readonly INotificationBusiness _notificationServices;

        public SalesPersonController(ILogger<SalesPersonController> logger, INotificationBusiness notificationServices)
        {
            _logger = logger;
            _notificationServices = notificationServices;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            // var limitNotification = _notificationServices.GetLimitNotification(10, 2); // Example customId

            // if (limitNotification != null)
            // {
            //     ViewBag.LimitNotification = limitNotification;
            // }

            if (TempData["SuccessModal"] != null)
            {
                var successModalJson = TempData["SuccessModal"]?.ToString();
                if (!string.IsNullOrEmpty(successModalJson))
                {
                    ViewBag.SuccessModal = JsonSerializer.Deserialize<ModalViewModels>(successModalJson);
                }
            }

            if (TempData["ErrorModal"] != null)
            {
                var errorModalJson = TempData["ErrorModal"]?.ToString();
                if (!string.IsNullOrEmpty(errorModalJson))
                {
                    ViewBag.ErrorModal = JsonSerializer.Deserialize<ModalViewModels>(errorModalJson);
                }
            }
            return View();
        }

        [HttpGet]
        [Route("DetailNotifications/{id}")]
        public async Task<IActionResult> DetailNotificationsAsync(int id)
        {
            var notifications = await _notificationServices.GetNotificationById(id);
            return View(notifications);
        }


        [HttpGet]
        [Route("ListNotifications")]
        public async Task<IActionResult> ListNotifications(string search)
        {
            var getDealerId = ViewBag.DealerId != null ? Convert.ToInt32(ViewBag.DealerId) : 0;
            var getSearch = !string.IsNullOrEmpty(search) ? search : string.Empty;
            if (getSearch == string.Empty)
            {
                var ListNotifications = await _notificationServices.GetLimitNotification(200, getDealerId);
                return View(ListNotifications);
            } else
            {
                var ListNotifications = await _notificationServices.GetNotificationBySearchBussiness(getSearch, getDealerId);
                return View(ListNotifications);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateNotificationAsync(int id, [FromBody] NotificationDTO notification)
        {
            if (id != notification.NotificationId)
            {
                return BadRequest("Notification ID mismatch");
            };

            var updatedNotification = await _notificationServices.UpdateReadNotificationStatus(id, notification.IsRead);
            if (updatedNotification == null)
            {
                return View();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("HandleNotification")]
        public IActionResult HandleNotification(NotificationDTO notification)
        {
            Console.WriteLine($"Handling notification with ID: {notification.NotificationId}");
            if (notification.NotificationId <= 0 || notification == null)
            {
                TempData["ErrorModal"] = JsonSerializer.Serialize(new ModalViewModels
                {
                    Title = "Error",
                    Message = "Invalid notification ID.",
                    ButtonText = "OK"
                });
                return RedirectToAction("ListNotifications");
            }

            var result = _notificationServices.UpdateReadAndNotificationTypeBusiness(notification);
            if (result == null)
            {
                TempData["ErrorModal"] = JsonSerializer.Serialize(new ModalViewModels
                {
                    Title = "Error",
                    Message = "Failed to process notification.",
                    ButtonText = "OK"
                });
                return RedirectToAction("ListNotifications");
            }
            else
            {
                TempData["SuccessModal"] = JsonSerializer.Serialize(new ModalViewModels
                {
                    Title = "Success",
                    Message = "Notification processed successfully.",
                    ButtonText = "OK"
                });
                return RedirectToAction("ListNotifications");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

    }
}