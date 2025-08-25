using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Persistence.Concepts;
using SchoolDonations.CoreDomain.Aggregates.Customers;
using SchoolDonations.EFCore.Customers;
using SchoolDonations.EFCore.DomainEvents;

namespace SchoolDonations.EFCore;

public class AppDbContext : DbContext
{
    #region DBSets

    internal DbSet<Customer> Customers { get; set;  }
    internal DbSet <DomainEventDto> DomainEvents { get; set; }

    #endregion DBSets

    private DbSettings DbSettings  { get; }

    public AppDbContext(DbContextOptions<AppDbContext> options, IOptionsSnapshot<DbSettings> dbSettings)
        : base(options)
    {
        DbSettings = dbSettings.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        optionsBuilder.UseNpgsql(DbSettings.ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        modelBuilder.ApplyConfiguration(new DomainConfiguration());
    }
}
