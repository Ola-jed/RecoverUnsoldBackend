using System.ComponentModel.DataAnnotations;
using RecoverUnsoldApi.Validation;

namespace RecoverUnsoldApi.Dto;

public record ProductCreateDto([Required] [StringLength(100)] string Name,
    [Required] string Description,
    [MaxFileSize(5 * 1024 * 1024, Nullable = true)]
    [AllowedExtensions(".jpg,.jpeg,.png,.bmp", Nullable = true)]
    IEnumerable<IFormFile>? Images = null);