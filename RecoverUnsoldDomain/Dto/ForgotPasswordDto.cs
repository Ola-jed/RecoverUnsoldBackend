using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldDomain.Dto;

public record ForgotPasswordDto([Required] [EmailAddress] string Email);