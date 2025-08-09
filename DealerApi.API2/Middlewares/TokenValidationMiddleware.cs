// using System;
// using System.Collections.Generic;
// using System.IdentityModel.Tokens.Jwt;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using Microsoft.IdentityModel.Tokens;

// namespace DealerApi.API2.Middlewares
// {
//     public class TokenValidationMiddleware
//     {
//         private readonly RequestDelegate _next;
//         private readonly string _secretKey;

//         public TokenValidationMiddleware(RequestDelegate next, string secretKey)
//         {
//             _next = next;
//             _secretKey = secretKey;
//         }

//         public async Task InvokeAsync(HttpContext context)
//         {
//             Console.WriteLine("TokenValidationMiddleware", context.Request.Path.StartsWithSegments("/api/login") ? "Login request" : "Non-login request");
//             // except when login
//             if (context.Request.Path.StartsWithSegments("/api/login"))
//             {
//                 await _next(context);
//                 return;
//             }

//             var token = context.User?.Claims.FirstOrDefault(c => c.Type == "Token")?.Value;
//             if (!string.IsNullOrEmpty(token))
//             {
//                 var tokenHandler = new JwtSecurityTokenHandler();
//                 var key = Encoding.UTF8.GetBytes(_secretKey);
//                 try
//                 {
//                     tokenHandler.ValidateToken(token, new TokenValidationParameters
//                     {
//                         ValidateIssuerSigningKey = true,
//                         IssuerSigningKey = new SymmetricSecurityKey(key),
//                         ValidateIssuer = false,
//                         ValidateAudience = false,
//                         // Set sesuai kebutuhan
//                     }, out SecurityToken validatedToken);
//                 }
//                 catch
//                 {
//                     context.Response.StatusCode = 401;
//                     await context.Response.WriteAsync("Invalid Token");
//                     return;
//                 }
//             }
//             else
//             {
//                 context.Response.StatusCode = 401;
//                 await context.Response.WriteAsync("Token Required");
//                 return;
//             }

//             await _next(context);
//         }
//     }
// }