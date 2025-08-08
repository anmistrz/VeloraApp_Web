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
namespace WebPromotion.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConsultationBusiness _consultationBusiness;
        private readonly IDealerCarBusiness _dealerCarBusiness;
        private readonly ITestDriveBusiness _testDriveBusiness;


        public HomeController(
            ILogger<HomeController> logger,
            IConsultationBusiness consultationBusiness,
            IDealerCarBusiness dealerCarBusiness,
            ITestDriveBusiness testDriveBusiness)
        {
            _logger = logger;
            _consultationBusiness = consultationBusiness;
            _dealerCarBusiness = dealerCarBusiness ?? throw new ArgumentNullException(nameof(dealerCarBusiness));
            _testDriveBusiness = testDriveBusiness ?? throw new ArgumentNullException(nameof(testDriveBusiness));
        }

        public IActionResult Index()
        {
            try
            {

                var DealerCarOptions = _dealerCarBusiness.GetOptionsDealerCarUnitByStatus("TestDrive") ?? new List<List<DealerCarUnitOptionsDTO>>();
                Console.WriteLine($"DealerCarOptions: {JsonSerializer.Serialize(DealerCarOptions)}");

                ViewBag.DealerCarOptions = DealerCarOptions;

                ViewBag.DealerOptions = DealerCarOptions?
                    .SelectMany(optionList => optionList.SelectMany(option => option.Dealers))
                    .Select(x => new SelectListItem
                    {
                        Value = x.DealerID.ToString(),
                        Text = x.DealerName
                    }).ToList() ?? new List<SelectListItem>();

                ViewBag.CarOptions = DealerCarOptions?
                    .SelectMany(optionList => optionList.SelectMany(option => option.Cars))
                    .Select(x => new SelectListItem
                    {
                        Value = $"{x.CarId} - {x.DealerCarUnitId}",
                        Text = x.CarName
                    }).ToList() ?? new List<SelectListItem>();

                // Set default values for the dropdowns


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

        [HttpPost]
        public async Task<IActionResult> AddConsultation(ConsultHistoryInsertGuestViewModels model)
        {
            Console.WriteLine($"Received model: {JsonSerializer.Serialize(model)}");
            
            // Additional validation for DealerId
            if (model.DealerId == 0)
            {
                ModelState.AddModelError("DealerId", "Please select a dealer");
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    var getDealerCarUnit = model.DealerCarUnitId.Split('-');
                    var dataBody = new ConsultationInsertGuestDTO
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        DealerCarUnitId = int.Parse(getDealerCarUnit[1].Trim()),
                        // ConsultDate = model.ConsultDate,
                        Note = model.Note,
                        SalesPersonId = model.SalesPersonId,
                        Budget = model.Budget ?? 0,
                        DealerId = model.DealerId
                    };

                    Console.WriteLine($"Data to be sent: {JsonSerializer.Serialize(dataBody)}");

                    await _consultationBusiness.CreateConsultHistoryGuest(dataBody);

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
                    return RedirectToAction("Index");
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
        public async Task<IActionResult> AddTestDrive(TestDriveGuestViewModels model)
        {
            Console.WriteLine($"Received model: {JsonSerializer.Serialize(model)}");

            Console.WriteLine($"DealerCarUnitId: {ModelState.IsValid}");

            if (ModelState.IsValid)
            {
                try
                {
                    var getDealerCarUnit = model.DealerCarUnitId.Split('-');
                    var dataBody = new TestDriveInsertGuestDTO
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        AppointmentDate = model.AppointmentDate,
                        Note = model.Note,
                        DealerId = model.DealerId,
                        DealerCarUnitId = int.Parse(getDealerCarUnit[1].Trim()),
                        CarId = int.Parse(getDealerCarUnit[0].Trim())
                    };

                    Console.WriteLine($"Data TEST DRIVE to be sent: {JsonSerializer.Serialize(dataBody)}");

                    await _testDriveBusiness.InsertTestDriveGuest(dataBody);

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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
