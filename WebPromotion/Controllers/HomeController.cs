using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebPromotion.Models;
using WebPromotion.ViewModels.ConsultHistoryView;
using WebPromotion.ViewModels.Modal;
using WebPromotion.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebPromotion.Services.DealerCar;
using WebPromotion.Services.Consultation;
using WebPromotion.Services.DTO;
using WebPromotion.ViewModels.TestDriveView;
using WebPromotion.Business;
using WebPromotion.Business.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using WebPromotion.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using WebPromotion.ViewModels.TestDriveView.TestDriveGuestViewModel;
namespace WebPromotion.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConsultationBusiness _consultationBusiness;
        private readonly IDealerCarBusiness _dealerCarBusiness;
        private readonly ITestDriveBusiness _testDriveBusiness;
        private readonly IAccountServices _accountServices;


        public HomeController(
            ILogger<HomeController> logger,
            IConsultationBusiness consultationBusiness,
            IDealerCarBusiness dealerCarBusiness,
            ITestDriveBusiness testDriveBusiness,
            IAccountServices accountServices)
        {
            _logger = logger;
            _consultationBusiness = consultationBusiness;
            _dealerCarBusiness = dealerCarBusiness ?? throw new ArgumentNullException(nameof(dealerCarBusiness));
            _testDriveBusiness = testDriveBusiness ?? throw new ArgumentNullException(nameof(testDriveBusiness));
            _accountServices = accountServices ?? throw new ArgumentNullException(nameof(accountServices));
        }

        public IActionResult Index()
        {
            try
            {
                var DealerCarOptions = _dealerCarBusiness.GetOptionsDealerCarUnitByStatus("TestDrive") ?? new List<List<DealerCarUnitOptionsDTO>>();
                Console.WriteLine($"DealerCarOptions: {JsonSerializer.Serialize(DealerCarOptions)}");

                // Pass the raw data for use in JS
                ViewBag.DealerCarData = DealerCarOptions.Select(optionList => optionList.Select(option => new {
                    dealers = option.Dealers.Select(d => new { dealerID = d.DealerID, dealerName = d.DealerName, dealerCarUnitId = d.DealerCarUnitId }),
                    cars = option.Cars.Select(c => new { carId = c.CarId, dealerCarUnitId = c.DealerCarUnitId, carName = c.CarName }),
                    dealerCarUnits = option.DealerCarUnits.Select(u => new { dealerCarUnitId = u.DealerCarUnitId })
                }).ToList()).SelectMany(x => x).ToList();

                Console.WriteLine("ErrorModal", TempData["ErrorModal"]);
                // Handle modal dari TempData setelah redirect
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving car options");
                ViewBag.ErrorMessage = "An error occurred while loading car options.";
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddConsultation(ConsultHistoryInsertGuestViewModels model)
        {
            
            // Additional validation for DealerId
            if (model.DealerId == 0)
            {
                ModelState.AddModelError("DealerId", "Please select a dealer");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _consultationBusiness.CreateConsultHistoryGuest(model);
                    
                    // Set success modal untuk ditampilkan di Index
                    TempData["SuccessModal"] = JsonSerializer.Serialize(new ModalViewModels
                    {
                        Title = "Success",
                        Message = "Consultation has been successfully added!",
                        ButtonText = "OK",
                        IsVisible = true,
                        Type = "success"
                    });

                    // Redirect ke Index setelah insert berhasil
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error adding consultation");

                    // Set error modal untuk ditampilkan di Index
                    TempData["ErrorModal"] = JsonSerializer.Serialize(new ModalViewModels
                    {
                        Title = "Error",
                        Message = "Failed to add consultation. Please try again.",
                        ButtonText = "OK",
                        IsVisible = true,
                        Type = "failed"
                    });

                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                // Set validation error modal untuk ditampilkan di Index
                TempData["ErrorModal"] = JsonSerializer.Serialize(new ModalViewModels
                {
                    Title = "Validation Error",
                    Message = "Please fill in all required fields correctly.",
                    ButtonText = "OK",
                    IsVisible = true,
                    Type = "failed"
                });

                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddTestDrive(TestDriveGuestViewModels model)
        {
            Console.WriteLine($"Received model: {JsonSerializer.Serialize(model)}");
            Console.WriteLine($"DealerCarUnitId: {ModelState.IsValid}");

            // Validate DealerCarUnitId is not null/empty and is a valid integer
            int dealerCarUnitIdInt = 0;
            if (string.IsNullOrWhiteSpace(model.DealerCarUnitId) || !int.TryParse(model.DealerCarUnitId, out dealerCarUnitIdInt))
            {
                ModelState.AddModelError("DealerCarUnitId", "Please select a valid car unit.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var getDealerCarData = _dealerCarBusiness.GetOptionsDealerCarUnitByStatus("TestDrive");
                    var getCarIdByDealerCarUnit = 0;
                    if (getDealerCarData != null && getDealerCarData.Any())
                    {
                        var dealerCarUnit = getDealerCarData
                            .SelectMany(x => x)
                            .FirstOrDefault(x => x.DealerCarUnits.Any(u => u.DealerCarUnitId == dealerCarUnitIdInt));
                        if (dealerCarUnit != null)
                        {
                            getCarIdByDealerCarUnit = dealerCarUnit.Cars
                                .FirstOrDefault(c => c.DealerCarUnitId == dealerCarUnitIdInt)?.CarId ?? 0;
                        }
                    }

                    var dataBody = new TestDriveGuestViewModels
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        ConsultDate = model.ConsultDate,
                        Note = model.Note,
                        DealerId = model.DealerId,
                        DealerCarUnitId = model.DealerCarUnitId,
                        CarId = getCarIdByDealerCarUnit
                    };

                    Console.WriteLine($"Data TEST DRIVE to be sent: {JsonSerializer.Serialize(dataBody)}");

                    await _testDriveBusiness.InsertTestDriveGuest(dataBody, getCarIdByDealerCarUnit);

                    // Set success modal untuk ditampilkan di Index
                    TempData["SuccessModal"] = JsonSerializer.Serialize(new ModalViewModels
                    {
                        Title = "Success",
                        Message = "Test drive has been successfully added!",
                        ButtonText = "OK",
                        IsVisible = true,
                        Type = "success"
                    });

                    // Redirect ke Index setelah insert berhasil
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error adding test drive");

                    // Set error modal untuk ditampilkan di Index
                    TempData["ErrorModal"] = JsonSerializer.Serialize(new ModalViewModels
                    {
                        Title = "Error",
                        Message = "Failed to add test drive. Please try again.",
                        ButtonText = "OK",
                        IsVisible = true,
                        Type = "failed"
                    });

                    return RedirectToAction("Index");
                }
            }
            else
            {
                // Set validation error modal untuk ditampilkan di Index
                TempData["ErrorModal"] = JsonSerializer.Serialize(new ModalViewModels
                {
                    Title = "Validation Error",
                    Message = "Please fill in all required fields correctly.",
                    ButtonText = "OK",
                    IsVisible = true,
                    Type = "failed"
                });

                return RedirectToAction("Index");
            }
        }   

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Response.Cookies.Delete("MyJwtCookie");
            HttpContext.Response.Cookies.Delete(".AspNetCore.Antiforgery.YZxJgg4YA_s");
            return RedirectToAction("Index", "Login");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // [HttpGet]
        // public IActionResult GetCarUnitSelect(int dealerId)
        // {
        //     var carUnits = _dealerCarBusiness.GetOptionsDealerCarUnitByStatus("TestDrive")
        //         ?.SelectMany(list => list.SelectMany(opt => opt.Cars))
        //         .Where(x => x.DealerCarUnitId == dealerId)
        //         .Select(x => new SelectListItem
        //         {
        //             Value = $"{x.CarId} - {x.DealerCarUnitId}",
        //             Text = x.CarName
        //         }).ToList();

        //     return PartialView("_CarUnitSelect", carUnits);
        // }
    }
}
