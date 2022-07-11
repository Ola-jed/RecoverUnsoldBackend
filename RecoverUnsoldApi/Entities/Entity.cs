using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace RecoverUnsoldApi.Entities;

[Index(nameof(CreatedAt))]
public abstract class Entity
{
    [Key]
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}