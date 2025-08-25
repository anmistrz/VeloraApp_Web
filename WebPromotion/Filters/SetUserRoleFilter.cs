using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace WebPromotion.Filters
{
    public class SetUserRoleFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as Controller;
            var token = controller?.Request.Cookies["MyJwtCookie"];
            Console.WriteLine($"Token: {token}");
            Console.WriteLine($"Controller: {controller?.GetType().Name}");
            string? role = null;
            if (!string.IsNullOrEmpty(token) && controller != null)
            {
                Console.WriteLine("Token is not null or empty, proceeding to decode.");
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);
                role = jwt.Claims.FirstOrDefault(c => c.Type == "role" || c.Type == ClaimTypes.Role)?.Value;
                Console.WriteLine($"Role: {role}");
                controller.ViewBag.FullName = jwt.Claims.FirstOrDefault(c => c.Type == "given_name" || c.Type == ClaimTypes.GivenName)?.Value;
                controller.ViewBag.Email = jwt.Claims.FirstOrDefault(c => c.Type == "email" || c.Type == ClaimTypes.Email)?.Value;
                // Parse DealerId claim safely to avoid FormatException when claim value is empty
                var dealerClaim = jwt.Claims.FirstOrDefault(c => c.Type == "DealerId")?.Value;
                if (!string.IsNullOrWhiteSpace(dealerClaim) && int.TryParse(dealerClaim, out var dealerId))
                {
                    controller.ViewBag.DealerId = dealerId;
                }
                else
                {
                    controller.ViewBag.DealerId = null; // keep as null so callers can default to 0
                }

                // Parse SalesPersonId claim safely
                var salesPersonClaim = jwt.Claims.FirstOrDefault(c => c.Type == "SalesPersonId")?.Value;
                Console.WriteLine($"SalesPersonId Claim: {salesPersonClaim}");
                if (!string.IsNullOrWhiteSpace(salesPersonClaim) && int.TryParse(salesPersonClaim, out var salesPersonId))
                {
                    controller.ViewBag.SalesPersonId = salesPersonId;
                }
                else
                {
                    controller.ViewBag.SalesPersonId = null; // keep as null so callers can default to 0
                }
            }
            if (controller != null)
            {
                controller.ViewBag.UserRole = role;
            }
        }
        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
