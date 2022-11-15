using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FluentPaginator.Lib.Core;
using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.EntityFrameworkCore;
using RecoverUnsoldDomain.Data;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldDomain.Entities;
using RecoverUnsoldApi.Extensions;
using Point = NetTopologySuite.Geometries.Point;

namespace RecoverUnsoldApi.Services.Locations;

public class LocationsService : ILocationsService
{
    private readonly DataContext _context;
    private readonly Cloudinary _cloudinary;

    public LocationsService(DataContext context, Cloudinary cloudinary)
    {
        _context = context;
        _cloudinary = cloudinary;
    }

    public async Task<bool> IsOwner(Guid userId, Guid locationId)
    {
        return await _context.Locations.AnyAsync(x => x.Id == locationId && x.DistributorId == userId);
    }

    public async Task<UrlPage<LocationReadDto>> GetAll(Guid userId, UrlPaginationParameter urlPaginationParameter)
    {
        return await Task.Run(() => _context.Locations
            .AsNoTracking()
            .Where(x => x.DistributorId == userId)
            .UrlPaginate(urlPaginationParameter, x => x.CreatedAt, PaginationOrder.Descending)
            .ToLocationReadDto()
        );
    }

    public async Task<LocationReadDto?> Get(Guid locationId)
    {
        var location = await _context.Locations
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == locationId);
        return location?.ToLocationReadDto();
    }

    public async Task<UrlPage<LocationReadDto>> FindByName(string search,
        UrlPaginationParameter urlPaginationParameter)
    {
        return await Task.Run(() => _context.Locations
            .AsNoTracking()
            .Where(x => x.Name.Contains(search))
            .UrlPaginate(urlPaginationParameter, x => x.CreatedAt, PaginationOrder.Descending)
            .ToLocationReadDto()
        );
    }

    public async Task<LocationReadDto> Create(Guid userId, LocationCreateDto locationCreateDto)
    {
        string? image = null;
        if (locationCreateDto.Image != null)
        {
            image = (await locationCreateDto.Image.UploadToCloudinary(_cloudinary))
                ?.Url
                ?.ToString();
        }

        var location = _context.Locations.Add(new Location
        {
            Name = locationCreateDto.Name,
            Coordinates = new Point(locationCreateDto.Longitude, locationCreateDto.Latitude),
            Indication = locationCreateDto.Indication,
            Image = image,
            DistributorId = userId
        });
        await _context.SaveChangesAsync();
        return location.Entity.ToLocationReadDto();
    }

    public async Task Update(Guid userId, Guid locationId, LocationUpdateDto locationUpdateDto)
    {
        var location = await _context.Locations
            .FirstOrDefaultAsync(x => x.Id == locationId && x.DistributorId == userId);

        if (location != null)
        {
            var image = location.Image;
            if (locationUpdateDto.Image != null)
            {
                await using var fileStream = locationUpdateDto.Image.OpenReadStream();
                var fileName = $"{DateTime.Now.Ticks}{Path.GetExtension(locationUpdateDto.Image.FileName)}";
                var imageUploadParameters = new ImageUploadParams
                {
                    File = new FileDescription(fileName, fileStream)
                };
                var uploadResult = await _cloudinary.UploadAsync(imageUploadParameters);
                image = uploadResult.Url.ToString();
            }

            location.Name = locationUpdateDto.Name;
            location.Coordinates = new Point(locationUpdateDto.Longitude, locationUpdateDto.Latitude);
            location.Indication = locationUpdateDto.Indication;
            location.Image = image;
            await _context.SaveChangesAsync();
        }
    }

    public async Task Delete(Guid userId, Guid locationId)
    {
        await _context.Locations
            .Where(l => l.Id == locationId && l.DistributorId == userId)
            .ExecuteDeleteAsync();
    }
}