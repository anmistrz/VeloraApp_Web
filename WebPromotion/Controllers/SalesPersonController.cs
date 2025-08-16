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
        private readonly IAccountBusiness _accountBusiness;

        public SalesPersonController(ILogger<SalesPersonController> logger, INotificationBusiness notificationServices, IAccountBusiness accountBusiness)
        {
            _logger = logger;
            _notificationServices = notificationServices;
            _accountBusiness = accountBusiness;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            if (TempData["SuccessModal"] != null)
            {
                var successModalJson = TempData["SuccessModal"]?.ToString();
                if (!string.IsNullOrWhiteSpace(successModalJson))
                {
                    try
                    {
                        ViewBag.SuccessModal = JsonSerializer.Deserialize<ModalViewModels>(successModalJson);
                    }
                    catch (JsonException ex)
                    {
                        // Optional: log error
                        ViewBag.SuccessModal = null;
                    }
                }
            }

            if (TempData["ErrorModal"] != null)
            {
                var errorModalJson = TempData["ErrorModal"]?.ToString();
                if (!string.IsNullOrWhiteSpace(errorModalJson))
                {
                    try
                    {
                        ViewBag.ErrorModal = JsonSerializer.Deserialize<ModalViewModels>(errorModalJson);
                    }
                    catch (JsonException ex)
                    {
                        // Optional: log error
                        ViewBag.ErrorModal = null;
                    }
                }
            }

            return View();
        }

        [HttpGet]
        [Route("DetailNotifications/{id}/{NotificationType}")]
        public async Task<IActionResult> DetailNotificationsAsync(int id, string NotificationType)
        {
            if (NotificationType == "ConsultationRequestHandled")
            {
                var notifications = await _notificationServices.GetNotificationById(id, "ConsultationRequest");
                return View(notifications);
            }
            else if (NotificationType == "TestDriveRequest")
            {
                var testDriveRequest = await _notificationServices.GetNotificationById(id, "TestDriveRequest");
                return View(testDriveRequest);
            }

            var notification = await _notificationServices.GetNotificationById(id, NotificationType);
            return View(notification);
        }


        [HttpGet]
        [Route("ListNotifications")]
        public async Task<IActionResult> ListNotifications(string search)
        {
            Console.WriteLine("SuccessModal", TempData["SuccessModal"]?.ToString());
            var getDealerId = ViewBag.DealerId != null ? Convert.ToInt32(ViewBag.DealerId) : 0;
            var getSearch = !string.IsNullOrEmpty(search) ? search : string.Empty;

            if (TempData["SuccessModal"] != null)
            {
                var successModalJson = TempData["SuccessModal"]?.ToString();
                if (!string.IsNullOrWhiteSpace(successModalJson))
                {
                    try
                    {
                        ViewBag.SuccessModal = JsonSerializer.Deserialize<ModalViewModels>(successModalJson);
                    }
                    catch (JsonException ex)
                    {
                        // Optional: log error
                        ViewBag.SuccessModal = null;
                    }
                }
            }

            if(TempData["ErrorModal"] != null)
            {
                var errorModalJson = TempData["ErrorModal"]?.ToString();
                if (!string.IsNullOrWhiteSpace(errorModalJson))
                {
                    try
                    {
                        ViewBag.ErrorModal = JsonSerializer.Deserialize<ModalViewModels>(errorModalJson);
                    }
                    catch (JsonException ex)
                    {
                        // Optional: log error
                        ViewBag.ErrorModal = null;
                    }
                }
            }

            if (getSearch == string.Empty)
            {
                var ListNotifications = await _notificationServices.GetLimitNotification(200, getDealerId);
                return View(ListNotifications);
            }
            else
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
        public async Task<IActionResult> HandleNotification(NotificationDTO notification)
        {
            Console.WriteLine("Handling notification...");
            Console.WriteLine($"Handling notification with Controlleer: {JsonSerializer.Serialize(notification)}");
            if (notification.NotificationId <= 0 || notification == null)
            {
                TempData["ErrorModal"] = JsonSerializer.Serialize(new ModalViewModels
                {
                    Title = "Error",
                    Message = "Invalid notification ID.",
                    ButtonText = "OK",
                    IsVisible = true,
                    Type = "error"
                });
                return RedirectToAction("ListNotifications");
            }

            var salesPersonId = ViewBag.SalesPersonId != null ? Convert.ToInt32(ViewBag.SalesPersonId) : 0;
            Console.WriteLine($"SalesPersonId: {salesPersonId}");

            var result = await _notificationServices.UpdateReadAndNotificationTypeBusiness(notification, salesPersonId);
            Console.WriteLine($"Notification processed: {result == null}");
            if (result == null)
            {
                TempData["ErrorModal"] = JsonSerializer.Serialize(new ModalViewModels
                {
                    Title = "Error",
                    Message = "Failed to process notification.",
                    ButtonText = "OK",
                    IsVisible = true,
                    Type = "error"
                });
                return RedirectToAction("ListNotifications");
            }
            else
            {
                TempData["SuccessModal"] = JsonSerializer.Serialize(new ModalViewModels
                {
                    Title = "Success",
                    Message = "Notification processed successfully.",
                    ButtonText = "OK",
                    IsVisible = true,
                    Type = "success"
                });

                return RedirectToAction("ListNotifications");
            }
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _accountBusiness.LogoutBusiness();
            return RedirectToAction("Login", "Account");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

    }
}