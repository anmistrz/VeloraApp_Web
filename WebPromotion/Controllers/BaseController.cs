using Microsoft.AspNetCore.Mvc;
using WebPromotion.Helpers.Execptions;
using WebPromotion.Services;

namespace WebPromotion.Controllers
{
    public class BaseController : Controller
    {
        private readonly ILogger<BaseController> _logger;

        public BaseController(ILogger<BaseController> logger)
        {
            _logger = logger;
        }

        protected async Task<IActionResult> HandleServiceCall(Func<Task<IActionResult>> action)
        {
            try
            {
                return await action();
            }
            catch (NotFoundConsultHistoryException ex)
            {
                _logger.LogError(ex, "Data not found");
                return View("~/Views/Error/NotFound.cshtml");
            }
            // You can add more custom exception handling here
        }
    }
}
