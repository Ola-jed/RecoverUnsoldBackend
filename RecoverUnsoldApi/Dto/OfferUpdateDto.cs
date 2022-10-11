using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Dto;

public record OfferUpdateDto([Required] DateTime StartDate, [Required] ulong Duration, int? Beneficiaries,
    [Required] decimal Price, [Required] Guid LocationId);