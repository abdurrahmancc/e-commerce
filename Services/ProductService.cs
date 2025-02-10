using AutoMapper;
using e_commerce.data;
using e_commerce.DTOs.Products;
using e_commerce.Interfaces;
using e_commerce.Models.Products;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace e_commerce.Services
{
    public class ProductService: IProductService
    {

        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public ProductService(AppDbContext appDbcontext, IMapper mapper)
        {
            _appDbContext = appDbcontext;
            _mapper = mapper;
        }



        public async Task<PaginatedResult<ProductReadDto>> GetAllProductsService(int pageNumber, int pageSize)
        {
            // Validate pageNumber and pageSize
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            // Get the total count of products
            var totalItems = await _appDbContext.Products.CountAsync();

            // Calculate total pages
            var totalPage = (int)Math.Ceiling((double)totalItems / pageSize);

            // Adjust pageNumber to stay within bounds
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPage));

            // Calculate the number of items to skip
            var skip = (pageNumber - 1) * pageSize;

            // Fetch paginated products
            var productList = await _appDbContext.Products.Skip(skip).Take(pageSize).ToListAsync();

            // Map to DTO
            var result = _mapper.Map<List<ProductReadDto>>(productList);

            // Build the paginated response
            var pager = new PaginatedResult<ProductReadDto>
            {
                Items = result,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
                StartPage = Math.Max(1, pageNumber - 2),
                EndPage = Math.Min(totalPage, pageNumber + 2)
            };

            return pager;
        }



        public ProductReadDto? GetProductsByIdService(Guid Id)
        {
            var result = _appDbContext.Products.FirstOrDefault(prod => prod.Id == Id);

            return result == null ? null : _mapper.Map<ProductReadDto>(result);
        }



        public async Task<ProductReadDto> CreateProductService(ProductCreateDto productData)
        {
            var newProduct = _mapper.Map<ProductModel>(productData);

            newProduct.Id = Guid.NewGuid();
            newProduct.UpdatedAt = DateTime.UtcNow;
            newProduct.CreatedAt = DateTime.UtcNow;
            newProduct.Date = DateTime.UtcNow;
            if (productData.Specifications != null)
            {
                newProduct.Specifications = JsonConvert.SerializeObject(productData.Specifications);
            }
            await _appDbContext.AddAsync(newProduct);
            await _appDbContext.SaveChangesAsync();

            return _mapper.Map<ProductReadDto>(newProduct); ;

        }


        public List<ProductReadDto>? GetProductsBySearchValueService(string searchData)
        {
            var searchResultProducts = _appDbContext.Products.Where(prod => prod.Name.Contains(searchData, StringComparison.OrdinalIgnoreCase)).ToList();

            if (searchResultProducts == null || !searchResultProducts.Any())
            {
                return null;
            }

            return _mapper.Map<List<ProductReadDto>>(searchResultProducts);
        }



        public List<ProductReadDto>? GetProductsByIdsService(List<Guid> Ids)
        {
            var foundProducts = _appDbContext.Products.Where(prod => Ids.Contains(prod.Id));
            if (foundProducts == null || !foundProducts.Any())
            {
                return null;
            }

            return _mapper.Map<List<ProductReadDto>>(foundProducts);
        }


        public List<ProductReadDto> GetProductsByPriceService(int minPrice, int maxPrice)
        {
            var foundProduct = _appDbContext.Products.Where(prod => prod.Price >= minPrice && prod.Price <= maxPrice).ToList();
            //if (foundProduct == null || !foundProduct.Any())
            //{
            //    return null;

            //}
            return _mapper.Map<List<ProductReadDto>>(foundProduct);
        }

    }
}