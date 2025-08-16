using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebPromotion.Business.Interface;
using WebPromotion.ViewModels.Modal;

namespace WebPromotion.Controllers.SalesPerson
{
    [Route("SalesPerson/[controller]")]
    public class SalesActivityController : Controller
    {
        private readonly ILogger<SalesActivityController> _logger;
        private readonly ISalesActivityBusiness _salesActivityBusiness;

        public SalesActivityController(ILogger<SalesActivityController> logger, ISalesActivityBusiness salesActivityBusiness)
        {
            _logger = logger;
            _salesActivityBusiness = salesActivityBusiness;
        }

        [HttpGet]
        [Route("ConsultationHandler")]
        public async Task<IActionResult> ConsultationHandler()
        {
            var salesPersonId = Convert.ToInt32(ViewBag.SalesPersonId);
                        Console.WriteLine($"SalesPersonId SalesActivity: {salesPersonId}");
            var salesActivity = await _salesActivityBusiness.getSalesActivityBySalesPerson(salesPersonId, "ConsultationRequestHandled");
            if (salesActivity == null)
            {
                _logger.LogError("No consultation activities found for the sales person.");
                return NotFound("No consultation activities found.");
            }
 
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
            return View(salesActivity);
        }

        [HttpGet]
        [Route("TestDriveHandler")]
        public async Task<IActionResult> TestDriveHandler()
        {
            var salesPersonId = Convert.ToInt32(ViewBag.SalesPersonId);
            var salesActivity = await _salesActivityBusiness.getSalesActivityBySalesPerson(salesPersonId, "TestDriveRequestHandled");
            if (salesActivity == null)
            {
                return NotFound("No test drive activities found.");
            }
            
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
            return View(salesActivity);
        }

        [HttpPost]
        [Route("UpdateConsultationResult/{salesActivityId}")]
        public async Task<IActionResult> UpdateConsultationResult(int salesActivityId, [FromForm] string Reason)
        {
            var isSuccess = await _salesActivityBusiness.UpdateResultConsultationAsync(salesActivityId, Reason);

            if (!isSuccess)
            {
                TempData["ErrorModal"] = JsonSerializer.Serialize(new ModalViewModels
                {
                    Title = "Error",
                    Message = "Failed to complete request for consultation activities.",
                    ButtonText = "OK",
                    IsVisible = true,
                    Type = "error"
                });

                _logger.LogError("Error updating consultation result.");
                return BadRequest("Error updating consultation result.");
            }
            TempData["SuccessModal"] = JsonSerializer.Serialize(new ModalViewModels
            {
                Title = "Success",
                Message = "Consultation activities retrieved successfully.",
                ButtonText = "OK",
                IsVisible = true,
                Type = "success"
            });
            return RedirectToAction("ConsultationHandler");
        }

        [HttpPost]
        [Route("UpdateTestDriveResult/{salesActivityId}")]
        public async Task<IActionResult> UpdateTestDriveResult(int salesActivityId, [FromForm] string Reason)
        {
            Console.WriteLine($"Updating server test drive result for SalesActivityId: {salesActivityId} with reason: {Reason}");
            var isSuccess = await _salesActivityBusiness.UpdateResultTestDriveAsync(salesActivityId, Reason);
            if (!isSuccess)
            {
                TempData["ErrorModal"] = JsonSerializer.Serialize(new ModalViewModels
                {
                    Title = "Error",
                    Message = "Failed to complete request for test drive activities.",
                    ButtonText = "OK",
                    IsVisible = true,
                    Type = "error"
                });
                _logger.LogError("Error updating test drive result.");
                return RedirectToAction("TestDriveHandler");
            }
            TempData["SuccessModal"] = JsonSerializer.Serialize(new ModalViewModels
            {
                Title = "Success",
                Message = "Test drive activities retrieved successfully.",
                ButtonText = "OK",
                IsVisible = true,
                Type = "success"
            });
            return RedirectToAction("TestDriveHandler");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}