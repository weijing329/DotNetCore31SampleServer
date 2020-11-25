using System.Data.Common;
using DotNetCore31SampleServer.GoogleCloud.CloudSQL;
using DotNetCore31SampleServer.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NodaTime;
using DotNetCore31SampleServer.ValueGenerators;

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
    => modelBuilder.Entity<TestRecord>(entity =>
    {
      entity.Property(t => t.CreatedAt)
        .HasDefaultValueSql("CURRENT_TIMESTAMP")
        .ValueGeneratedOnAdd();
      entity.Property(t => t.LastUpdated)
        // .HasValueGenerator<NodaTimeInstantGenerator>()
        .ValueGeneratedOnAddOrUpdate();
      entity.UseXminAsConcurrencyToken();
    });
  }
}
