using AutoMapper;
using e_commerce.DTOs.Account;
using e_commerce.Models.Account;
using Microsoft.AspNetCore.Identity.Data;


namespace e_commerce.Profiles
{
    public class UserRegisterProfile:Profile
    {
      public  UserRegisterProfile() {
            CreateMap<RegisterRequestDto, UserRegisterModel>();
            CreateMap<UserRegisterModel, RegisterRequestDto>();
            CreateMap<UserRegisterModel, RegisterResponseDto>();
        }
    }
}
