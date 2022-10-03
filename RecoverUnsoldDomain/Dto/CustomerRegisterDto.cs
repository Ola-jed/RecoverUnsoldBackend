using System.ComponentModel.DataAnnotations;
using RecoverUnsoldDomain.Validation;

namespace RecoverUnsoldDomain.Dto;

public record CustomerRegisterDto([Required] [StringLength(100)] [UniqueUsername] string Username,
    [Required] [EmailAddress] [StringLength(100)] [UniqueEmail]
    string Email,
    [Required] string Password);