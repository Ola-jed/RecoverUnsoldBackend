using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Dto;

public record FcmTokenCreateDto([Required] string Value);