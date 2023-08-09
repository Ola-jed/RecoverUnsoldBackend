using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecoverUnsoldDomain.Entities;

public class Report : Entity
{
    [StringLength(100)]
    public string Reason { get; set; } = null!;

    [DataType(DataType.Text)]
    public string? Description { get; set; }

    public bool Processed { get; set; } = false;

    [ForeignKey(nameof(Customer))]
    public Guid CustomerId { get; set; }

    public Customer? Customer { get; set; }

    [ForeignKey(nameof(ReportedDistributor))]
    public Guid ReportedDistributorId { get; set; }

    public Distributor? ReportedDistributor { get; set; }
}