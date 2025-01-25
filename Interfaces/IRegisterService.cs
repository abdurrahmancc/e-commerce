using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using e_commerce.DTOs.Account;

namespace e_commerce.Interfaces
{
    public interface IRegisterService
    {
        Task<RegisterResponseDto> RegisterUserService(RegisterRequestDto registerData);
    }
}
