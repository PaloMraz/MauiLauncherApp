using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiLauncherApp;

public partial class LauncherService
{
  public partial IEnumerable<AppInfoRecord> GetInstalledApps()
  {
    UserManager userManager = (UserManager)Android.App.Application.Context.GetSystemService(Android.App.Application.UserService)!;
    LauncherApps launcherApps = (LauncherApps)Android.App.Application.Context.GetSystemService(Context.LauncherAppsService)!;

    foreach (UserHandle userHandle in userManager.UserProfiles!)
    {
      foreach (var resolveActivityInfo in launcherApps.GetActivityList(null, userHandle)!)
      {
        ApplicationInfo applicationInfo = resolveActivityInfo.ApplicationInfo!;

        string packageName = applicationInfo.PackageName!;
        string label = resolveActivityInfo.Label ?? packageName;
        Drawable? icon = resolveActivityInfo.GetIcon(0);
        if (icon is null)
        {
          continue;
        }
        ImageSource imageSource = DrawableToImageSource(icon);
        yield return new AppInfoRecord(packageName, label, imageSource);
      }
    }
  }


  private static ImageSource DrawableToImageSource(Drawable drawable)
  {
    Bitmap bitmap = Bitmap.CreateBitmap(drawable.IntrinsicWidth, drawable.IntrinsicHeight, Bitmap.Config.Argb8888!);
    using Canvas bitmapCanvas = new Canvas(bitmap);
    drawable.SetBounds(0, 0, bitmapCanvas.Width, bitmapCanvas.Height);
    drawable.Draw(bitmapCanvas);

    var stream = new MemoryStream();
    bitmap.Compress(Bitmap.CompressFormat.Png!, 100, stream);
    stream.Seek(0, SeekOrigin.Begin);

    return ImageSource.FromStream(() => stream);
  }
}

