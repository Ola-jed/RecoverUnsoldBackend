using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldDomain.Dto;

public record OfferUpdateDto([Required] DateTime StartDate, [Required] ulong Duration, int? Beneficiaries,
    [Required] decimal Price, [Required] Guid LocationId);