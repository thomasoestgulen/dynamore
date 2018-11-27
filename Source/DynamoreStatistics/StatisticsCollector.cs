using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace Dynamore.DynamoreStatistics
{
  /// <summary>
  /// Used to collects usage statistics with same format as SwecoCommon
  /// </summary>

  class StatisticsCollector
  {
    /// <summary>
    /// At the moment, all the log files are saved to S-drive where someone has to manually send to server
    /// </summary>

    public const string Directory = @"S:\Administration\7 Bygg\35123 BGO Bru\05 Faglig\09 Dynamo arbeidsområde\DynamoStatistics";

    private Statistics _statistics;

    public void Start(string string1, string string2, string string3, string string4, string string5, string string6, string string7, string string8, string string9, string string10)
    {
      _statistics = new Statistics(string1, string2, string3, string4, string5, string6, string7, string8, string9, string10);
    }

    public void End()
    {
      try
      {
        _statistics.EndTime = DateTime.Now;

        var jsonLine = JsonConvert.SerializeObject(_statistics);
        var file = Path.Combine(Directory, Guid.NewGuid() + ".log");

        using (var writer = new StreamWriter(File.Open(file, FileMode.CreateNew, FileAccess.Write, FileShare.Read)))
        {
          writer.WriteLine(jsonLine);
        }

      }
      catch (Exception e)
      {
        Debug.WriteLine(e);
      }
    }


  }
}
