namespace RecoverUnsoldApi.Dto;

public record OfferReadDto(DateTime StartDate, ulong Duration, int? Beneficiaries, decimal Price, DateTime CreatedAt,
    LocationReadDto? LocationReadDto, IEnumerable<ProductReadDto>? Products);