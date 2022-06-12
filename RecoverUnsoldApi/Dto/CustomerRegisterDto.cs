using System.ComponentModel.DataAnnotations;
using RecoverUnsoldApi.Validation;

namespace RecoverUnsoldApi.Dto;

public record CustomerRegisterDto([Required] [StringLength(100)] [UniqueUsername] string Username,
    [Required] [EmailAddress] [StringLength(100)] [UniqueEmail]
    string Email,
    [Required] string Password);