using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldDomain.Dto;

public record PasswordResetDto([Required] string Token, [Required] string Password);