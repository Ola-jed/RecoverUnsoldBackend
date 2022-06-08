using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace RecoverUnsoldApi.Entities;

[Index(nameof(Ifu), IsUnique = true)]
[Index(nameof(Rccm), IsUnique = true)]
[Index(nameof(Phone), IsUnique = true)]
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
    public string? WebsiteUrl { get; set; }

    public ICollection<Location> Locations { get; set; } = new HashSet<Location>();
}