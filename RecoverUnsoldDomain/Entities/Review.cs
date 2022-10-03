using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecoverUnsoldDomain.Entities;

public class Review: Entity
{
    [Required]
    [DataType(DataType.Text)]
    public string Comment { get; set; } = null!;
    
    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }

    public User? User { get; set; }
}