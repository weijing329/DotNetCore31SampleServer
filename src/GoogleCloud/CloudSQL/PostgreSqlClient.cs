using System;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Logging;
using Npgsql;
using Polly;

namespace DotNetCore31SampleServer.GoogleCloud.CloudSQL
{
  public class PostgreSqlClient : ISqlClient
  {
    private readonly ILogger<PostgreSqlClient> _logger;
    private readonly DbConnection _connection;

    public DbConnection GetDbConnection => _connection;

    public PostgreSqlClient(ILogger<PostgreSqlClient> logger)
    {
      _logger = logger;
      _connection = InitializeDatabase();
    }

    private DbConnection InitializeDatabase()
    {
      // DbConnection connection;
      // connection = GetPostgreSqlConnection();
      // connection.Open();
      // using (var createTableCommand = connection.CreateCommand())
      // {
      //   createTableCommand.CommandText = @"
      //               CREATE TABLE IF NOT EXISTS
      //               votes(
      //                   vote_id SERIAL NOT NULL,
      //                   time_cast timestamp NOT NULL,
      //                   candidate VARCHAR(6) NOT NULL,
      //                   PRIMARY KEY (vote_id)   
      //               )";
      //   createTableCommand.ExecuteNonQuery();
      // }
      return GetPostgreSqlConnection();
    }

    private void SetDbConfigOptions(NpgsqlConnectionStringBuilder connectionString)
    {
      // [START cloud_sql_postgres_dotnet_ado_limit]
      // MaxPoolSize sets maximum number of connections allowed in the pool. 
      connectionString.MaxPoolSize = 5;
      // MinPoolSize sets the minimum number of connections in the pool.
      connectionString.MinPoolSize = 0;
      // [END cloud_sql_postgres_dotnet_ado_limit]
      // [START cloud_sql_postgres_dotnet_ado_timeout]
      // Timeout sets the time to wait (in seconds) while
      // trying to establish a connection before terminating the attempt.
      connectionString.Timeout = 15;
      // [END cloud_sql_postgres_dotnet_ado_timeout]
      // [START cloud_sql_postgres_dotnet_ado_lifetime]
      // ConnectionIdleLifetime sets the time (in seconds) to wait before
      // closing idle connections in the pool if the count of all
      // connections exceeds MinPoolSize.
      connectionString.ConnectionIdleLifetime = 300;
      // [END cloud_sql_postgres_dotnet_ado_lifetime]
    }

    private DbConnection NewPostgreSqlTCPConnection()
    {
      // [START cloud_sql_postgres_dotnet_ado_connection_tcp]
      // Equivalent connection string: 
      // "Uid=<DB_USER>;Pwd=<DB_PASS>;Host=<DB_HOST>;Database=<DB_NAME>;"
      var connectionString = new NpgsqlConnectionStringBuilder()
      {
        // The Cloud SQL proxy provides encryption between the proxy and instance. 
        SslMode = SslMode.Disable,

        // Remember - storing secrets in plaintext is potentially unsafe. Consider using
        // something like https://cloud.google.com/secret-manager/docs/overview to help keep
        // secrets secret.
        Host = Environment.GetEnvironmentVariable("DB_HOST"),     // e.g. '127.0.0.1'
                                                                  // Set Host to 'cloudsql' when deploying to App Engine Flexible environment
        Username = Environment.GetEnvironmentVariable("DB_USER"), // e.g. 'my-db-user'
        Password = Environment.GetEnvironmentVariable("DB_PASS"), // e.g. 'my-db-password'
        Database = Environment.GetEnvironmentVariable("DB_NAME"), // e.g. 'my-database'
      };
      connectionString.Pooling = true;
      // Specify additional properties here.
      // [START_EXCLUDE]
      SetDbConfigOptions(connectionString);
      // [END_EXCLUDE]
      NpgsqlConnection connection =
          new NpgsqlConnection(connectionString.ConnectionString);
      // [END cloud_sql_postgres_dotnet_ado_connection_tcp]
      return connection;
    }

    private DbConnection NewPostgreSqlUnixSocketConnection()
    {
      // [START cloud_sql_postgres_dotnet_ado_connection_socket]
      // Equivalent connection string: 
      // "Server=<dbSocketDir>/<INSTANCE_CONNECTION_NAME>;Uid=<DB_USER>;Pwd=<DB_PASS>;Database=<DB_NAME>"
      String dbSocketDir = Environment.GetEnvironmentVariable("DB_SOCKET_PATH") ?? "/cloudsql";
      String instanceConnectionName = Environment.GetEnvironmentVariable("INSTANCE_CONNECTION_NAME");
      var connectionString = new NpgsqlConnectionStringBuilder()
      {
        // The Cloud SQL proxy provides encryption between the proxy and instance. 
        SslMode = SslMode.Disable,
        // Remember - storing secrets in plaintext is potentially unsafe. Consider using
        // something like https://cloud.google.com/secret-manager/docs/overview to help keep
        // secrets secret.
        Host = String.Format("{0}/{1}", dbSocketDir, instanceConnectionName),
        Username = Environment.GetEnvironmentVariable("DB_USER"), // e.g. 'my-db-user
        Password = Environment.GetEnvironmentVariable("DB_PASS"), // e.g. 'my-db-password'
        Database = Environment.GetEnvironmentVariable("DB_NAME"), // e.g. 'my-database'

      };
      connectionString.Pooling = true;
      // Specify additional properties here.
      // [START_EXCLUDE]
      SetDbConfigOptions(connectionString);
      // [END_EXCLUDE]
      NpgsqlConnection connection =
          new NpgsqlConnection(connectionString.ConnectionString);
      // [END cloud_sql_postgres_dotnet_ado_connection_socket]
      return connection;
    }

    private DbConnection GetPostgreSqlConnection()
    {
      // [START cloud_sql_postgres_dotnet_ado_backoff]
      var connection = Policy
          .HandleResult<DbConnection>(conn => conn.State != ConnectionState.Open)
          .WaitAndRetry(new[]
          {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(5)
          }, (result, timeSpan, retryCount, context) =>
          {
            // Log any warnings here.
          })
          .Execute(() =>
          {
            // Return a new connection.
            // [START_EXCLUDE]
            if (Environment.GetEnvironmentVariable("DB_HOST") != null)
            {
              return NewPostgreSqlTCPConnection();
            }
            else
            {
              return NewPostgreSqlUnixSocketConnection();
            }
            // [END_EXCLUDE]
          });
      // [END cloud_sql_postgres_dotnet_ado_backoff]
      return connection;
    }
  }
}