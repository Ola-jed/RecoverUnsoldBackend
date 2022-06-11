using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using RecoverUnsoldApi.Dto;

namespace RecoverUnsoldApi.Services.Products;

public interface IProductService
{
    Task<UrlPage<ProductReadDto>> GetOfferProducts(Guid offerId, UrlPaginationParameter urlPaginationParameter);
    Task<ProductReadDto?> GetProduct(Guid distributorId, Guid id);
    Task<ProductReadDto> Create(Guid offerId, ProductCreateDto productCreateDto);
    Task Update(Guid id, Guid distributorId, ProductUpdateDto productUpdateDto);
    Task Delete(Guid id, Guid distributorId);
}