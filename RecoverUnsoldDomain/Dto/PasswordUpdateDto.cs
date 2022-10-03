using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldDomain.Dto;

public record PasswordUpdateDto([Required] string NewPassword, [Required] string OldPassword);