using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.EntityFrameworkCore;
using RecoverUnsoldApi.Data;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Entities;
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

    public async Task<UrlPage<OfferReadDto>> GetOffers(UrlPaginationParameter urlPaginationParameter,
        OfferFilterDto offerFilterDto)
    {
        return await Task.Run(() => _context.Offers
            .AsNoTracking()
            .Include(o => o.Location)
            .Include(o => o.Products)
            .ThenInclude(p => p.Images)
            .ApplyFilters(offerFilterDto)
            .ToOfferReadDto()
            .UrlPaginate(urlPaginationParameter, o => o.CreatedAt)
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
            .ToOfferReadDto()
            .UrlPaginate(urlPaginationParameter, o => o.CreatedAt)
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
        var offer = await _context.Offers.FindAsync(id);
        if (offer == null || offer.DistributorId != distributorId)
        {
            return;
        }

        offer.StartDate = offerUpdateDto.StartDate;
        offer.Duration = offerUpdateDto.Duration;
        offer.Beneficiaries = offerUpdateDto.Beneficiaries;
        offer.Price = offerUpdateDto.Price;
        offer.LocationId = offerUpdateDto.LocationId;
        _context.Offers.Update(offer);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid id, Guid distributorId)
    {
        var offer = await _context.Offers.FindAsync(id);
        if (offer == null || offer.DistributorId != distributorId)
        {
            return;
        }

        _context.Offers.Remove(offer);
        await _context.SaveChangesAsync();
    }
}