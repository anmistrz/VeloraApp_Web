using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebPromotion.Views.SalesPerson.DetailNotification
{
    public class DetailNotification : PageModel
    {
        private readonly ILogger<DetailNotification> _logger;

        public DetailNotification(ILogger<DetailNotification> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}