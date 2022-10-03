using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldDomain.Dto;

public record UserVerificationDto([Required] string Token);