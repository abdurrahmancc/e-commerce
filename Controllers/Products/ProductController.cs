using e_commerce.Core;
using e_commerce.DTOs.Products;
using e_commerce.Enums;
using e_commerce.Interfaces;
using e_commerce.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;


namespace e_commerce.Controllers.Products
{
    [ApiController]
    [Route("v1/api/products")]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;
        private readonly UserTokenContext _userTokenContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductController(IProductService productService, UserTokenContext userTokenContext, IHttpContextAccessor httpContextAccessor)
        {
            _productService = productService;
            _userTokenContext = userTokenContext;
            _httpContextAccessor = httpContextAccessor;
        }



        [HttpGet("get-products")]
        //[Authorize(Roles ="User")]
        public async Task<ActionResult> GetAllProducts(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 3, 
            [FromQuery] string search = null, 
            [FromQuery] string catg = null, 
            [FromQuery] decimal? minPrice = null, 
            [FromQuery] decimal? maxPrice = null, 
            [FromQuery] decimal? rating = null,
            [FromQuery] int? status = null,
            [FromQuery] string tag = null
         )
        {
            var responseData = await _productService.GetAllProductsService(pageNumber, pageSize, search, catg, minPrice, maxPrice, rating, status, tag);

            var userTokenContext = _httpContextAccessor.HttpContext?.Items["UserTokenContext"] as UserTokenContext;
            var userId = userTokenContext?.Id.ToString();
            var user = _httpContextAccessor.HttpContext.User;
            if (responseData.Items == null || !responseData.Items.Any())
            {
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string> { $"dose not exit products" }, 404, "Validation failed"));
            }

            return Ok(ApiResponse<PaginatedResult<ProductReadDto>>.SuccessResponse(responseData, 200, "Get successful"));
        }


        //GET:  v1/api/products/GetProductsById/id--- get product by id
        [HttpGet("get-products-by-id/{id:Guid}")]
        public IActionResult GetProductsById(Guid id)
        {
            var result = _productService.GetProductsByIdService(id);
            if (result == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string> { "product with this id does not exist" }, 404, "Validation failed"));
            }
            return Ok(ApiResponse<ProductReadDto>.SuccessResponse(result, 200, "Get successful"));
        }

        [HttpDelete("delete-product-by-id")]
        public async Task<IActionResult> DeleteProductById(Guid id)
        {
            var result = await _productService.DeleteProductByIdService(id);

            if (!result)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string> { "product with this id does not exist" }, 404, "Validation failed"));
            }
            return Ok(ApiResponse<string>.SuccessResponse("Deleted", 200, "Delete successful"));
        }


        //POST: http://localhost:5121/v1/api/products create a product
        //[Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> CreateProducts([FromForm] ProductCreateDto productData)
        {

            var result = await _productService.CreateProductService(productData);
            return Created(nameof(GetProductsById), ApiResponse<ProductReadDto>.SuccessResponse(result, 201, "Product create successful"));
        }


        //GET: /v1/api/products/get-product-search-value/?searchData=app get products by search value
        [HttpGet("get-products-search-value")]
        public async Task<IActionResult> GetProductsBySearchValue([FromQuery] string searchData, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 3)
        {
            var result = await  _productService.GetProductsBySearchValueService(searchData, pageNumber, pageSize);
            if (result == null || result.Items == null || !result.Items.Any())
            {
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string> { $"Product with this {searchData} dose not exit" }, 404, "Validation failed"));
            }

            return Ok(ApiResponse<PaginatedResult<ProductReadDto>>.SuccessResponse(result, 200, "Success"));
        }


        //POST: v1/api/products/getProductsByIds get products by multiple ids
        [HttpPost("get-products-by-ids")]
        public IActionResult GetProductsByIds(List<Guid> ids)
        {

            var foundProducts = _productService.GetProductsByIdsService(ids);
            if (foundProducts == null || !foundProducts.Any())
            {
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string> { $"Product with these ids dose not exit" }, 404, "Validation failed"));
            }

            return Ok(ApiResponse<List<ProductReadDto>>.SuccessResponse(foundProducts, 200, "Success"));
        }


        //GET: http://localhost:5121/v1/api/products/getProductByPrice?minPrice=10&maxPrice=100 get product by min and max prices
        [HttpGet("get-products-by-price")]
        public async Task<IActionResult> GetProductsByPrice([FromQuery] int minPrice = 5, [FromQuery] int maxPrice = 1000, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 3)
        {
            var result = await _productService.GetProductsByPriceService(minPrice, maxPrice, pageNumber, pageSize);
            if (result == null || result.Items == null || !result.Items.Any())
            {
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string> { $"Product with these prices dose not exit" }, 404, "Validation failed"));
            }
            return Ok(ApiResponse<PaginatedResult<ProductReadDto>>.SuccessResponse(result, 200, "successful"));
        }

        [HttpGet("get-products-by-rating/{rating}")]
        public async Task<IActionResult> GetProductsByRating([FromRoute] double rating, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 3)
        {
            var result = await _productService.GetProductByRatingServiceAsync(rating, pageNumber, pageSize);
            if (result == null || result.Items == null || !result.Items.Any())
            {
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string> { $"Product with this rating does not exist" }, 404, "Validation failed"));
            }
            return Ok(ApiResponse<PaginatedResult<ProductReadDto>>.SuccessResponse(result, 200, "successful"));
        }

        [HttpGet("get-products-by-status/{status}")]
        public async Task<IActionResult> GetProductsByStatus([FromRoute] int status, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 3)
        {
            var res = await _productService.GetProductByStatusServiceAsync(status, pageNumber, pageSize);
            if (res == null || res.Items == null || !res.Items.Any())
            {
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string> { $"Product with this status does not exist" }, 404, "Validation failed"));
            }
            return Ok(ApiResponse<PaginatedResult<ProductReadDto>>.SuccessResponse(res, 200, "successful"));
        }

    }
}
