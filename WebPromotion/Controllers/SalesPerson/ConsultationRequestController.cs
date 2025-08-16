using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DealerApi.Application.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebPromotion.Business.Interface;
using WebPromotion.Models;
using WebPromotion.Services.DTO;
using WebPromotion.ViewModels.Modal;

namespace WebPromotion.Controllers
{
    [Route("SalesPerson/[controller]")]
    public class ConsultationRequestController : Controller
    {
        private readonly ILogger<ConsultationRequestController> _logger;
        private readonly IConsultationBusiness _consultationBusiness;
        private readonly ISalesActivityBusiness _salesActivityBusiness;

        public ConsultationRequestController(ILogger<ConsultationRequestController> logger, IConsultationBusiness consultationBusiness, ISalesActivityBusiness salesActivityBusiness)
        {
            _logger = logger;
            _consultationBusiness = consultationBusiness;
            _salesActivityBusiness = salesActivityBusiness;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var salesPersonId = ViewBag.SalesPersonId;
            Console.WriteLine($"SalesPersonId: {salesPersonId}");
            if (salesPersonId == null)
            {
                _logger.LogError("SalesPersonId is null.");
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

            var dataTableConsultation = await _consultationBusiness.GetConsultHistoryRequestBySalesPerson(salesPersonId);
            return View(dataTableConsultation);
        }

    [HttpPost("delete-after-handled")]
    public async Task<IActionResult> DeleteAfterHandled([FromForm] int id, [FromForm] string Reason)
    {
        try
        {
            if (id <= 0)
            {
                    _logger.LogError("Invalid ConsultHistoryId: {Id}", id);
                    return BadRequest("Invalid ConsultHistoryIdddddddddddd.");
                }

                var salesPersonId = Convert.ToInt32(ViewBag.SalesPersonId);
                var DealerId = Convert.ToInt32(ViewBag.DealerId);
                var dataBody = new DeleteConsultRequestClientDTO
                {
                    ConsultHistoryId = id,
                    SalesPersonId = salesPersonId,
                    DealerId = DealerId,
                    Reason = Reason
                };
                _logger.LogInformation($"Deleting consult history with ID: {id}, SalesPersonId: {salesPersonId}, DealerId: {DealerId}");
                var result = await _consultationBusiness.DeleteConsultHistoryAfterHandled(id, dataBody);
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
                return RedirectToAction("Index");
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
                var consultHistory = await _salesActivityBusiness.getSalesActivityConsultationById(id);
                if (consultHistory == null)
                {
                    _logger.LogError("Consult history not found for ID: {Id}", id);
                    return NotFound("Consult history not found.");
                }
                return View(consultHistory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving consult history details for ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View("Error!");
            }
        }
}