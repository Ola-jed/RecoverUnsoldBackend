using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Dto;

public record CustomerUpdateDto([Required] [StringLength(100)] string Username, [StringLength(100)] string? FirstName,
    [StringLength(100)] string? LastName);