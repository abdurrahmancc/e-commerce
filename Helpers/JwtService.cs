using e_commerce.DTOs.Account;
using e_commerce.Enums;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace e_commerce.Helpers
{
    public class JwtService
    {

        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(UserJwtClaimsDto userClaims)
        {
            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, userClaims.Id.ToString()),
                new Claim(ClaimTypes.Email, userClaims.Email),
                new Claim(ClaimTypes.Name, userClaims.Username)
            };

            if (userClaims.Roles != null && userClaims.Roles.Any())
            {
                claims.Add(new Claim(ClaimTypes.Role, string.Join(",", userClaims.Roles)));
            }

            var claimsDictionary = new Dictionary<string, string>
                {
                    { "Country", userClaims.Country ?? string.Empty},
                    { "CountryCode", userClaims.CountryCode ?? string.Empty},
                    { "FirstName", userClaims.FirstName ?? string.Empty},
                    { "LastName", userClaims.LastName ?? string.Empty},

                };

            foreach (var claim in claimsDictionary)
            {
                if (!string.IsNullOrEmpty(claim.Value))
                {
                    claims.Add(new Claim(claim.Key, claim.Value));
                }
            }


            var keyString = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured.");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signIn
            );

            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }

        public ClaimsPrincipal? VerifyJwtToken(string token)
        {
            var keyString = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured.");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return null;
            }
        }


    }
}

