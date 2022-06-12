using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using RecoverUnsoldApi.Dto;

namespace RecoverUnsoldApi.Services.Offers;

public interface IOffersService
{
    Task<bool> IsOwner(Guid distributorId, Guid id);
    Task<UrlPage<OfferReadDto>> GetOffers(UrlPaginationParameter urlPaginationParameter, OfferFilterDto offerFilterDto);

    Task<UrlPage<OfferReadDto>> GetDistributorOffers(Guid distributorId, UrlPaginationParameter urlPaginationParameter,
        OfferFilterDto offerFilterDto);

    Task<OfferReadDto?> GetOffer(Guid id);
    Task<OfferReadDto> Create(Guid distributorId, OfferCreateDto offerCreateDto);
    Task Update(Guid id, Guid distributorId, OfferUpdateDto offerUpdateDto);
    Task Delete(Guid id, Guid distributorId);
}