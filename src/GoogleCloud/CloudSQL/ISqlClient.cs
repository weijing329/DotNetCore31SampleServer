using System.Data.Common;

namespace DotNetCore31SampleServer.GoogleCloud.CloudSQL
{
  public interface ISqlClient
  {
    DbConnection GetDbConnection { get; }
  }
}