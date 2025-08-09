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

        public TokenExpiryRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Cookies["MyJwtCookie"];
            var listPathNotHandledMiddleware = new List<string>();
            {
                listPathNotHandledMiddleware.Add("/Login");
                listPathNotHandledMiddleware.Add("/Register");
                listPathNotHandledMiddleware.Add("/");
                listPathNotHandledMiddleware.Add("/Error");
            }
            Console.WriteLine($"Token from cookie: {token}");   
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                Console.WriteLine($"Token: {token}");
                // Check expiry
                if (jwt.ValidTo < DateTime.UtcNow)
                {
                    Console.WriteLine("Token expired, redirecting to login page.");
                    context.Response.Redirect("/Login");
                    return;
                }
            }

            if (listPathNotHandledMiddleware.Any(path => context.Request.Path.StartsWithSegments(path)))
            {
                Console.WriteLine($"Path {context.Request.Path} is not handled by middleware, skipping token validation.");
                await _next(context);
                return;
            }
            else
            {
                if (string.IsNullOrEmpty(token))
                {
                    context.Response.Redirect("/Login");
                    return;
                }
            }
            await _next(context);
        }
    }
}