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
        private readonly IDashboardSalesPersonBusiness _dashboardSalesPersonBusiness;

        public SalesPersonController(ILogger<SalesPersonController> logger, INotificationBusiness notificationServices, IAccountBusiness accountBusiness, IDashboardSalesPersonBusiness dashboardSalesPersonBusiness)
        {
            _logger = logger;
            _notificationServices = notificationServices;
            _accountBusiness = accountBusiness;
            _dashboardSalesPersonBusiness = dashboardSalesPersonBusiness;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
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
            // Safely parse SalesPersonId from ViewBag (it may be int, string or null)
            int salesPersonId = 0;
            var spIdObj = ViewBag.SalesPersonId;
            if (spIdObj is int spInt)
            {
                salesPersonId = spInt;
            }
            else if (spIdObj is string spStr && int.TryParse(spStr, out var spParsed))
            {
                salesPersonId = spParsed;
            }
            var dataTotalConsultation = await _dashboardSalesPersonBusiness.GetTotalConsultationHandledSalesPersonAsync(salesPersonId);
            ViewBag.DataTotalConsultation = dataTotalConsultation;

            var dataTotalTestDrive = await _dashboardSalesPersonBusiness.GetTotalTestDriveHandledSalesPersonAsync(salesPersonId);
            ViewBag.DataTotalTestDrive = dataTotalTestDrive;

            var dataTotalTargetConsultationThisMonth = await _dashboardSalesPersonBusiness.GetTotalTargetConsultationHandledSalesPersonAsync(salesPersonId);
            ViewBag.DataTotalTargetConsultationThisMonth = dataTotalTargetConsultationThisMonth;
            var dataTotalTargetTestDriveThisMonth = await _dashboardSalesPersonBusiness.GetTotalTargetTestDriveHandledSalesPersonAsync(salesPersonId);
            ViewBag.DataTotalTargetTestDriveThisMonth = dataTotalTargetTestDriveThisMonth;

            var dataTotalActivityConsultationByMonth = await _dashboardSalesPersonBusiness.GetTotalSalesActivityConsultationByMonthAsync(salesPersonId);
            Console.WriteLine($"DataTotalActivityConsultationByMonth: {JsonSerializer.Serialize(dataTotalActivityConsultationByMonth)}");
            // ensure we have an enumerable to avoid null reference or cast errors
            var activityConsultationList = (dataTotalActivityConsultationByMonth as IEnumerable<dynamic>) ?? Enumerable.Empty<dynamic>();
            ViewBag.DataTotalActivityConsultationByMonth = activityConsultationList.ToList();
            Console.WriteLine($"Unique Months for Activity Consultation: {string.Join(", ", activityConsultationList.Select(x => x.month).Distinct())}");

            var dataTotalActivityTestDriveByMonth = await _dashboardSalesPersonBusiness.GetTotalSalesActivityTestDriveByMonthAsync(salesPersonId);
            var activityTestDriveList = (dataTotalActivityTestDriveByMonth as IEnumerable<dynamic>) ?? Enumerable.Empty<dynamic>();
            ViewBag.DataTotalActivityTestDriveByMonth = activityTestDriveList.ToList();
            Console.WriteLine($"Unique Months for Activity Test Drive: {string.Join(", ", activityTestDriveList.Select(x => x.month).Distinct())}");

            var listDataActivitySalesPerformance = await _dashboardSalesPersonBusiness.GetDetailActivitySalesPerformanceByDayInThisMonthAsync(salesPersonId);
            ViewBag.ListDataActivitySalesPerformance = (listDataActivitySalesPerformance as IEnumerable<dynamic>) ?? Enumerable.Empty<dynamic>();

            var dataSalesPerformanceReview = await _dashboardSalesPersonBusiness.GetSalesPerformanceReviewAsync(salesPersonId);
            ViewBag.DataSalesPerformanceReview = dataSalesPerformanceReview;

            var dataSalesAverageRating = await _dashboardSalesPersonBusiness.GetSalesRatingAsync(salesPersonId);
            ViewBag.DataSalesAverageRating = dataSalesAverageRating;

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

        // [HttpGet]
        // [Route("GetSalesPerformanceReview")]
        // public async Task<IActionResult> GetSalesPerformanceReview(int salesPersonId)
        // {
        //     var result = await _dashboardSalesPersonBusiness.GetSalesPerformanceReviewAsync(salesPersonId);
        //     return Json(result);
        // }

        // [HttpGet]
        // [Route("GetSalesRating")]
        // public async Task<IActionResult> GetSalesRating(int salesPersonId)
        // {
        //     var result = await _dashboardSalesPersonBusiness.GetSalesRatingAsync(salesPersonId);
        //     return Json(result);
        // }

        // [HttpGet]
        // [Route("GetTotalConsultationHandledSalesPerson")]
        // public async Task<IActionResult> GetTotalConsultationHandledSalesPerson(int salesPersonId)
        // {
        //     var result = await _dashboardSalesPersonBusiness.GetTotalConsultationHandledSalesPersonAsync(salesPersonId);
        //     return Json(result);
        // }

        // [HttpGet]
        // [Route("GetTotalSalesActivityConsultationByMonth")]
        // public async Task<IActionResult> GetTotalSalesActivityConsultationByMonth(int salesPersonId)
        // {
        //     var result = await _dashboardSalesPersonBusiness.GetTotalSalesActivityConsultationByMonthAsync(salesPersonId);
        //     return Json(result);
        // }

        // [HttpGet]
        // [Route("GetTotalSalesActivityTestDriveByMonth")]
        // public async Task<IActionResult> GetTotalSalesActivityTestDriveByMonth(int salesPersonId)
        // {
        //     var result = await _dashboardSalesPersonBusiness.GetTotalSalesActivityTestDriveByMonthAsync(salesPersonId);
        //     return Json(result);
        // }

        // [HttpGet]
        // [Route("GetTotalTargetConsultationHandledSalesPerson")]
        // public async Task<IActionResult> GetTotalTargetConsultationHandledSalesPerson(int salesPersonId)
        // {
        //     var result = await _dashboardSalesPersonBusiness.GetTotalTargetConsultationHandledSalesPersonAsync(salesPersonId);
        //     return Json(result);
        // }

        // [HttpGet]
        // [Route("GetTotalTargetTestDriveHandledSalesPerson")]
        // public async Task<IActionResult> GetTotalTargetTestDriveHandledSalesPerson(int salesPersonId)
        // {
        //     var result = await _dashboardSalesPersonBusiness.GetTotalTargetTestDriveHandledSalesPersonAsync(salesPersonId);
        //     return Json(result);
        // }

        // [HttpGet]
        // [Route("GetTotalTestDriveHandledSalesPerson")]
        // public async Task<IActionResult> GetTotalTestDriveHandledSalesPerson(int salesPersonId)
        // {
        //     var result = await _dashboardSalesPersonBusiness.GetTotalTestDriveHandledSalesPersonAsync(salesPersonId);
        //     return Json(result);
        // }


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