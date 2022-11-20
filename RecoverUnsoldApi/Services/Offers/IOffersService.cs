using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldApi.Services.Offers;

public interface IOffersService
{
    Task<bool> IsOwner(Guid distributorId, Guid id);
    Task<bool> Exists(Guid id);
    Task<Page<OfferReadDto>> GetOffers(PaginationParameter paginationParameter, OfferFilterDto offerFilterDto);
    Task<Page<OfferReadDto>> GetDistributorOffers(Guid distributorId, PaginationParameter paginationParameter, OfferFilterDto offerFilterDto);
    Task<Page<OfferWithRelativeDistanceDto>> GetOffersCloseTo(LatLong latLong, PaginationParameter paginationParameter, double distance);
    Task<OfferReadDto?> GetOffer(Guid id);
    Task<OfferReadDto> Create(Guid distributorId, OfferCreateDto offerCreateDto);
    Task Update(Guid id, Guid distributorId, OfferUpdateDto offerUpdateDto);
    Task Delete(Guid id, Guid distributorId);
}