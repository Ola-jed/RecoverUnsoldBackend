using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Dto;

public record DistributorRegisterDto([Required] [StringLength(100)] string UserName,
    [Required] [EmailAddress] [StringLength(100)] string Email,
    [Required] string Password, [Required] [RegularExpression(@"^\+[1-9]\d{1,14}$")] string Phone,
    [Required] [StringLength(100)] string Ifu, [Required] [StringLength(100)] string Rccm, string? WebsiteUrl);