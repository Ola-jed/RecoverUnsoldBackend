using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecoverUnsoldDomain.Entities;

public class Offer : Entity
{
    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public ulong Duration { get; set; }

    public int? Beneficiaries { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public bool OnlinePayment { get; set; }

    [ForeignKey(nameof(Distributor))]
    public Guid DistributorId { get; set; }

    public Distributor? Distributor { get; set; }

    [ForeignKey(nameof(Location))]
    public Guid LocationId { get; set; }

    public Location? Location { get; set; }

    public ICollection<Product> Products { get; set; } = new HashSet<Product>();
}