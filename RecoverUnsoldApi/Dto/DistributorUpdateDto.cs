using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Dto;

public record DistributorUpdateDto([Required] [StringLength(100)] string Username,
    [Required] [RegularExpression(@"^\+[1-9]\d{1,14}$")]
    string Phone,
    [Required] [StringLength(100)] string TaxId, [Required] [StringLength(100)] string Rccm,
    [StringLength(100)] string? WebsiteUrl);