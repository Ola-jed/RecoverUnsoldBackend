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
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        modelBuilder.Entity<Distributor>()
            .HasIndex(d => d.Email)
            .IsUnique();
        modelBuilder.Entity<Distributor>()
            .HasIndex(d => d.Phone)
            .IsUnique();
        modelBuilder.Entity<Distributor>()
            .HasIndex(d => d.Ifu)
            .IsUnique();
        modelBuilder.Entity<Distributor>()
            .HasIndex(d => d.Rccm)
            .IsUnique();
        modelBuilder.Entity<EmailVerification>()
            .HasIndex(e => e.Token)
            .IsUnique();
        modelBuilder.Entity<PasswordReset>()
            .HasIndex(p => p.Token)
            .IsUnique();
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