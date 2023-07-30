using System.ComponentModel.DataAnnotations.Schema;

namespace RecoverUnsoldDomain.Entities;

public class Repayment : Entity
{
    public bool Done { get; set; }

    [ForeignKey(nameof(Order))] public Guid OrderId { get; set; }

    public string? Note { get; set; }

    public string? TransactionId { get; set; }

    public Order? Order { get; set; }
}