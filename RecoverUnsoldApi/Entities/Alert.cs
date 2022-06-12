using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RecoverUnsoldApi.Entities.Enums;

namespace RecoverUnsoldApi.Entities;

public class Alert : Entity
{
    [Required]
    public AlertType AlertType { get; set; }

    [StringLength(50)]
    // If the type is distributor, store the id of the distributor here
    public string? Trigger { get; set; } = null!;

    [ForeignKey(nameof(Customer))]
    public Guid CustomerId { get; set; }

    public Customer? Customer { get; set; }
}