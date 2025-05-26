using e_commerce.DTOs.Blogs;
using e_commerce.DTOs.Products;
using e_commerce.Interfaces;
using e_commerce.Models.Blogs;
using e_commerce.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.Controllers.Blogs
{
    [ApiController]
    [Route("v1/api/blogs")]
    public class BlogController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBlogService _blogService;
        public BlogController(IBlogService blogService, IHttpContextAccessor httpContextAccessor) {
            _blogService = blogService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlog([FromBody] BlogCreateDto blogModel)
        {
            var res = await _blogService.CreateBlogService(blogModel);
            dynamic result = res?.Value;

            if (result.StatusCode != 200)
            {
                return NotFound(ApiResponse<string>.ErrorResponse(new List<string> { result.error }, result.StatusCode, "Something went wrong"));
            }

            return Ok(ApiResponse<object>.SuccessResponse(new { BlogId = "" }, 200, "Blog saved successfully"));
        }

        [HttpGet]
        public async Task<IActionResult> GetBlogs([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 6, [FromQuery] string search = "", [FromQuery] string catg = "", [FromQuery] string tag = "")
        {
         var res = await _blogService.GetBlogsService( pageNumber,  pageSize,  search,  catg,  tag);
            return Ok(res);
        }

    }
}
