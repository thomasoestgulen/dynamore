using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assembly = System.Reflection.Assembly;

namespace Dynamore.DynamoreStatistics
{
  internal class Statistics
  {
    // Defines the statistics class.

    public Guid SessionKey { get; private set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set }
    public string String1 { get; set; }
    public string String2 { get; set; }
    public string String3 { get; set; }
    public string String4 { get; set; }
    public string String5 { get; set; }
    public string String6 { get; set; }
    public string String7 { get; set; }
    public string String8 { get; set; }
    public string String9 { get; set; }
    public string String10 { get; set; }

    public string Version { get; set; }
    public string Software { get; set; }
    public string SoftwareVersion { get; set; }

    public string User { get; set; }
    public string Machine { get; set; }

    internal Statistics(string string1, string string2, string string3, string string4, string string5, string string6, string string7, string string8, string string9, string string10)
    {
      SessionKey = Guid.NewGuid();
      StartTime = DateTime.Now;
      String1 = string1 ?? "";
      String2 = string2 ?? "";
      String3 = string3 ?? "";
      String4 = string4 ?? "";
      String5 = string5 ?? "";
      String6 = string6 ?? "";
      String7 = string7 ?? "";
      String8 = string8 ?? "";
      String9 = string9 ?? "";
      String10 = string10 ?? "";
      Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
      Software = "Dynamo";
      SoftwareVersion = string8;//Grasshopper.Versioning.VersionString;
      User = Environment.UserName;
      Machine = Environment.MachineName;
    }

  }
}
