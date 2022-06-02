using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Entities;

public class Image : Entity
{
    [Required]
    [StringLength(255)]
    public string Path { get; set; } = null!;
}