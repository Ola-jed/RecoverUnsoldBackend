using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using RecoverUnsoldApi.Dto;

namespace RecoverUnsoldApi.Services.Locations;

public interface ILocationsService
{
    Task<bool> IsOwner(Guid userId, Guid locationId);
    Task<UrlPage<LocationReadDto>> GetAll(Guid userId, UrlPaginationParameter urlPaginationParameter);
    Task<LocationReadDto?> Get(Guid locationId);

    Task<UrlPage<LocationReadDto>>
        FindByName(Guid userId, string search, UrlPaginationParameter urlPaginationParameter);

    Task<LocationReadDto> Create(Guid userId, LocationCreateDto locationCreateDto);
    Task Update(Guid userId, Guid locationId, LocationUpdateDto locationUpdateDto);
    Task Delete(Guid userId, Guid locationId);
}