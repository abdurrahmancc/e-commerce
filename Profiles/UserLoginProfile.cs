using AutoMapper;
using e_commerce.DTOs.Account;
using e_commerce.Models.Account;

namespace e_commerce.Profiles
{
    public class UserLoginProfile:Profile
    {
        public UserLoginProfile() {
            CreateMap<UserLoginModel, LoginRequestDto>();
            CreateMap<UserLoginModel, LoginResponseDto>();
            CreateMap<LoginRequestDto, LoginResponseDto>();
            CreateMap<LoginResponseDto, LoginResponseDto>();
            CreateMap<UserDto, UserJwtClaimsDto>();
        }
    }
}
