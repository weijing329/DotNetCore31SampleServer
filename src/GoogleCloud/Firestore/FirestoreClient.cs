using System;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;
using DotNetCore31SampleServer.GoogleCloud.PubSub;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DotNetCore31SampleServer.GoogleCloud.Firestore
{
  public class FirestoreClient : IFirestoreClient
  {
    private readonly ILogger<FirestoreClient> _logger;
    private readonly FirestoreDb _db;

    public FirestoreDb Db => _db;

    public FirestoreClient(ILogger<FirestoreClient> logger)
    {
      _logger = logger;

      const string projectId = "sunkang-iot-monitor-service";

      _db = FirestoreDb.Create(projectId);
      _logger.LogInformation($"Created Cloud Firestore client with project ID: {projectId}");
    }

    public async Task SetPubSubMessageDoc(PubsubMessage message)
    {
      DocumentReference docRef = _db.Collection("PubSubMessage").Document(DateTime.UtcNow.ToString("o"));

      // decode pubsub message data
      string decodedMessageText = Encoding.UTF8.GetString(Convert.FromBase64String(message.message.data));
      message.message.data = decodedMessageText;

      // Dirty workaround for Error: Unable to create converter
      // From: Google.Cloud.Firestore.Converters.ConverterCache.CreateConverter 
      var expandoObject = JsonConvert.DeserializeObject<ExpandoObject>(JsonConvert.SerializeObject(message));

      await docRef.SetAsync(expandoObject);
    }
  }
}