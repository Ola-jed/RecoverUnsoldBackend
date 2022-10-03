using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldDomain.Dto;

public record LocationReadDto(Guid Id, string Name, LatLong Coordinates, string? Indication, string? Image,
    DateTime CreatedAt);