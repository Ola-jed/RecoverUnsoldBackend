using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Dto;

public record PasswordResetDto([Required] string Token, [Required] string Password);