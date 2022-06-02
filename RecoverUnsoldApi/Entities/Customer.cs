using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Entities;

public class Customer : User
{
    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    [StringLength(100)]
    public string LastName { get; set; } = null!;

    public ICollection<Alert> Alerts { get; set; } = new HashSet<Alert>();
}