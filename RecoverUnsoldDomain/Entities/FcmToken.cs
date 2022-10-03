using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldDomain.Entities;

public class FcmToken: Entity
{
    [Required]
    [StringLength(255)]
    public string Value { get; set; } = null!;
    
    [Required]
    public Guid UserId { get; set; }
}