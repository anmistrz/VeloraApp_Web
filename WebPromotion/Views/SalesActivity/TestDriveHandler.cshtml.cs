using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebPromotion.Views.TestDriveHandler
{
    public class TestDriveHandler : PageModel
    {
        private readonly ILogger<TestDriveHandler> _logger;

        public TestDriveHandler(ILogger<TestDriveHandler> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}