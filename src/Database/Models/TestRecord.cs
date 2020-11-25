using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NodaTime;

namespace DotNetCore31SampleServer.Database.Models
{
  public class TestRecord
  {
    [Key]
    public int Id { get; set; }

    public Boolean TestTypeBoolean { get; set; }

    public string TestTypeStringMax { get; set; }

    [StringLength(255)]
    public string TestTypeString255 { get; set; }

    public Int16 TestTypeInt16 { get; set; }

    public Int32 TestTypeInt32 { get; set; }

    // public TimeSpan TestTypeTimeSpan { get; set; }

    // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    // public DateTime Created { get; set; } = DateTime.UtcNow;

    // [Timestamp]
    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    // public DateTime LastUpdated { get; set; }

    // Date/Time Mapping with NodaTime
    // type 'Duration' is not a supported primitive type
    // type 'Instant' = timestamp = timestamp without time zone
    public Period TestTypePeriod { get; set; }

    // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Instant CreatedAt { get; set; }

    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public Instant LastUpdated { get; set; }

    // Concurrency Token
    public uint xmin { get; set; }
  }
}