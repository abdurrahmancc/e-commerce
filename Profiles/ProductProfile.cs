using AutoMapper;
using e_commerce.DTOs.Products;
using e_commerce.Models.Products;

namespace e_commerce.Profiles
{
    public class ProductProfile: Profile
    {
        public ProductProfile() { 
            CreateMap<ProductModel, ProductCreateDto>();
            CreateMap<ProductModel, ProductReadDto>();
            CreateMap<ProductCreateDto, ProductModel>();
        }
    }
}
