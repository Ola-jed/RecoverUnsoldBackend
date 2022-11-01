using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace RecoverUnsoldDomain.Entities;

public class Location : Entity
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [Column(TypeName="geography")]
    // X : Long
    // Y : Lat
    // Distance is in meters so divide by 1000
    public Point Coordinates { get; set; } = null!;

    public string? Indication { get; set; }
    public string? Image { get; set; }

    [ForeignKey(nameof(Distributor))]
    public Guid DistributorId { get; set; }

    public Distributor? Distributor { get; set; }

    public ICollection<Offer> Offers { get; set; } = new HashSet<Offer>();
}

public record LatLong
{
    public LatLong() { }

    public LatLong(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public double Latitude { get; init; }
    public double Longitude { get; init; }

    public override string ToString()
    {
        return $"{Latitude} {Longitude}";
    }
}