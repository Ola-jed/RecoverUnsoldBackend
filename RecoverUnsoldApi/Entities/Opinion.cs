using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecoverUnsoldApi.Entities;

public class Opinion : Entity
{
    [Required]
    [Range(0, 5)]
    public int Rating { get; set; }

    [Required]
    [DataType(DataType.Text)]
    public string Comment { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Order))]
    public Guid OrderId { get; set; }

    public Order? Order { get; set; }
}