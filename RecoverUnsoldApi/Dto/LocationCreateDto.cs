using System.ComponentModel.DataAnnotations;
using RecoverUnsoldApi.Validation;

namespace RecoverUnsoldApi.Dto;

public record LocationCreateDto([Required] [StringLength(100)] string Name,
    [Required] [Range(0, 90)] double Latitude, [Required] [Range(-180, 180)] double Longitude,
    string? Indication = null,
    [MaxFileSize(5 * 1024 * 1024, Nullable = true)] [AllowedExtensions(".jpg,.jpeg,.png,.bmp", Nullable = true)]
    IFormFile? Image = null);