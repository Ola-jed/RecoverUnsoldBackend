using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Entities;

public abstract class User : Entity
{
    [Required]
    [StringLength(100)]
    public string Username { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    public DateTime? EmailVerifiedAt { get; set; }
}