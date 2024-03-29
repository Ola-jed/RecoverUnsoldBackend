using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace RecoverUnsoldDomain.Entities;

[Index(nameof(Token), IsUnique = true)]
public class EmailVerification : Entity
{
    [Required]
    [StringLength(150)]
    public string Token { get; set; } = null!;

    [Required]
    public Guid UserId { get; set; }
}