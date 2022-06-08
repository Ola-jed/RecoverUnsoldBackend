using Microsoft.EntityFrameworkCore;
using RecoverUnsoldApi.Entities;

namespace RecoverUnsoldApi.Data;

public class DataContext: DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Location>()
            .Property(l => l.Coordinates)
            .HasConversion(
                v => v.ToString(),
                v => LatLong.FromString(v)
            );
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<EmailVerification> EmailVerifications { get; set; } = null!;
    public DbSet<PasswordReset> PasswordResets { get; set; } = null!;
    public DbSet<Distributor> Distributors { get; set; } = null!;
    public DbSet<Location> Locations { get; set; } = null!;
    public DbSet<Offer> Offers { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Product> Images { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<Opinion> Opinions { get; set; } = null!;
    public DbSet<Alert> Alerts { get; set; } = null!;
}