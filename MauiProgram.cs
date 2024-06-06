using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace MauiLauncherApp;

public static class MauiProgram
{
  public static MauiApp CreateMauiApp()
  {
    var builder = MauiApp.CreateBuilder();
    builder
      .UseMauiApp<App>()
      .UseMauiCommunityToolkit()
      .ConfigureFonts(fonts =>
      {
        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
      });

#if DEBUG
    builder.Logging.AddDebug();
#endif

    builder.Services.AddSingleton<LauncherService>();

    builder.Services.AddTransient<MainViewModel>();
    builder.Services.AddTransient<MainPage>();

    return builder.Build();
  }
}
