using System.Threading.Tasks;
using DotNetCore31SampleServer.GoogleCloud.PubSub;
using Google.Cloud.Firestore;

namespace DotNetCore31SampleServer.GoogleCloud.Firestore
{
  public interface IFirestoreClient
  {
    FirestoreDb Db { get; }

    Task SetPubSubMessageDoc(PubsubMessage message);
  }
}