﻿using Azure;
using e_commerce.DTOs.Products;
using e_commerce.Enums;
using e_commerce.Services;
using static e_commerce.Enums.ProductEnums;

namespace e_commerce.Interfaces
{
    public interface IProductService
    {
        Task<PaginatedResult<ProductReadDto>> GetAllProductsService(int currentPage, int pageSize, string search, string catg, decimal? minPrice, decimal? maxPrice, decimal? rating, ProductStatus? status, string tag);

        ProductReadDto GetProductsByIdService(Guid Id);

        Task<bool> DeleteProductByIdService(Guid id);

        Task<ProductReadDto> CreateProductService(ProductCreateDto productData);

        Task<PaginatedResult<ProductReadDto>> GetProductsBySearchValueService(string searchData, int pageNumber, int pageSize);

        List<ProductReadDto> GetProductsByIdsService(List<Guid> Ids);

        Task<PaginatedResult<ProductReadDto>> GetProductsByPriceService(int minPrice, int maxPrice, int pageNumber, int pageSize);

        Task<PaginatedResult<ProductReadDto>> GetProductByRatingServiceAsync(double rating, int pageNumber, int pageSize);

        Task<PaginatedResult<ProductReadDto>> GetProductByStatusServiceAsync(int status, int pageNumber, int pageSize);
    }
}

