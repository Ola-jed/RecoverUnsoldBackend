using RecoverUnsoldApi.Entities;
using RecoverUnsoldApi.Extensions;

namespace RecoverUnsoldApi.Tests;

public class LatLongTest
{
    [Fact]
    public void TestDistanceZeroForSameLocation()
    {
        var latLong1 = new LatLong(0, 0);
        var latLong2 = new LatLong(0, 0);
        Assert.Equal(0, latLong1.GetDistanceWith(latLong2));
    }

    [Fact]
    public void TestDistanceWithKnownLocations()
    {
        var latLong1 = new LatLong(41.507483, -99.436554);
        var latLong2 = new LatLong(38.504048, -98.315949);
        Assert.Equal(347.3, latLong1.GetDistanceWith(latLong2), 1);
    }
}