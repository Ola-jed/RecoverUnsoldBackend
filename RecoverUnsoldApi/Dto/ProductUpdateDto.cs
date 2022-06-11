using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Dto;

public record ProductUpdateDto([Required][StringLength(100)] string Name, [Required] string Description);