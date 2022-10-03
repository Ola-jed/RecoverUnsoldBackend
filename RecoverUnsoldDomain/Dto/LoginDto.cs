using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldDomain.Dto;

public record LoginDto([Required] [EmailAddress] [StringLength(100)] string Email,
    [Required] string Password);