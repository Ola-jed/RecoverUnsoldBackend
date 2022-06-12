using System.ComponentModel.DataAnnotations;
using RecoverUnsoldApi.Validation;

namespace RecoverUnsoldApi.Dto;

public record LocationUpdateDto([Required] [StringLength(100)] string Name,
    [RegularExpression(@"^-?(([0-8]?\d)\.(\d+))|(90(\.0+)?);-?((((1[0-7]\d)|(\d?\d))\.(\d+))|180(\.0+)?)$")]
    string Coordinates, string? Indication = null,
    [MaxFileSize(5 * 1024 * 1024)] [AllowedExtensions(".jpg,.jpeg,.png,.bmp")]
    IFormFile? Image = null);