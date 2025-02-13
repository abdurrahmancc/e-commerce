using AutoMapper;
using e_commerce.data;
using e_commerce.DTOs.Account;
using e_commerce.Enums;
using e_commerce.Helpers;
using e_commerce.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.Services.Account
{
    public class LoginService: ILoginService
    {

        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly JwtService _jwtService;

        public LoginService(AppDbContext appDbContext, JwtService jwtService, IMapper mapper) 
        {
            _appDbContext = appDbContext;
            _jwtService = jwtService;
            _mapper = mapper;
        }
      public async  Task<LoginResponseDto> LoginUserService(LoginRequestDto loginData)
        {

            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => (u.Email == loginData.Email || u.Username == loginData.Email) && u.Password == loginData.Password);
            if (user != null) {
                var userClaims = new UserJwtClaimsDto
                {
                     Id = user.Id,
                    Email = user.Email,
                    Username = user.Username,
                    Roles = user.Roles,
                    Country = user.Country,
                    CountryCode= user.CountryCode,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                };
                var jwtToken = _jwtService.GenerateJwtToken(userClaims);
                var response = new LoginResponseDto
                {
                    Id = user.Id,
                    UserName = user.Username,
                    Status = user.Status,
                    Token = jwtToken
                };
                return response;
            }
             return null;
        }
    }
}
