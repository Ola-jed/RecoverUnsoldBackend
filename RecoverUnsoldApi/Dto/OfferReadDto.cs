namespace RecoverUnsoldApi.Dto;

public record OfferReadDto(DateTime StartDate, ulong Duration, int? Beneficiaries, decimal Price,
    LocationReadDto? LocationReadDto, IEnumerable<ProductReadDto>? Products);