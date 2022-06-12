using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.EntityFrameworkCore;
using RecoverUnsoldApi.Data;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Entities;
using RecoverUnsoldApi.Extensions;

namespace RecoverUnsoldApi.Services.Products;

public class ProductsesService : IProductsService
{
    private readonly DataContext _context;
    private readonly Cloudinary _cloudinary;

    public ProductsesService(DataContext context, Cloudinary cloudinary)
    {
        _context = context;
        _cloudinary = cloudinary;
    }

    public async Task<bool> IsOwner(Guid id, Guid distributorId)
    {
        return await _context.Products
            .AsNoTracking()
            .Include(p => p.Images)
            .Include(p => p.Offer)
            .AnyAsync(p => p.Offer != null && p.Offer.DistributorId == distributorId);
    }

    public async Task<UrlPage<ProductReadDto>> GetOfferProducts(Guid offerId,
        UrlPaginationParameter urlPaginationParameter)
    {
        return await Task.Run(() => _context.Products
            .AsNoTracking()
            .Include(p => p.Images)
            .Where(p => p.OfferId == offerId)
            .ToProductReadDto()
            .UrlPaginate(urlPaginationParameter, p => p.CreatedAt));
    }

    public async Task<UrlPage<ProductReadDto>> GetProducts(UrlPaginationParameter urlPaginationParameter)
    {
        return await Task.Run(() => _context.Products
            .AsNoTracking()
            .Include(p => p.Images)
            .ToProductReadDto()
            .UrlPaginate(urlPaginationParameter, p => p.CreatedAt));
    }

    public async Task<UrlPage<ProductReadDto>> GetDistributorProducts(Guid distributorId,
        UrlPaginationParameter urlPaginationParameter)
    {
        return await Task.Run(() => _context.Products
            .AsNoTracking()
            .Include(p => p.Images)
            .Include(p => p.Offer)
            .Where(p => p.Offer != null && p.Offer.DistributorId == distributorId)
            .ToProductReadDto()
            .UrlPaginate(urlPaginationParameter, p => p.CreatedAt));
    }

    public async Task<ProductReadDto?> GetProduct(Guid id)
    {
        var product = await _context.Products
            .AsNoTracking()
            .Include(p => p.Images)
            .Include(p => p.Offer)
            .FirstOrDefaultAsync(p => p.Id == id);
        return product?.ToProductReadDto();
    }

    public async Task<ProductReadDto> Create(Guid offerId, ProductCreateDto productCreateDto)
    {
        var images = productCreateDto.Images ?? Enumerable.Empty<IFormFile>();
        var imageModels = await Task.WhenAll(images.Select(async i =>
        {
            var uploadResult = (await i.UploadToCloudinary(_cloudinary))!;
            return new Image
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.Url.ToString()
            };
        }));

        var productEntityEntry = _context.Products.Add(new Product
        {
            Name = productCreateDto.Name,
            Description = productCreateDto.Description,
            OfferId = offerId,
            Images = imageModels
        });

        await _context.SaveChangesAsync();
        return productEntityEntry.Entity.ToProductReadDto();
    }

    public async Task Update(Guid id, Guid distributorId, ProductUpdateDto productUpdateDto)
    {
        var product = await _context.Products
            .Include(p => p.Offer)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (product == null || product.Offer?.DistributorId != distributorId)
        {
            return;
        }

        product.Name = productUpdateDto.Name;
        product.Description = productUpdateDto.Description;
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid id, Guid distributorId)
    {
        var product = await _context.Products
            .Include(p => p.Offer)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (product == null || product.Offer?.DistributorId != distributorId)
        {
            return;
        }

        foreach (var productImage in product.Images)
        {
            await _cloudinary.DestroyAsync(new DeletionParams(productImage.PublicId));
        }

        _context.Products.Remove(product);
    }
}