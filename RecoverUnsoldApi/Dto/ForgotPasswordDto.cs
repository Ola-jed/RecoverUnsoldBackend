using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Dto;

public record ForgotPasswordDto([Required][EmailAddress] string Email);