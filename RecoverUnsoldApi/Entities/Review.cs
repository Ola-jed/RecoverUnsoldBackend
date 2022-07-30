using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecoverUnsoldApi.Entities;

public class Review: Entity
{
    [Required]
    [DataType(DataType.Text)]
    public string Comment { get; set; } = null!;
    
    [ForeignKey(nameof(Customer))]
    public Guid CustomerId { get; set; }

    public Customer? Customer { get; set; }
}