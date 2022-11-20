using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using RecoverUnsoldApi.Dto;

namespace RecoverUnsoldApi.Services.Products;

public interface IProductsService
{
    Task<bool> IsOwner(Guid id, Guid distributorId);
    Task<ProductReadDto?> GetProduct(Guid id);
    Task<Page<ProductReadDto>> GetProducts(PaginationParameter paginationParameter);

    Task<Page<ProductReadDto>> GetDistributorProducts(Guid distributorId, PaginationParameter paginationParameter);

    Task<Page<ProductReadDto>> GetOfferProducts(Guid offerId, PaginationParameter paginationParameter);
    Task<ProductReadDto> Create(Guid offerId, ProductCreateDto productCreateDto);
    Task Update(Guid id, Guid distributorId, ProductUpdateDto productUpdateDto);
    Task Delete(Guid id, Guid distributorId);
}