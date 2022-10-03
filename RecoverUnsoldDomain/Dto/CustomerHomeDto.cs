namespace RecoverUnsoldDomain.Dto;

public record CustomerHomeDto(IEnumerable<OfferReadDto> Offers, IEnumerable<DistributorInformationDto> Distributors,
    CustomerOrderStatsDto? OrderStats);