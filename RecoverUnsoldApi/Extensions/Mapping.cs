using RecoverUnsoldApi.Dto;
using RecoverUnsoldApi.Entities;

namespace RecoverUnsoldApi.Extensions;

public static class Mapping
{
    public static LocationReadDto ToLocationReadDto(this Location location)
    {
        return new LocationReadDto(location.Id, location.Name, location.Coordinates, location.Indication,
            location.Image,
            location.CreatedAt);
    }

    public static IQueryable<LocationReadDto> ToLocationReadDto(this IQueryable<Location> locations)
    {
        return locations.Select(l => l.ToLocationReadDto());
    }
}