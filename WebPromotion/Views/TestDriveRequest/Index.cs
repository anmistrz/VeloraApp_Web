using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebPromotion.Views.ConsultationRequest
{
    public class ConsultationRequest : PageModel
    {
        private readonly ILogger<ConsultationRequest> _logger;

        public ConsultationRequest(ILogger<ConsultationRequest> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}