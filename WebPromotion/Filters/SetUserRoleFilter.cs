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
            string role = null;
            if (!string.IsNullOrEmpty(token) && controller != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);
                role = jwt.Claims.FirstOrDefault(c => c.Type == "role" || c.Type == ClaimTypes.Role)?.Value;
                controller.ViewBag.FullName = jwt.Claims.FirstOrDefault(c => c.Type == "given_name" || c.Type == ClaimTypes.GivenName)?.Value;
                controller.ViewBag.Email = jwt.Claims.FirstOrDefault(c => c.Type == "email" || c.Type == ClaimTypes.Email)?.Value;
                controller.ViewBag.DealerId = jwt.Claims.FirstOrDefault(c => c.Type == "DealerId")?.Value;
            }
            if (controller != null)
            {
                controller.ViewBag.UserRole = role;
            }
        }
        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
