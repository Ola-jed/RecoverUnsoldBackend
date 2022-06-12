using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Entities;

public class Image : Entity
{
    [Required]
    [StringLength(255)]
    public string Url { get; set; } = null!;

    [Required]
    [StringLength(255)]
    public string PublicId { get; set; } = null!;
}