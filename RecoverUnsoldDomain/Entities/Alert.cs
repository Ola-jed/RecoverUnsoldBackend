using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RecoverUnsoldDomain.Entities.Enums;

namespace RecoverUnsoldDomain.Entities;

public class Alert : Entity
{
    [Required]
    public AlertType AlertType { get; set; }

    [StringLength(50)]
    // If the type is distributor, store the id of the distributor here
    public string? Trigger { get; set; }

    [ForeignKey(nameof(Customer))]
    public Guid CustomerId { get; set; }

    public Customer? Customer { get; set; }
}