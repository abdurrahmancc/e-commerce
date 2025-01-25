using e_commerce.DTOs.Account;
using e_commerce.Helpers;
using e_commerce.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.Controllers.Account
{
    [ApiController]
    [Route("v1/api/login")]
    public class LoginController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public LoginController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }


        [HttpPost]
        public async Task<ActionResult> Login(LoginRequestDto loginInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(new List<string> { "Invalid email or password." }, 400, "Validation failed"));
            }

            var tokenValue = _jwtService.GenerateJwtToken(loginInfo.Email);

            return Ok(ApiResponse<object>.SuccessResponse(new { Token = tokenValue }, 200, "Login successful")); ;
        }
    }
}
