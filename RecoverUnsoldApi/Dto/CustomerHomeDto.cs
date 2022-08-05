namespace RecoverUnsoldApi.Dto;

public record CustomerHomeDto(IEnumerable<OfferReadDto> Offers, IEnumerable<DistributorInformationDto> Distributors);