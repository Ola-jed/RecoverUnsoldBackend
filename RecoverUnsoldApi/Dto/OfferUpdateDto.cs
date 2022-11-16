using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Dto;

public record OfferUpdateDto([Required] DateTime StartDate, [Required] ulong Duration,[Required] bool OnlinePayment,
    int? Beneficiaries, [Required] decimal Price, [Required] Guid LocationId);