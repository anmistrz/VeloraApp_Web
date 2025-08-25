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
using WebPromotion.ViewModels.Modal;

namespace WebPromotion.Controllers.SalesPerson
{
    [Route("SalesPerson/[controller]")]
    public class TestDriveRequestController : Controller
    {
        private readonly ILogger<TestDriveRequestController> _logger;
        private readonly ITestDriveBusiness _testDriveBusiness;
        private readonly ISalesActivityBusiness _salesActivityBusiness;

        public TestDriveRequestController(ILogger<TestDriveRequestController> logger, ITestDriveBusiness testDriveBusiness, ISalesActivityBusiness salesActivityBusiness)
        {
            _logger = logger;
            _testDriveBusiness = testDriveBusiness;
            _salesActivityBusiness = salesActivityBusiness;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var salesPersonIdObj = ViewBag.SalesPersonId;
            var salesPersonId = salesPersonIdObj?.ToString();
            Console.WriteLine($"SalesPersonId: {salesPersonId}");
            if (string.IsNullOrWhiteSpace(salesPersonId))
            {
                _logger.LogError("SalesPersonId is null or empty.");
                return NotFound("SalesPersonId is not provided.");
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
            var dataTableTestDrive = await _testDriveBusiness.GetTestDriveRequestBySalesPerson(salesPersonId);
            return View(dataTableTestDrive);
        }


        [HttpPost("delete-after-handled")]
        public async Task<IActionResult> DeleteAfterHandled([FromForm] int id, [FromForm] string Reason)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogError("Invalid ConsultHistoryId: {Id}", id);
                    return BadRequest("Invalid ConsultHistoryId.");
                }

                    var salesPersonId = Convert.ToInt32(ViewBag.SalesPersonId);
                    var DealerId = Convert.ToInt32(ViewBag.DealerId);
                    var dataBody = new DeleteTestDriveRequestClientDTO
                    {
                        TestDriveId = id,
                        SalesPersonId = salesPersonId,
                        DealerId = DealerId,
                        Reason = Reason
                    };
                    _logger.LogInformation($"Deleting test drive request with ID: {id}, SalesPersonId: {salesPersonId}, DealerId: {DealerId}, Reason: {Reason}");
                    var result = await _testDriveBusiness.DeleteTestDriveAfterHandled(id, dataBody);
                    Console.WriteLine($"DeleteAfterHandled result: {result}");
                    if (!result)
                    {
                        TempData["ErrorModal"] = JsonSerializer.Serialize(new ModalViewModels
                        {
                            Title = "Error",
                            Message = "Failed to delete test drive request.",
                            ButtonText = "OK",
                            IsVisible = true,
                            Type = "error"
                        });
                        return RedirectToAction("Index");
                    }

                    TempData["SuccessModal"] = JsonSerializer.Serialize(new ModalViewModels
                    {
                        Title = "Success",
                        Message = "Test drive request deleted successfully.",
                        ButtonText = "OK",
                        IsVisible = true,
                        Type = "success"
                    });

                    return RedirectToAction("Index");
                }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting consult history");
                throw new Exception("An error occurred while deleting the consult history.", ex);
            }
        }

        [HttpPost("delete-before-handled")]
        public async Task<IActionResult> DeleteBeforeHandled([FromForm] int id, [FromForm] string Reason, [FromForm] string location)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogError("Invalid ConsultHistoryId: {Id}", id);
                    return BadRequest("Invalid ConsultHistoryId.");
                }

                var salesPersonId = Convert.ToInt32(ViewBag.SalesPersonId);
                var DealerId = Convert.ToInt32(ViewBag.DealerId);
                var dataBody = new DeleteTestDriveRequestClientDTO
                {
                    TestDriveId = id,
                    SalesPersonId = salesPersonId,
                    DealerId = DealerId,
                    Reason = Reason
                };
                Console.WriteLine($"Location: {location}");
                Console.WriteLine($"Deleting test drive request with ID: {id}, SalesPersonId: {salesPersonId}, DealerId: {DealerId}, Reason: {Reason}");
                _logger.LogInformation($"Deleting test drive with ID: {id}, SalesPersonId: {salesPersonId}, DealerId: {DealerId}");
                var result = await _testDriveBusiness.DeleteTestDriveBeforeHandled(id, dataBody);
                if (!result)
                {
                    TempData["ErrorModal"] = JsonSerializer.Serialize(new ModalViewModels
                    {
                        Title = "Error",
                        Message = "Failed to delete consult history.",
                        ButtonText = "OK",
                        IsVisible = true,
                        Type = "error"
                    });
                    return RedirectToAction("Index");
                }

                TempData["SuccessModal"] = JsonSerializer.Serialize(new ModalViewModels
                {
                    Title = "Success",
                    Message = "Consult history deleted successfully.",
                    ButtonText = "OK",
                    IsVisible = true,
                    Type = "success"
                });
                
                if(location == "notification")
                {
                    return RedirectToAction("ListNotifications", "SalesPerson");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting consult history");
                throw new Exception("An error occurred while deleting the consult history.", ex);
            }
        }

        [HttpGet]
        [Route("Detail/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0)
            {
                _logger.LogError("Invalid ConsultHistoryId: {Id}", id);
                return BadRequest("Invalid ConsultHistoryId.");
            }

            try
            {
                var consultHistory = await _salesActivityBusiness.getSalesActivityTestDriveById(id);
                if (consultHistory == null)
                {
                    _logger.LogError("Test Drive not found for ID: {Id}", id);
                    return NotFound("Test Drive not found.");
                }
                return View(consultHistory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving test drive details for ID: {Id}", id);
                return StatusCode(500, ex.Message);
            }
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}