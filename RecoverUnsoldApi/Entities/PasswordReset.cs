using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Entities;

public class PasswordReset : Entity
{
    [Required]
    [StringLength(150)]
    public string Token { get; set; } = null!;
    
    [Required]
    public Guid UserId { get; set; }
}