using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DotNetCoreGoogleCloudPubSubPushSubscriberOnCloudRun.Controllers
{
  [ApiController]
  [Route("")]
  public class IndexController : ControllerBase
  {
    private readonly ILogger<IndexController> _logger;

    public IndexController(ILogger<IndexController> logger)
    {
      _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PubsubMessage message)
    {
      var jsonMessage = JsonConvert.SerializeObject(message);
      Console.WriteLine(jsonMessage);
      // HTTP 400
      // return BadRequest();

      // HTTP 500
      // return StatusCode(500);

      // HTTP 204
      return Accepted();
    }
  }
}
