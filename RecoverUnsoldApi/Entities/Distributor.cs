using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Entities;

public class Distributor : User
{
    [Required]
    [StringLength(50)]
    public string Phone { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Ifu { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Rccm { get; set; } = null!;

    [StringLength(100)]
    public string? WebsiteUrl { get; set; } = null!;

    public ICollection<Location> Locations { get; set; } = new HashSet<Location>();
}