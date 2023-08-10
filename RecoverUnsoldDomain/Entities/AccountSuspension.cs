using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecoverUnsoldDomain.Entities;

public class AccountSuspension : Entity
{
    [StringLength(255)]
    public string Reason { get; set; } = null!;

    public bool Active { get; set; }

    public DateTime Date { get; set; }

    public DateTime? EndDate { get; set; }

    [ForeignKey(nameof(Distributor))]
    public Guid DistributorId { get; set; }

    public Distributor? Distributor { get; set; }
}