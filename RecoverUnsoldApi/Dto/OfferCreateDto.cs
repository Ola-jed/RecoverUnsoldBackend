using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Dto;

public record OfferCreateDto([Required] DateTime StartDate, [Required] ulong Duration, int? Beneficiaries,
    [Required] decimal Price, [Required] Guid LocationId, IEnumerable<ProductCreateDto>? Products = null);