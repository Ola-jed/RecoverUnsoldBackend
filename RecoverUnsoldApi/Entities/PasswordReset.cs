using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace RecoverUnsoldApi.Entities;

[Index(nameof(Token), IsUnique = true)]
public class PasswordReset : Entity
{
    [Required]
    [StringLength(150)]
    public string Token { get; set; } = null!;

    [Required]
    public Guid UserId { get; set; }
}