using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using RecoverUnsoldApi.Dto;

namespace RecoverUnsoldApi.Services.Offers;

public interface IOffersService
{
    Task<UrlPage<OfferReadDto>> GetOffers(UrlPaginationParameter urlPaginationParameter);
    Task<UrlPage<OfferReadDto>> GetDistributorOffers(Guid distributorId, UrlPaginationParameter urlPaginationParameter);
    Task<OfferReadDto?> GetOffer(Guid id, Guid distributorId);
    Task<OfferReadDto> Create(Guid distributorId, OfferCreateDto offerCreateDto);
    Task Update(Guid id, Guid distributorId, OfferUpdateDto offerUpdateDto);
    Task Delete(Guid id, Guid distributorId);
}