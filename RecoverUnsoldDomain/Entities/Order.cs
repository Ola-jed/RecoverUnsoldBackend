using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RecoverUnsoldDomain.Entities.Enums;

namespace RecoverUnsoldDomain.Entities;

public class Order : Entity
{
    public DateTime WithdrawalDate { get; set; }

    [Required]
    public Status Status { get; set; } = Status.Pending;

    [ForeignKey(nameof(Customer))]
    public Guid CustomerId { get; set; }

    public Customer? Customer { get; set; }

    [ForeignKey(nameof(Offer))]
    public Guid OfferId { get; set; }

    public Offer? Offer { get; set; }

    public ICollection<Opinion> Opinions { get; set; } = new HashSet<Opinion>();
}