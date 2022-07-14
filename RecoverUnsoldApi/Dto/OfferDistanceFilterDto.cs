using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Dto;

public record OfferDistanceFilterDto([Required] [Range(0, 90)] double Latitude,
    [Required] [Range(-180, 180)] double Longitude, double Distance, int Page = 1, int PerPage = 10);