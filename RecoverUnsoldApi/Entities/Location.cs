using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace RecoverUnsoldApi.Entities;

public class Location : Entity
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    public LatLong Coordinates { get; set; } = null!;

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
        return $"{Latitude},{Longitude}";
    }

    public static LatLong FromString(string s)
    {
        var coordinates = s.Split(",");
        Debug.Assert(coordinates.Length == 2, "Only lat and long");
        return new LatLong
        {
            Latitude = double.Parse(coordinates.First()),
            Longitude = double.Parse(coordinates.Last())
        };
    }
}