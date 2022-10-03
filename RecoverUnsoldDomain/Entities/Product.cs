using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecoverUnsoldDomain.Entities;

public class Product : Entity
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [DataType(DataType.Text)]
    public string Description { get; set; } = null!;

    [ForeignKey(nameof(Offer))]
    public Guid OfferId { get; set; }

    public Offer? Offer { get; set; }

    public ICollection<Image> Images { get; set; } = new HashSet<Image>();
}