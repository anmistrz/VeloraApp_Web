using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebPromotion.ViewModels.Modal;

namespace WebPromotion.Controllers
{
    [Route("[controller]")]
    public class SalesPersonController : Controller
    {
        private readonly ILogger<SalesPersonController> _logger;

        public SalesPersonController(ILogger<SalesPersonController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}