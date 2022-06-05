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

    public async Task<bool> IsOwner(Guid userId,Guid locationId)
    {
        return await _context.Locations.AnyAsync(x => x.Id == locationId && x.DistributorId == userId);
    }

    public async Task<UrlPage<LocationReadDto>> GetAll(Guid userId, UrlPaginationParameter urlPaginationParameter)
    {
        return await Task.Run(() => _context.Locations
            .AsNoTracking()
            .Where(x => x.DistributorId == userId)
            .ToLocationReadDto()
            .UrlPaginate(urlPaginationParameter, x => x.CreatedAt)
        );
    }

    public async Task<LocationReadDto?> Get(Guid locationId)
    {
        var location = await _context.Locations
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == locationId);
        return location?.ToLocationReadDto();
    }

    public async Task<UrlPage<LocationReadDto>> FindByName(Guid userId, string search,
        UrlPaginationParameter urlPaginationParameter)
    {
        return await Task.Run(() => _context.Locations
            .AsNoTracking()
            .Where(x => x.DistributorId == userId && EF.Functions.Like(x.Name, $"%{search}%"))
            .ToLocationReadDto()
            .UrlPaginate(urlPaginationParameter, x => x.CreatedAt)
        );
    }

    public async Task<LocationReadDto> Create(Guid userId, LocationCreateDto locationCreateDto)
    {
        string? image = null;
        if (locationCreateDto.Image != null)
        {
            await using var fileStream = locationCreateDto.Image.OpenReadStream();
            var fileName = $"{DateTime.Now.Ticks}{Path.GetExtension(locationCreateDto.Image.FileName)}";
            var imageUploadParameters = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileStream)
            };
            var uploadResult = await _cloudinary.UploadAsync(imageUploadParameters);
            image = uploadResult.Url.ToString();
        }

        var location = _context.Locations.Add(new Location
        {
            Name = locationCreateDto.Name,
            Coordinates = LatLong.FromString(locationCreateDto.Coordinates),
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
            location.Coordinates = LatLong.FromString(locationUpdateDto.Coordinates);
            location.Indication = locationUpdateDto.Indication;
            location.Image = image;
            await _context.SaveChangesAsync();
        }
    }

    public async Task Delete(Guid userId, Guid locationId)
    {
        var location = await _context.Locations
            .FirstOrDefaultAsync(x => x.Id == locationId && x.DistributorId == userId);

        if (location != null)
        {
            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
        }
    }
}