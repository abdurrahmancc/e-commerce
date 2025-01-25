using e_commerce.DTOs.Account;
using e_commerce.Interfaces;
using e_commerce.Utilities;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.Controllers.Account
{
    [ApiController]
    [Route("v1/api/register")]
    public class RegisterController: ControllerBase
    {
        private readonly IRegisterService _registerService;
        public RegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
        }
        [HttpPost]
        public async Task<ActionResult> Register(RegisterRequestDto registerData)
        {
            var result = await _registerService.RegisterUserService(registerData);
            return Ok(ApiResponse<RegisterResponseDto>.SuccessResponse(result, 200, "Register successfull"));
        }
    }
}
