using e_commerce.DTOs.Products;
using e_commerce.Interfaces;
using e_commerce.Models.Products;
using e_commerce.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.Controllers.Products
{
    [ApiController]
    [Route("v1/api/products")]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpGet]
        [Route("get-products")]
        public async Task<ActionResult> GetAllProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 3)
        {
            var responseData = await _productService.GetAllProductsService(pageNumber, pageSize);

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



        //POST: http://localhost:5121/v1/api/products create a product
        [HttpPost]
        public async Task<IActionResult> CreateProducts([FromBody] ProductCreateDto productData)
        {

            var result = await _productService.CreateProductService(productData);
            return Created(nameof(GetProductsById), ApiResponse<ProductReadDto>.SuccessResponse(result, 201, "Product create successful"));
        }


        //GET: /v1/api/products/get-product-search-value/?searchData=app get products by search value
        [HttpGet]
        [Route("get-product-search-value")]
        public IActionResult GetProductsBySearchValue(string searchData)
        {
            var result = _productService.GetProductsBySearchValueService(searchData);
            if (result == null || !result.Any())
            {
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string> { $"Product with this {searchData} dose not exit" }, 404, "Validation failed"));
            }

            return Ok(ApiResponse<List<ProductReadDto>>.SuccessResponse(result, 200, "Success"));
        }


        //POST: v1/api/products/getProductsByIds get products by multiple ids
        [HttpPost]
        [Route("get-products-by-ids")]
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
        [HttpGet]
        [Route("get-product-by-price")]
        public IActionResult GetProductsByPrice(int minPrice = 5, int maxPrice = 1000)
        {
            var foundProduct = _productService.GetProductsByPriceService(minPrice, maxPrice);
            if (foundProduct == null || !foundProduct.Any())
            {
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string> { $"Product with these prices dose not exit" }, 404, "Validation failed"));
            }
            return Ok(ApiResponse<List<ProductReadDto>>.SuccessResponse(foundProduct, 200, "successful"));
        }
    }
}
