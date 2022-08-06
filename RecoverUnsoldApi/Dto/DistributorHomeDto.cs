using RecoverUnsoldApi.Entities;

namespace RecoverUnsoldApi.Dto;

public record DistributorHomeDto(Dictionary<DateTime, int> OrdersPerDay, IEnumerable<OfferReadDto> Offers,
    IEnumerable<OrderReadDto> Orders);