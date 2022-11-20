using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using RecoverUnsoldApi.Dto;

namespace RecoverUnsoldApi.Services.Locations;

public interface ILocationsService
{
    Task<bool> IsOwner(Guid userId, Guid locationId);
    Task<Page<LocationReadDto>> GetAll(Guid userId, PaginationParameter paginationParameter);
    Task<LocationReadDto?> Get(Guid locationId);
    Task<Page<LocationReadDto>> FindByName(string search, PaginationParameter paginationParameter);
    Task<LocationReadDto> Create(Guid userId, LocationCreateDto locationCreateDto);
    Task Update(Guid userId, Guid locationId, LocationUpdateDto locationUpdateDto);
    Task Delete(Guid userId, Guid locationId);
}