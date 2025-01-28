using e_commerce.Core;
using e_commerce.DTOs.Account;
using e_commerce.Enums;
using e_commerce.Helpers;
using System.Data;
using System.Diagnostics.Metrics;
using System.Security.Claims;

namespace e_commerce.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, JwtService jwtService)
        {
            var authenticationHeader = context.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authenticationHeader) && authenticationHeader.StartsWith("Bearer "))
            {
                var token = authenticationHeader.Substring("Bearer ".Length);
                var claimPrincipal = jwtService.VerifyJwtToken(token);

                
                if (claimPrincipal != null)
                {
                    context.Items["UserTokenContext"] = new UserTokenContext
                    {
                        Id = Guid.TryParse(claimPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : Guid.Empty,
                        Email = claimPrincipal.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty,
                        Username = claimPrincipal.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty,
                        Roles = claimPrincipal.Claims.Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => Enum.TryParse(typeof(UserRole), c.Value, out var role) ? (UserRole)role : UserRole.User).Cast<UserRole>().ToList(),
                        Country = claimPrincipal.FindFirst("Country")?.Value ?? string.Empty,
                        CountryCode = claimPrincipal.FindFirst("CountryCode")?.Value ?? string.Empty,
                        FirstName = claimPrincipal.FindFirst("FirstName")?.Value ?? string.Empty,
                        LastName = claimPrincipal.FindFirst("LastName")?.Value ?? string.Empty
                    };
                }
            }

            // Call the next middleware in the pipeline
            await _next(context);
        }


    }
}
