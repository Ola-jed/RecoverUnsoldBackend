using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Dto;

public record UserVerificationStartDto([Required] [EmailAddress] string Email);