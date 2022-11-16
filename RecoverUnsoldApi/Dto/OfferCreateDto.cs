using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Dto;

public record OfferCreateDto([Required] DateTime StartDate, [Required] ulong Duration, [Required] bool OnlinePayment,
    int? Beneficiaries, [Required] decimal Price, [Required] Guid LocationId,
    IEnumerable<ProductCreateDto>? Products = null);