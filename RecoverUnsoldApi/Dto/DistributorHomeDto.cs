using RecoverUnsoldApi.Entities;

namespace RecoverUnsoldApi.Dto;

public record DistributorHomeDto(Dictionary<DateOnly, int> OrdersPerDay, IEnumerable<OfferReadDto> Offers,
    IEnumerable<OrderReadDto> Orders);