using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebPromotion.Views.ConsultationHandler
{
    public class ConsultationHandler : PageModel
    {
        private readonly ILogger<ConsultationHandler> _logger;

        public ConsultationHandler(ILogger<ConsultationHandler> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}