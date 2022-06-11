using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using RecoverUnsoldApi.Dto;

namespace RecoverUnsoldApi.Services.Offers;

public class OffersService: IOffersService
{
    public async Task<UrlPage<OfferReadDto>> GetOffers(UrlPaginationParameter urlPaginationParameter)
    {
        throw new NotImplementedException();
    }

    public async Task<UrlPage<OfferReadDto>> GetDistributorOffers(Guid distributorId, UrlPaginationParameter urlPaginationParameter)
    {
        throw new NotImplementedException();
    }

    public async Task<OfferReadDto?> GetOffer(Guid id, Guid distributorId)
    {
        throw new NotImplementedException();
    }

    public async Task<OfferReadDto> Create(Guid distributorId, OfferCreateDto offerCreateDto)
    {
        throw new NotImplementedException();
    }

    public async Task Update(Guid id, Guid distributorId, OfferUpdateDto offerUpdateDto)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(Guid id, Guid distributorId)
    {
        throw new NotImplementedException();
    }
}