using RecoverUnsoldApi.Entities;

namespace RecoverUnsoldApi.Extensions;

public static class CalculusExtensions
{
    private const double EarthRadius = 6371;

    public static double GetDistanceWith(this LatLong latLong1, LatLong latLong2)
    {
        var latDifference = (latLong2.Latitude - latLong1.Latitude).DegToRadians();
        var longDifference = (latLong2.Longitude - latLong1.Longitude).DegToRadians();
        var a = Math.Sin(latDifference / 2) * Math.Sin(latDifference / 2) +
                Math.Sin(longDifference / 2) * Math.Sin(longDifference / 2) *
                Math.Cos(latLong1.Latitude.DegToRadians()) * Math.Cos(latLong2.Latitude.DegToRadians());
        return EarthRadius * 2 * Math.Asin(Math.Sqrt(a));
    }

    public static double DegToRadians(this double angle)
    {
        return Math.PI / 180 * angle;
    }
}