using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore31SampleServer.GoogleCloud.Firestore;
using DotNetCore31SampleServer.GoogleCloud.PubSub;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DotNetCore31SampleServer.WebAPI.Controllers
{
  [ApiController]
  [Route("")]
  public class IndexController : ControllerBase
  {
    private readonly ILogger<IndexController> _logger;
    private readonly IFirestoreClient _firestore;

    public IndexController(ILogger<IndexController> logger, IFirestoreClient firestore)
    {
      _logger = logger;
      _firestore = firestore;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PubsubMessage message)
    {
      await _firestore.SetPubSubMessageDoc(message);
      // var jsonMessage = JsonConvert.SerializeObject(message);
      // Console.WriteLine(jsonMessage);

      // For parameters check
      // HTTP 400
      // return BadRequest();

      // Try-Catch exception
      // HTTP 500
      // return StatusCode(500);

      // PubSub message acknowledgement
      // HTTP 202
      return Accepted();
    }
  }
}
