using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Entities;

public abstract class Entity
{
    [Key]
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}