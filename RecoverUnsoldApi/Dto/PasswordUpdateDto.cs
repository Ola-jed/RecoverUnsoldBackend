using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Dto;

public record PasswordUpdateDto([Required] string NewPassword, [Required] string OldPassword);