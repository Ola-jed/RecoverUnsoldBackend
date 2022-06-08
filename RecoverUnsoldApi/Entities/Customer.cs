using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Entities;

public class Customer : User
{
    [StringLength(100)]
    public string? FirstName { get; set; }

    [StringLength(100)]
    public string? LastName { get; set; }

    public ICollection<Alert> Alerts { get; set; } = new HashSet<Alert>();
}