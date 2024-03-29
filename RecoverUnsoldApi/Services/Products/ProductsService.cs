﻿using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FluentPaginator.Lib.Core;
using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.EntityFrameworkCore;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Extensions;
using RecoverUnsoldDomain.Data;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldApi.Services.Products;

public class ProductsService : IProductsService
{
    private readonly DataContext _context;
    private readonly Cloudinary _cloudinary;

    public ProductsService(DataContext context, Cloudinary cloudinary)
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

    public async Task<Page<ProductReadDto>> GetOfferProducts(Guid offerId,
        PaginationParameter paginationParameter)
    {
        var page = await _context.Products
            .AsNoTracking()
            .Include(p => p.Images)
            .Where(p => p.OfferId == offerId)
            .AsyncPaginate(paginationParameter, p => p.CreatedAt, PaginationOrder.Descending);

        return page.Map(p => p.ToProductReadDto());
    }

    public async Task<Page<ProductReadDto>> GetProducts(PaginationParameter paginationParameter)
    {
        var page = await _context.Products
            .AsNoTracking()
            .Include(p => p.Images)
            .AsyncPaginate(paginationParameter, p => p.CreatedAt, PaginationOrder.Descending);

        return page.Map(p => p.ToProductReadDto());
    }

    public async Task<Page<ProductReadDto>> GetDistributorProducts(Guid distributorId,
        PaginationParameter paginationParameter)
    {
        var page = await _context.Products
            .AsNoTracking()
            .Include(p => p.Images)
            .Include(p => p.Offer)
            .Where(p => p.Offer != null && p.Offer.DistributorId == distributorId)
            .AsyncPaginate(paginationParameter, p => p.CreatedAt, PaginationOrder.Descending);

        return page.Map(p => p.ToProductReadDto());
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
        await _context.Products
            .Include(p => p.Offer)
            .Include(p => p.Images)
            .Where(p => p.Id == id && p.Offer!.DistributorId != distributorId)
            .ExecuteUpdateAsync(product => product.SetProperty(x => x.Name, productUpdateDto.Name)
                .SetProperty(x => x.Description, productUpdateDto.Description)
            );
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
        await _context.SaveChangesAsync();
    }
}