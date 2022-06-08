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

    public static CustomerReadDto ToCustomerReadDto(this Customer customer)
    {
        return new CustomerReadDto(customer.Username, customer.Email, customer.LastName, customer.FirstName,
            customer.EmailVerifiedAt, customer.CreatedAt);
    }

    public static DistributorReadDto ToDistributorReadDto(this Distributor distributor)
    {
        return new DistributorReadDto(distributor.Username, distributor.Email, distributor.Phone,
            distributor.Ifu, distributor.Rccm, distributor.WebsiteUrl, distributor.EmailVerifiedAt,
            distributor.CreatedAt);
    }
}