using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Dto;

public record CustomerRegisterDto([Required] [StringLength(100)] string Username,
    [Required] [EmailAddress] [StringLength(100)] string Email,
    [Required] string Password);