using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using RecoverUnsoldDomain.Dto;

namespace RecoverUnsoldDomain.Services.Products;

public interface IProductsService
{
    Task<bool> IsOwner(Guid id, Guid distributorId);
    Task<ProductReadDto?> GetProduct(Guid id);
    Task<UrlPage<ProductReadDto>> GetProducts(UrlPaginationParameter urlPaginationParameter);

    Task<UrlPage<ProductReadDto>> GetDistributorProducts(Guid distributorId,
        UrlPaginationParameter urlPaginationParameter);

    Task<UrlPage<ProductReadDto>> GetOfferProducts(Guid offerId, UrlPaginationParameter urlPaginationParameter);
    Task<ProductReadDto> Create(Guid offerId, ProductCreateDto productCreateDto);
    Task Update(Guid id, Guid distributorId, ProductUpdateDto productUpdateDto);
    Task Delete(Guid id, Guid distributorId);
}