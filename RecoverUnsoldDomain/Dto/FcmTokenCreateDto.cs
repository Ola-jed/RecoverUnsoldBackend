using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldDomain.Dto;

public record FcmTokenCreateDto([Required] string Value);