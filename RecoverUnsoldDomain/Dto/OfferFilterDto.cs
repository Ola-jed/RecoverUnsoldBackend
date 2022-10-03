namespace RecoverUnsoldDomain.Dto;

public record OfferFilterDto(int Page = 1, int PerPage = 10, decimal? MinPrice = null, decimal? MaxPrice = null,
    DateTime? MinDate = null, DateTime? MaxDate = null, bool? Active = null);