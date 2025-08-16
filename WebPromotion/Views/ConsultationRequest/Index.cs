using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebPromotion.Views.TestDriveRequest
{
    public class TestDriveRequest : PageModel
    {
        private readonly ILogger<TestDriveRequest> _logger;

        public TestDriveRequest(ILogger<TestDriveRequest> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}