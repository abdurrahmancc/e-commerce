using e_commerce.DTOs.Account;

namespace e_commerce.Interfaces
{
    public interface ILoginService
    {
        Task<LoginResponseDto> LoginUserService(LoginRequestDto loginData);
    }
}
