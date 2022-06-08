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

    public static ProductReadDto ToProductReadDto(this Product product)
    {
        return new ProductReadDto(product.Id, product.Name, product.Description, product.OfferId,
            product.Images.ToImageReadDto());
    }

    public static ImageReadDto ToImageReadDto(this Image image)
    {
        return new ImageReadDto(image.Id, image.Url);
    }

    public static IQueryable<ImageReadDto> ToImageReadDto(this IQueryable<Image> images)
    {
        return images.Select(i => i.ToImageReadDto());
    }

    public static IEnumerable<ImageReadDto> ToImageReadDto(this IEnumerable<Image> images)
    {
        return images.Select(i => i.ToImageReadDto());
    }
}