using AutoMapper;
using e_commerce.data;
using e_commerce.DTOs.Account;
using e_commerce.Interfaces;
using e_commerce.Models.Account;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.Services.Account
{
    public class RegisterService:IRegisterService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public RegisterService(AppDbContext appDbContext, IMapper mapper) { 
            _appDbContext = appDbContext;
            _mapper = mapper;
        }
        public async Task<RegisterResponseDto> RegisterUserService(RegisterRequestDto registerData) {
            var newUser = _mapper.Map<UserModel>(registerData);
            newUser.Id = Guid.NewGuid();
            newUser.CreateAt = DateTime.Now;
            newUser.UpdateAt = DateTime.Now;
            newUser.Username = newUser.Email.Split('@')[0];

            await _appDbContext.AddAsync(newUser);
            await _appDbContext.SaveChangesAsync();

            return _mapper.Map<RegisterResponseDto>(newUser);
        }

    }
}
