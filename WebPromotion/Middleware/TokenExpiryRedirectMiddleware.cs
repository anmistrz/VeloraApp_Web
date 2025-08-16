using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace WebPromotion.Middleware
{

    public class TokenExpiryRedirectMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly HashSet<string> PathsNotHandledByMiddleware = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "/Login",
            "/Register",
            "/",
            "/Error",
            "/Home/Index",
            "/Home/AddConsultation",
            "/Home/AddTestDrive"
        };

        public TokenExpiryRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Skip middleware for certain paths
            if (PathsNotHandledByMiddleware.Contains(context.Request.Path))
            {
                await _next(context);
                return;
            }

            var token = context.Request.Cookies["MyJwtCookie"];

            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine($"Token missing for path: {context.Request.Path}, redirecting to login.");
                context.Response.Redirect("/Login");
                return;
            }

            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadJwtToken(token);

            // Check expiry
            if (jwtToken.ValidTo < DateTime.UtcNow)
            {
                Console.WriteLine("Token expired, redirecting to login page.");
                context.Response.Redirect("/Login");
                return;
            }

            

            await _next(context);
        }
    }
}