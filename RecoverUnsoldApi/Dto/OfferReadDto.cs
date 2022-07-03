namespace RecoverUnsoldApi.Dto;

public record OfferReadDto(Guid Id,DateTime StartDate, ulong Duration, int? Beneficiaries, decimal Price, DateTime CreatedAt,
    LocationReadDto? Location, IEnumerable<ProductReadDto>? Products);