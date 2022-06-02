using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Entities;

public abstract class Entity
{
    [Key]
    public Guid Id { get; set; }

    public DateTime CreatedAt = DateTime.Now;
    public DateTime? DeletedAt { get; set; }
}