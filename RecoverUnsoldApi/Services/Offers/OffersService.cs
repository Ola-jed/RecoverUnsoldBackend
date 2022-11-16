using FluentPaginator.Lib.Core;
using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using RecoverUnsoldDomain.Data;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldDomain.Entities;
using RecoverUnsoldApi.Extensions;
using RecoverUnsoldApi.Services.Products;

namespace RecoverUnsoldApi.Services.Offers;

public class OffersService : IOffersService
{
    private readonly DataContext _context;
    private readonly IProductsService _productsService;

    public OffersService(DataContext context, IProductsService productsService)
    {
        _context = context;
        _productsService = productsService;
    }

    public async Task<bool> IsOwner(Guid distributorId, Guid id)
    {
        return await _context.Offers.AnyAsync(x => x.DistributorId == distributorId && x.Id == id);
    }

    public async Task<bool> Exists(Guid id)
    {
        return await _context.Offers.AnyAsync(x => x.Id == id);
    }

    public async Task<UrlPage<OfferReadDto>> GetOffers(UrlPaginationParameter urlPaginationParameter,
        OfferFilterDto offerFilterDto)
    {
        return await Task.Run(() => _context.Offers
            .AsNoTracking()
            .Include(o => o.Location)
            .Include(o => o.Products)
            .ThenInclude(p => p.Images)
            .ApplyFilters(offerFilterDto)
            .UrlPaginate(urlPaginationParameter, o => o.CreatedAt, PaginationOrder.Descending)
            .ToOfferReadDto()
        );
    }

    public async Task<UrlPage<OfferReadDto>> GetDistributorOffers(Guid distributorId,
        UrlPaginationParameter urlPaginationParameter, OfferFilterDto offerFilterDto)
    {
        return await Task.Run(() => _context.Offers
            .AsNoTracking()
            .Include(o => o.Location)
            .Include(o => o.Products)
            .ThenInclude(p => p.Images)
            .Where(o => o.DistributorId == distributorId)
            .ApplyFilters(offerFilterDto)
            .UrlPaginate(urlPaginationParameter, o => o.CreatedAt, PaginationOrder.Descending)
            .ToOfferReadDto()
        );
    }

    public async Task<UrlPage<OfferWithRelativeDistanceDto>> GetOffersCloseTo(LatLong latLong,
        UrlPaginationParameter urlPaginationParameter, double distance)
    {
        var referencePoint = new Point(latLong.Longitude, latLong.Latitude);
        return await Task.Run(() => _context.Offers
            .AsNoTracking()
            .Include(o => o.Location)
            .Include(o => o.Products)
            .ThenInclude(p => p.Images)
            .Where(o => o.Location!.Coordinates.Distance(referencePoint) <= distance * 1000)
            .Select(o => new OfferWithRelativeDistanceDto(
                o.ToOfferReadDto(),
                o.Location!.Coordinates.Distance(referencePoint)
            ))
            .UrlPaginate(urlPaginationParameter, o => o.Offer.CreatedAt, PaginationOrder.Descending)
        );
    }

    public async Task<OfferReadDto?> GetOffer(Guid id)
    {
        var offer = await _context.Offers
            .AsNoTracking()
            .Include(o => o.Location)
            .Include(o => o.Products)
            .ThenInclude(p => p.Images)
            .FirstOrDefaultAsync(o => o.Id == id);
        return offer?.ToOfferReadDto();
    }

    public async Task<OfferReadDto> Create(Guid distributorId, OfferCreateDto offerCreateDto)
    {
        var offer = new Offer
        {
            StartDate = offerCreateDto.StartDate,
            Duration = offerCreateDto.Duration,
            OnlinePayment = offerCreateDto.OnlinePayment,
            Beneficiaries = offerCreateDto.Beneficiaries,
            Price = offerCreateDto.Price,
            LocationId = offerCreateDto.LocationId,
            DistributorId = distributorId
        };

        var offerEntityEntry = _context.Offers.Add(offer);
        await _context.SaveChangesAsync();

        var products = await Task.WhenAll((offerCreateDto.Products ?? Enumerable.Empty<ProductCreateDto>())
            .Select(async productCreateDto =>
                await _productsService.Create(offerEntityEntry.Entity.Id, productCreateDto))
        );

        return offerEntityEntry.Entity.ToOfferReadDto() with { Products = products };
    }

    public async Task Update(Guid id, Guid distributorId, OfferUpdateDto offerUpdateDto)
    {
        await _context.Offers
            .Where(o => o.Id == id && o.DistributorId == distributorId)
            .ExecuteUpdateAsync(offer => offer.SetProperty(x => x.StartDate, offerUpdateDto.StartDate)
                .SetProperty(x => x.Duration, offerUpdateDto.Duration)
                .SetProperty(x => x.OnlinePayment, offerUpdateDto.OnlinePayment)
                .SetProperty(x => x.Beneficiaries, offerUpdateDto.Beneficiaries)
                .SetProperty(x => x.Price, offerUpdateDto.Price)
                .SetProperty(x => x.LocationId, offerUpdateDto.LocationId)
            );
    }

    public async Task Delete(Guid id, Guid distributorId)
    {
        await _context.Offers
            .Where(o => o.Id == id && o.DistributorId == distributorId)
            .ExecuteDeleteAsync();
    }
}