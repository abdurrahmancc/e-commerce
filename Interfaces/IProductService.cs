using e_commerce.DTOs.Products;

namespace e_commerce.Interfaces
{
    public interface IProductService
    {
        Task<PaginatedResult<ProductReadDto>> GetAllProductsService(int currentPage, int pageSize);

        ProductReadDto GetProductsByIdService(Guid Id);

        Task<ProductReadDto> CreateProductService(ProductCreateDto productData);

        List<ProductReadDto> GetProductsBySearchValueService(string searchData);

        List<ProductReadDto> GetProductsByIdsService(List<Guid> Ids);

        List<ProductReadDto> GetProductsByPriceService(int minPrice, int maxPrice);
        Task<List<ProductReadDto>> GetProductByRatingServiceAsync(double rating);
    }
}

