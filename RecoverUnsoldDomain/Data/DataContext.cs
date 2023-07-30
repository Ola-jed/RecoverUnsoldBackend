using Microsoft.EntityFrameworkCore;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldDomain.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Location>()
            .Property(l => l.Coordinates)
            .HasColumnType("geography (point)");
        modelBuilder.HasSequence<int>("OrderNumbers")
            .StartsAt(1)
            .IncrementsBy(1);

        modelBuilder.Entity<Order>()
            .Property(o => o.Number)
            .HasDefaultValueSql("nextval('\"OrderNumbers\"')");
        modelBuilder.HasPostgresExtension("postgis");
        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        ApplyCreatedAtToNewEntities();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplyCreatedAtToNewEntities();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        ApplyCreatedAtToNewEntities();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = new())
    {
        ApplyCreatedAtToNewEntities();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void ApplyCreatedAtToNewEntities()
    {
        var entityEntries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added);
        foreach (var entityEntry in entityEntries)
        {
            ((Entity)entityEntry.Entity).CreatedAt = DateTime.Now;
        }
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Administrator> Administrators { get; set; } = null!;
    public DbSet<EmailVerification> EmailVerifications { get; set; } = null!;
    public DbSet<PasswordReset> PasswordResets { get; set; } = null!;
    public DbSet<Distributor> Distributors { get; set; } = null!;
    public DbSet<Location> Locations { get; set; } = null!;
    public DbSet<Offer> Offers { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Product> Images { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;
    public DbSet<Opinion> Opinions { get; set; } = null!;
    public DbSet<Alert> Alerts { get; set; } = null!;
    public DbSet<FcmToken> FcmTokens { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; } = null!;
    public DbSet<Repayment> Repayments { get; set; } = null!;
}