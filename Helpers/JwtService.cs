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
                new Claim(JwtRegisteredClaimNames.Jti, userClaims.Id.ToString()),
                new Claim("Email", userClaims.Email),
            };

            foreach (var role in userClaims.Role)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var claimsDictionary = new Dictionary<string, string>
                {
                    { "Username", userClaims.Username },
                    { "Country", userClaims.Country },
                    { "CountryCode", userClaims.CountryCode },
                    { "FirstName", userClaims.FirstName },
                    { "LastName", userClaims.LastName }
                };

            foreach (var claim in claimsDictionary)
            {
                if (!string.IsNullOrEmpty(claim.Value))
                {
                    claims.Add(new Claim(claim.Key, claim.Value));
                }
            }



            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
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
    }
}

