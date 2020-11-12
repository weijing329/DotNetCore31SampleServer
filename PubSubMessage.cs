using System;
using System.Collections.Generic;

namespace DotNetCoreGoogleCloudPubSubPushSubscriberOnCloudRun
{
  public class PubsubMessage
  {
    public PushMessage message { get; set; }

    public string subscription { get; set; }
  }

  public class PushMessage
  {
    public Dictionary<String, String> attributes { get; set; }
    public string data { get; set; }
    public string messageId { get; set; }
  }
}
