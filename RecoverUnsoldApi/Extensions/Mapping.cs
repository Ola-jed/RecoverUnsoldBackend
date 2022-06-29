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
        return new CustomerReadDto(customer.Id, customer.Username, customer.Email, customer.LastName, customer.FirstName,
            customer.EmailVerifiedAt, customer.CreatedAt);
    }

    public static DistributorReadDto ToDistributorReadDto(this Distributor distributor)
    {
        return new DistributorReadDto(distributor.Id, distributor.Username, distributor.Email, distributor.Phone,
            distributor.TaxId, distributor.Rccm, distributor.WebsiteUrl, distributor.EmailVerifiedAt,
            distributor.CreatedAt);
    }

    public static ProductReadDto ToProductReadDto(this Product product)
    {
        return new ProductReadDto(product.Id, product.Name, product.Description, product.OfferId,
            product.Images.ToImageReadDto(), product.CreatedAt);
    }

    public static IQueryable<ProductReadDto> ToProductReadDto(this IQueryable<Product> products)
    {
        return products.Select(p => p.ToProductReadDto());
    }

    public static IEnumerable<ProductReadDto> ToProductReadDto(this IEnumerable<Product> products)
    {
        return products.Select(p => p.ToProductReadDto());
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

    public static OfferReadDto ToOfferReadDto(this Offer offer)
    {
        return new OfferReadDto(offer.Id,offer.StartDate, offer.Duration, offer.Beneficiaries, offer.Price,
            offer.CreatedAt, offer.Location?.ToLocationReadDto(), offer.Products.ToProductReadDto());
    }

    public static IQueryable<OfferReadDto> ToOfferReadDto(this IQueryable<Offer> offers)
    {
        return offers.Select(o => o.ToOfferReadDto());
    }
}