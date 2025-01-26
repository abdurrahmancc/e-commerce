using e_commerce.DTOs.Account;
using e_commerce.Helpers;
using e_commerce.Interfaces;
using e_commerce.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.Controllers.Account
{
    [ApiController]
    [Route("v1/api/login")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService) {
            _loginService = loginService;
        }


        [HttpPost]
        public async Task<ActionResult> Login(LoginRequestDto loginInfo)
        {

            var result = await _loginService.LoginUserService(loginInfo);

            if (result == null) { 
                 return NotFound(ApiResponse<object>.ErrorResponse(new List<string> { "Invalid email or password." }, 404, "Validation failed"));
            }


            return Ok(ApiResponse<LoginResponseDto>.SuccessResponse(result, 200, "Login successful")); ;
        }
    }
}
