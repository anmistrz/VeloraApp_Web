using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebPromotion.Views.Error.NotFound
{
    public class NotFoundPage : PageModel
    {
        private readonly ILogger<NotFoundPage> _logger;

        public NotFoundPage(ILogger<NotFoundPage> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}