namespace RecoverUnsoldDomain.Dto;

public record OfferReadDto(Guid Id, DateTime StartDate, ulong Duration, int? Beneficiaries, decimal Price,
    DateTime CreatedAt, Guid DistributorId, LocationReadDto? Location, IEnumerable<ProductReadDto>? Products);