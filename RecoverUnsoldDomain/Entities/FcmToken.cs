using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace RecoverUnsoldDomain.Entities;

[Index(nameof(Value), IsUnique = true)]
public class FcmToken: Entity
{
    [Required]
    [StringLength(255)]
    public string Value { get; set; } = null!;
    
    [Required]
    public Guid UserId { get; set; }
}