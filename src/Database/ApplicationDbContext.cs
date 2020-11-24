using System.Data.Common;
using DotNetCore31SampleServer.GoogleCloud.CloudSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
            => optionsBuilder.UseNpgsql(_connection);
  }
}
