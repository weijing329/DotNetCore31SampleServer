using System.Data.Common;
using DotNetCore31SampleServer.GoogleCloud.CloudSQL;
using DotNetCore31SampleServer.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using NodaTime;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetCore31SampleServer.Database
{
  public class ApplicationDbContext : DbContext
  {
    private readonly ILogger<ApplicationDbContext> _logger;
    private readonly DbConnection _connection;

    public ApplicationDbContext(ILogger<ApplicationDbContext> logger, ISqlClient client)
    {
      _logger = logger;
      _connection = client.GetDbConnection;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(_connection, o => o.UseNodaTime());

    public DbSet<TestRecord> TestRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<TestRecord>(entity =>
      {
        entity.Property(t => t.CreatedAt)
          .HasDefaultValueSql("CURRENT_TIMESTAMP");
        // // .ValueGeneratedOnAdd();
        entity.Property(t => t.LastUpdated)
        //   // postgresql generated columns only allow immutatable function, all timestamp functions are stable function
        //   // .HasComputedColumnSql("now()::timestamp") 
        //   // .HasValueGenerator<NodaTimeInstantGenerator>()
        //   // .ValueGeneratedOnAddOrUpdate();
          .HasDefaultValueSql("CURRENT_TIMESTAMP");
        entity.UseXminAsConcurrencyToken();
      });
    }

    public override int SaveChanges()
    {
      GenerateInstantColumn();

      return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
      GenerateInstantColumn();

      return base.SaveChangesAsync(acceptAllChangesOnSuccess: true, cancellationToken);
    }

    private void GenerateInstantColumn()
    {
      var entries = ChangeTracker
          .Entries()
          .Where(e => e.Entity is TestRecord && (
                  // e.State == EntityState.Added ||
                  e.State == EntityState.Modified));

      foreach (var entityEntry in entries)
      {
        ((TestRecord)entityEntry.Entity).LastUpdated = SystemClock.Instance.GetCurrentInstant();

        // if (entityEntry.State == EntityState.Added)
        // {
        //   ((TestRecord)entityEntry.Entity).CreatedAt = SystemClock.Instance.GetCurrentInstant();
        // }
      }
    }
  }
}
