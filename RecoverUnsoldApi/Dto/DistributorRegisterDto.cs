using System.ComponentModel.DataAnnotations;
using RecoverUnsoldApi.Validation;

namespace RecoverUnsoldApi.Dto;

public record DistributorRegisterDto([Required] [StringLength(100)] [UniqueUsername] string Username,
    [Required] [EmailAddress] [StringLength(100)] [UniqueEmail]
    string Email,
    [Required] string Password, [Required] [RegularExpression(@"^\+[1-9]\d{1,14}$")] string Phone,
    [Required] [StringLength(100)] string Ifu, [Required] [StringLength(100)] string Rccm,
    [StringLength(100)] string? WebsiteUrl);