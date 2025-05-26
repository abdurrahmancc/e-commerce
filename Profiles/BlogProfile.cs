using AutoMapper;
using e_commerce.DTOs.Blogs;
using e_commerce.DTOs.Products;
using e_commerce.Models.Blogs;
using e_commerce.Models.Products;

namespace e_commerce.Profiles
{
    public class BlogProfile  : Profile
    {
        public BlogProfile()
        {
            CreateMap<BlogModel, BlogCreateDto>();
            CreateMap<BlogCreateDto, BlogModel>();
            CreateMap<BlogCreateDto, BlogComment>();
        }
    }
}
