using System.ComponentModel.DataAnnotations;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldDomain.Dto;

public record OfferDistanceFilterDto([Required] [Range(0, 90)] double Latitude,
    [Required] [Range(-180, 180)] double Longitude, double Distance, int Page = 1, int PerPage = 10)
{
    public LatLong ToLatLong() => new LatLong(Latitude, Longitude);
}