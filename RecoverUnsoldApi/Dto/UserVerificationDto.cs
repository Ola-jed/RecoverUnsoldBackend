using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Dto;

public record UserVerificationDto([Required] string Token);