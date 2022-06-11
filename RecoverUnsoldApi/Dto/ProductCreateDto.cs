using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Dto;

public record ProductCreateDto([Required][StringLength(100)] string Name,
    [Required] string Description, IEnumerable<IFormFile> Images);