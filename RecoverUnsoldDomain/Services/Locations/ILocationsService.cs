using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using RecoverUnsoldDomain.Dto;

namespace RecoverUnsoldDomain.Services.Locations;

public interface ILocationsService
{
    Task<bool> IsOwner(Guid userId, Guid locationId);
    Task<UrlPage<LocationReadDto>> GetAll(Guid userId, UrlPaginationParameter urlPaginationParameter);
    Task<LocationReadDto?> Get(Guid locationId);

    Task<UrlPage<LocationReadDto>>
        FindByName(string search, UrlPaginationParameter urlPaginationParameter);

    Task<LocationReadDto> Create(Guid userId, LocationCreateDto locationCreateDto);
    Task Update(Guid userId, Guid locationId, LocationUpdateDto locationUpdateDto);
    Task Delete(Guid userId, Guid locationId);
}