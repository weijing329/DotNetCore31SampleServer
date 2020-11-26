using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore31SampleServer.Database;
using DotNetCore31SampleServer.Database.Models;
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
    private readonly ApplicationDbContext _dbContext;

    public IndexController(ILogger<IndexController> logger, IFirestoreClient firestore, ApplicationDbContext dbContext)
    {
      _logger = logger;
      _firestore = firestore;
      _dbContext = dbContext;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PubsubMessage message)
    {
      // await _firestore.SetPubSubMessageDoc(message);
      // var jsonMessage = JsonConvert.SerializeObject(message);
      // Console.WriteLine(jsonMessage);

      var newRecord = new TestRecord()
      {
        TestTypeStringMax = "Add Record"
      };

      try
      {
        TestRecord existingRecord = await _dbContext.TestRecords.FirstOrDefaultAsync();
        if (existingRecord != null)
        {
          existingRecord.TestTypeStringMax = $"{existingRecord.TestTypeStringMax};Updated";
        }
        else
        {
          _dbContext.TestRecords.Add(newRecord);
        }
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Upsert a record");
      }
      catch (System.Exception exception)
      {
        _logger.LogError($"{exception.Message} - {exception.InnerException.Message}");
      }

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
