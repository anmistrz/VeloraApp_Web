using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebPromotion.Business.Interface;
using WebPromotion.ViewModels;
using WebPromotion.ViewModels.AccountView;
using WebPromotion.ViewModels.Modal;

namespace WebPromotion.Controllers
{
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IAccountBusiness _accountBusiness;

        public LoginController(ILogger<LoginController> logger, IAccountBusiness accountBusiness)
        {
            _logger = logger;
            _accountBusiness = accountBusiness;
        }


        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
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
            return View(new LoginViewModels());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModels model)
        {
            try
            {
                Console.WriteLine($"Attempting login... {model.Email}");
                Console.WriteLine($"Attempting login with password... {model.Password}");
                Console.WriteLine($"Model state is valid: {ModelState.IsValid}");
                if (ModelState.IsValid)
                {
                    var body = new LoginViewModel
                    {
                        email = model.Email,
                        password = model.Password
                    };
                    Console.WriteLine($"Model prepared for login: {JsonSerializer.Serialize(body)}");
                    var result = await _accountBusiness.LoginBusiness(body);
                    Console.WriteLine($"Login result: {JsonSerializer.Serialize(result)}");
                    if (result != null)
                    {
                        // Set success modal untuk ditampilkan di Index
                        TempData["SuccessModal"] = JsonSerializer.Serialize(new ModalViewModels
                        {
                            Title = "Success",
                            Message = "Login successful!",
                            ButtonText = "OK",
                            IsVisible = true,
                            Type = "success"
                        });

                        return RedirectToAction("Index", "Home");
                    }

                    Console.WriteLine("Login failed, setting error modal.");
                    TempData["ErrorModal"] = JsonSerializer.Serialize(new ModalViewModels
                    {
                        Title = "Error",
                        Message = "Invalid login attempt.",
                        ButtonText = "OK",
                        IsVisible = true,
                        Type = "failed"
                    });

                    return RedirectToAction("Index", "Login");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while logging in.");
                TempData["ErrorModal"] = JsonSerializer.Serialize(new ModalViewModels
                {
                    Title = "Error",
                    Message = "An error occurred while processing your request.",
                    ButtonText = "OK",
                    IsVisible = true,
                    Type = "failed"
                });
            }

            return View("Index", model);
        }

        [HttpGet]
        [Route("Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}