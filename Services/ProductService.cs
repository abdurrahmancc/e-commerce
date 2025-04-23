using AutoMapper;
using e_commerce.data;
using e_commerce.DTOs.Products;
using e_commerce.Interfaces;
using e_commerce.Models.Products;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using e_commerce.Helpers;
using System.Linq;
using e_commerce.Enums;
using e_commerce.Services.FilesManagement;
using SixLabors.ImageSharp;
using static e_commerce.Enums.ProductEnums;

namespace e_commerce.Services
{
    public class ProductService: IProductService
    {
        private readonly FilestService _filesService;
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly FilesManagementHelper _filesManagementHelper;
        private readonly FilestService _filestService;
        public ProductService(AppDbContext appDbcontext, IMapper mapper, FilestService filesService, FilesManagementHelper filesManagementHelper, FilestService filestService)
        {
            _appDbContext = appDbcontext;
            _mapper = mapper;
            _filesService = filesService;
            _filesManagementHelper = filesManagementHelper;
            _filestService = filestService;
        }



        public async Task<PaginatedResult<ProductReadDto>> GetAllProductsService(int pageNumber, int pageSize, string search, string catg, decimal? minPrice, decimal? maxPrice, decimal? rating, ProductStatus? status, string tag)
        {
            // Validate pageNumber and pageSize
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            string searchText = search ?? "";
            var productsQuery = _appDbContext.Products.AsQueryable();

            // Apply search filters
            if (!string.IsNullOrEmpty(searchText))
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(searchText) || p.FullName.Contains(searchText) || p.Categories.Any(c => EF.Functions.Like(c, "%" + searchText + "%")));
            }

            // Apply category filter
            if (!string.IsNullOrEmpty(catg))
            {
                productsQuery = productsQuery.Where(p => p.Categories.Any(c => EF.Functions.Like(c, "%" + catg + "%")));
            }

            // Apply price filters
            if (minPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price <= maxPrice.Value);
            }

            // Apply rating filter
            if (rating.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Rating <= rating.Value);
            }

            // Apply status filter
            if (status.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Status == status.Value);
            }

            // Apply tag filter
            if (!string.IsNullOrEmpty(tag))
            {
                productsQuery = productsQuery.Where(p => p.Tags.Any(tg => EF.Functions.Like(tg, "%" + tag + "%")));
            }

            // Get the total count of products before pagination
            var totalItems = await productsQuery.CountAsync();

            // Calculate total pages
            var totalPage = (int)Math.Ceiling((double)totalItems / pageSize);

            // Adjust pageNumber to stay within bounds
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPage));

            // Apply pagination (skip and take)
            var skip = (pageNumber - 1) * pageSize;
            var paginatedProducts = await productsQuery
                .Skip(skip)
                .Take(pageSize)
                .Select(p => _mapper.Map<ProductReadDto>(p))
                .ToListAsync();

            // Build the paginated response
            var pager = new PaginatedResult<ProductReadDto>
            {
                Items = paginatedProducts,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
                StartPage = Math.Max(1, pageNumber - 2),
                EndPage = Math.Min(totalPage, pageNumber + 2)
            };

            return pager;
        }




        public ProductReadDto GetProductsByIdService(Guid Id)
        {
            var result = _appDbContext.Products.FirstOrDefault(prod => prod.Id == Id);

            return result == null ? null : _mapper.Map<ProductReadDto>(result);
        }

        public async Task<bool> DeleteProductByIdService(Guid id)
        {

            var result =  _appDbContext.Products.FirstOrDefault(prod => prod.Id == id);
            if (result == null)
            {
                return false;
            }

            var deleteTasks = result.Images.Select(imageUrl => _filestService.DeleteImageAsync(imageUrl));
            var deleteResults = await Task.WhenAll(deleteTasks);


            if (deleteResults.All(success => success))
            {
                _appDbContext.Products.Remove(result);
                await _appDbContext.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task<ProductReadDto> CreateProductService(ProductCreateDto productData)
        {
            var newProduct = _mapper.Map<ProductModel>(productData);
            var images = new List<string>();
            newProduct.Id = Guid.NewGuid();
            newProduct.UpdatedAt = DateTime.UtcNow;
            newProduct.CreatedAt = DateTime.UtcNow;
            newProduct.Date = DateTime.UtcNow;
            newProduct.CurrencySymbol = CurrencyService.GetCurrencySymbolByCode(productData.Currency);
            newProduct.Currency = CurrencyService.GetCurrencyCodeBySymbol(newProduct.CurrencySymbol);

            foreach (var image in productData.Images) {
                var result = await _filesService.UploadImageAsync(image);

                if (result != null)
                {
                    var isValid = await  _filesManagementHelper.IsValidImageAsync(result);
                    if (isValid)
                    {
                        images.Add(result);
                    }
                }
            }
            newProduct.Images = images;
            ;
            if (productData.Specifications != null)
            {
                newProduct.Specifications = JsonConvert.SerializeObject(productData.Specifications);
            }
            await _appDbContext.AddAsync(newProduct);
            await _appDbContext.SaveChangesAsync();

            return _mapper.Map<ProductReadDto>(newProduct); ;

        }


        public async Task<PaginatedResult<ProductReadDto>> GetProductsBySearchValueService(string searchData, int pageNumber, int pageSize)
        {
            //var searchResultProducts = _appDbContext.Products.Where(prod => EF.Functions.Like(prod.Name, $"%{searchData}%")).ToList();



            var foundProducts = await _appDbContext.Products.Where(prod => EF.Functions.Like(prod.Name, $"%{searchData}%")).ToListAsync();

            // Validate pageNumber and pageSize
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            // Get the total count of products
            var totalItems = await _appDbContext.Products.Where(prod => EF.Functions.Like(prod.Name, $"%{searchData}%")).CountAsync();

            // Calculate total pages
            var totalPage = (int)Math.Ceiling((double)totalItems / pageSize);

            // Adjust pageNumber to stay within bounds
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPage));

            // Calculate the number of items to skip
            var skip = (pageNumber - 1) * pageSize;

            // Fetch paginated products
            var productList = await _appDbContext.Products.Where(prod => EF.Functions.Like(prod.Name, $"%{searchData}%")).Skip(skip).Take(pageSize).ToListAsync();

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



        public List<ProductReadDto> GetProductsByIdsService(List<Guid> Ids)
        {
            var foundProducts = _appDbContext.Products.Where(prod => Ids.Contains(prod.Id));
            if (foundProducts == null || !foundProducts.Any())
            {
                return null;
            }

            return _mapper.Map<List<ProductReadDto>>(foundProducts);
        }


        public async Task<PaginatedResult<ProductReadDto>> GetProductsByPriceService(int minPrice, int maxPrice, int pageNumber, int pageSize)
        {

            var foundProducts = await _appDbContext.Products.Where(prod => prod.Price >= minPrice && prod.Price <= maxPrice).ToListAsync();

            // Validate pageNumber and pageSize
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            // Get the total count of products
            var totalItems = await _appDbContext.Products.Where(prod => prod.Price >= minPrice && prod.Price <= maxPrice).CountAsync();

            // Calculate total pages
            var totalPage = (int)Math.Ceiling((double)totalItems / pageSize);

            // Adjust pageNumber to stay within bounds
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPage));

            // Calculate the number of items to skip
            var skip = (pageNumber - 1) * pageSize;

            // Fetch paginated products
            var productList = await _appDbContext.Products.Where(prod => prod.Price >= minPrice && prod.Price <= maxPrice).Skip(skip).Take(pageSize).ToListAsync();

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



        public async Task<PaginatedResult<ProductReadDto>> GetProductByRatingServiceAsync(double rating, int pageNumber, int pageSize)
        {
            double tolerance = 0.9;

            // Use asynchronous database querying
            var foundProducts = await _appDbContext.Products
                .Where(prod => (Convert.ToDouble(prod.Rating) >= (rating - tolerance))
                            && (Convert.ToDouble(prod.Rating) <= (rating + tolerance)))
                .ToListAsync();



            // Validate pageNumber and pageSize
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            // Get the total count of products
            var totalItems = await _appDbContext.Products
                .Where(prod => (Convert.ToDouble(prod.Rating) >= (rating - tolerance))
                            && (Convert.ToDouble(prod.Rating) <= (rating + tolerance))).CountAsync();

            // Calculate total pages
            var totalPage = (int)Math.Ceiling((double)totalItems / pageSize);

            // Adjust pageNumber to stay within bounds
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPage));

            // Calculate the number of items to skip
            var skip = (pageNumber - 1) * pageSize;

            // Fetch paginated products
            var productList = await _appDbContext.Products.Where(prod => (Convert.ToDouble(prod.Rating) >= (rating - tolerance))
                            && (Convert.ToDouble(prod.Rating) <= (rating + tolerance))).Skip(skip).Take(pageSize).ToListAsync();

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

        public async Task<PaginatedResult<ProductReadDto>> GetProductByStatusServiceAsync(int status, int pageNumber, int pageSize)
        {
            // Validate pageNumber and pageSize
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            // Get the total count of products
            var totalItems = await  _appDbContext.Products.Where(prod => (int)prod.Status == status).CountAsync();

            // Calculate total pages
            var totalPage = (int)Math.Ceiling((double)totalItems / pageSize);

            // Adjust pageNumber to stay within bounds
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPage));

            // Calculate the number of items to skip
            var skip = (pageNumber - 1) * pageSize;

            // Fetch paginated products
            var productList = await _appDbContext.Products
                                     .Where(prod => (int)prod.Status == status).Skip(skip).Take(pageSize).ToListAsync();

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




    }
}