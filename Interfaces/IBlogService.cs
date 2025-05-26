using e_commerce.DTOs.Blogs;
using e_commerce.DTOs.Products;
using e_commerce.Models.Blogs;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.Interfaces
{
    public interface IBlogService
    {
        public Task<JsonResult> CreateBlogService(BlogCreateDto blogModel);
        public Task<PaginatedResult<BlogCreateDto>> GetBlogsService(int pageNumber, int pageSize, string search, string catg, string tag);

    }
}
