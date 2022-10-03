using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldDomain.Dto;

public record UserVerificationStartDto([Required] [EmailAddress] string Email);