using RecoverUnsoldApi.Dto;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldApi.Extensions;

public static class Mapping
{
    public static LocationReadDto ToLocationReadDto(this Location location)
    {
        return new LocationReadDto(location.Id, location.Name,
            new LatLong(location.Coordinates.Y, location.Coordinates.X), location.Indication,
            location.Image,
            location.CreatedAt);
    }

    public static CustomerReadDto ToCustomerReadDto(this Customer customer)
    {
        return new CustomerReadDto(customer.Id, customer.Username, customer.Email, customer.LastName,
            customer.FirstName,
            customer.EmailVerifiedAt, customer.CreatedAt);
    }

    public static DistributorReadDto ToDistributorReadDto(this Distributor distributor)
    {
        return new DistributorReadDto(distributor.Id, distributor.Username, distributor.Email, distributor.Phone,
            distributor.TaxId, distributor.Rccm, distributor.WebsiteUrl, distributor.EmailVerifiedAt,
            distributor.CreatedAt);
    }

    public static DistributorInformationDto ToDistributorInformationDto(this Distributor distributor)
    {
        return new DistributorInformationDto(distributor.Id, distributor.Username, distributor.Email,
            distributor.Phone, distributor.WebsiteUrl, distributor.CreatedAt);
    }

    public static IQueryable<DistributorInformationDto> ToDistributorInformationReadDto(
        this IQueryable<Distributor> distributors)
    {
        return distributors.Select(d => d.ToDistributorInformationDto());
    }

    public static DistributorLabelReadDto ToDistributorLabelReadDto(this Distributor distributor)
    {
        return new DistributorLabelReadDto(distributor.Id, distributor.Username);
    }

    public static ProductReadDto ToProductReadDto(this Product product)
    {
        return new ProductReadDto(product.Id, product.Name, product.Description, product.OfferId,
            product.Images.ToImageReadDto(), product.CreatedAt);
    }

    private static IEnumerable<ProductReadDto> ToProductReadDto(this IEnumerable<Product> products)
    {
        return products.Select(p => p.ToProductReadDto());
    }

    private static ImageReadDto ToImageReadDto(this Image image)
    {
        return new ImageReadDto(image.Id, image.Url);
    }

    private static IEnumerable<ImageReadDto> ToImageReadDto(this IEnumerable<Image> images)
    {
        return images.Select(i => i.ToImageReadDto());
    }

    public static OfferReadDto ToOfferReadDto(this Offer offer)
    {
        return new OfferReadDto(offer.Id, offer.StartDate, offer.Duration, offer.OnlinePayment,
            offer.Beneficiaries, offer.Price, offer.CreatedAt, offer.DistributorId,
            offer.Location?.ToLocationReadDto(), offer.Products.ToProductReadDto());
    }

    public static IQueryable<OfferReadDto> ToOfferReadDto(this IQueryable<Offer> offers)
    {
        return offers.Select(o => o.ToOfferReadDto());
    }

    public static OrderReadDto ToOrderReadDto(this Order order)
    {
        return new OrderReadDto(order.Id, order.WithdrawalDate, order.Customer?.ToCustomerReadDto(),
            order.Payment?.ToPaymentDto(), order.OfferId, order.Offer?.ToOfferReadDto(), order.Status,
            order.Opinions.ToOpinionReadDto(), order.CreatedAt);
    }

    public static IQueryable<OrderReadDto> ToOrderReadDto(this IQueryable<Order> orders)
    {
        return orders.Select(o => o.ToOrderReadDto());
    }

    public static OpinionReadDto ToOpinionReadDto(this Opinion opinion)
    {
        return new OpinionReadDto(opinion.Id, opinion.Comment, opinion.OrderId, opinion.CreatedAt);
    }

    public static IQueryable<OpinionReadDto> ToOpinionReadDto(this IQueryable<Opinion> opinions)
    {
        return opinions.Select(o => o.ToOpinionReadDto());
    }

    private static IEnumerable<OpinionReadDto> ToOpinionReadDto(this IEnumerable<Opinion> opinions)
    {
        return opinions.Select(o => o.ToOpinionReadDto());
    }

    public static ReviewReadDto ToReviewReadDto(this Review review)
    {
        return new ReviewReadDto(review.Comment,
            review.User! is Customer
                ? (review.User as Customer)!.ToCustomerReadDto()
                : (review.User as Distributor)!.ToDistributorReadDto(), review.CreatedAt);
    }

    public static PaymentDto ToPaymentDto(this Payment payment)
    {
        return new PaymentDto(payment.TransactionId, payment.OrderId, payment.CreatedAt);
    }

    public static RepaymentReadDto ToRepaymentReadDto(this Repayment repayment)
    {
        return new RepaymentReadDto(repayment.Id, repayment.Done, repayment.Note, repayment.TransactionId,
            repayment.Order!.ToOrderReadDto(), repayment.CreatedAt);
    }
}