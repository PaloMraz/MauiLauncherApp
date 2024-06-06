using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiLauncherApp;

public partial class LauncherService
{
  public partial IEnumerable<AppInfoRecord> GetInstalledApps();
}


public record class AppInfoRecord(string PackageName, string Label, ImageSource Icon);
