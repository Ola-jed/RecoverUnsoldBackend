using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RecoverUnsoldDomain.Entities;

[Index(nameof(TransactionId), IsUnique = true)]
public class Payment: Entity
{
    [Required]
    public string TransactionId { get; set; } = null!;
    
    [ForeignKey(nameof(Order))]
    public Guid OrderId { get; set; }

    public bool PaidBack { get; set; } = false;

    public Order? Order { get; set; }
}