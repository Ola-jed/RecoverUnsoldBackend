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

    public async Task<Page<OfferReadDto>> GetOffers(PaginationParameter paginationParameter, OfferFilterDto offerFilterDto)
    {
        var page = await _context.Offers
            .AsNoTracking()
            .Include(o => o.Location)
            .Include(o => o.Products)
            .ThenInclude(p => p.Images)
            .ApplyFilters(offerFilterDto)
            .AsSplitQuery()
            .AsyncPaginate(paginationParameter, o => o.CreatedAt, PaginationOrder.Descending);
        
        return page.ToOfferReadDto();
    }

    public async Task<Page<OfferReadDto>> GetDistributorOffers(Guid distributorId,
        PaginationParameter paginationParameter, OfferFilterDto offerFilterDto)
    {
        var page = await _context.Offers
            .AsNoTracking()
            .Include(o => o.Location)
            .Include(o => o.Products)
            .ThenInclude(p => p.Images)
            .Where(o => o.DistributorId == distributorId)
            .ApplyFilters(offerFilterDto)
            .AsSplitQuery()
            .AsyncPaginate(paginationParameter, o => o.CreatedAt, PaginationOrder.Descending);
        
        return page.ToOfferReadDto();
    }

    public async Task<Page<OfferWithRelativeDistanceDto>> GetOffersCloseTo(LatLong latLong,
        PaginationParameter paginationParameter, double distance /* In kilometers */)
    {
        var referencePoint = new Point(latLong.Longitude, latLong.Latitude);
        var page = await _context.Offers
            .AsNoTracking()
            .Include(o => o.Location)
            .Include(o => o.Products)
            .ThenInclude(p => p.Images)
            .Where(o => o.Location!.Coordinates.Distance(referencePoint) <= distance * 1000)
            .AsSplitQuery()
            .AsyncPaginate(paginationParameter, o => o.CreatedAt, PaginationOrder.Descending);
        
        return page.Map<Offer, OfferWithRelativeDistanceDto>(o => new OfferWithRelativeDistanceDto(
                o.ToOfferReadDto(),
                o.Location!.Coordinates.Distance(referencePoint) * 100
            ));
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

        var entityEntry = _context.Offers.Add(offer);
        await _context.SaveChangesAsync();
        var products = await Task.WhenAll((offerCreateDto.Products ?? Enumerable.Empty<ProductCreateDto>())
            .Select(async productCreateDto => await _productsService.Create(entityEntry.Entity.Id, productCreateDto))
        );
        return entityEntry.Entity.ToOfferReadDto() with { Products = products };
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