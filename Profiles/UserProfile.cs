using AutoMapper;
using e_commerce.DTOs.Account;
using e_commerce.Models.Account;
using Microsoft.AspNetCore.Identity.Data;


namespace e_commerce.Profiles
{
    public class UserProfile:Profile
    {
      public  UserProfile() {
            CreateMap<RegisterRequestDto, UserModel>();
            CreateMap<UserModel, RegisterRequestDto>();
            CreateMap<UserModel, RegisterResponseDto>();
        }
    }
}
