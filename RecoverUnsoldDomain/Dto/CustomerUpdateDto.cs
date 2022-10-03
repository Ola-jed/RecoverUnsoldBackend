using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldDomain.Dto;

public record CustomerUpdateDto([Required] [StringLength(100)] string Username, [StringLength(100)] string? FirstName,
    [StringLength(100)] string? LastName);