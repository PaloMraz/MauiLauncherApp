using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MauiLauncherApp;

public partial class MainViewModel : ObservableObject
{
  private readonly LauncherService _launcherService;

  public MainViewModel(LauncherService launcherService)
  {
    this._launcherService = launcherService;
  }


  public string AppName => $"{AppInfo.Name} {AppInfo.VersionString}";


  [RelayCommand]
  private async Task RefreshAppsAsync()
  {
    try
    {
      this.IsBusy = true;
      this.AppsForCollectionView.Clear();
      this.AppsForListView.Clear();
      await Task.Yield();

      var currentList = this.UseCollectionView ? this.AppsForCollectionView : this.AppsForListView;

      var retrieveStopwatch = Stopwatch.StartNew();
      IEnumerable<AppInfoRecord> apps = this._launcherService.GetInstalledApps().ToList();
      retrieveStopwatch.Stop();

      var addToListStopwatch = Stopwatch.StartNew();
      foreach (var app in apps.OrderBy(app => app.Label))
      {
        currentList.Add(app);
      }
      addToListStopwatch.Stop();

      await Application.Current!.MainPage!.DisplayAlert(
        AppInfo.Name, 
        $"Retrieval time {retrieveStopwatch.Elapsed}, add to list time: {addToListStopwatch.Elapsed}", "OK");
    }
    catch (Exception ex)
    {
      await Application.Current!.MainPage!.DisplayAlert(AppInfo.Name, $"Error: {ex}", "OK");
    }
    finally
    {
      this.IsBusy = false;
    }
  }


  [ObservableProperty]
  private bool _isBusy;


  public bool IsReady => !this.IsBusy;


  partial void OnIsBusyChanged(bool value)
  {
    this.OnPropertyChanged(nameof(this.IsReady));
  }


  [ObservableProperty]
  private bool _useCollectionView;


  public bool UseListView => !this.UseCollectionView;


  partial void OnUseCollectionViewChanged(bool value)
  {
    this.OnPropertyChanged(nameof(this.UseListView));
  }


  public ObservableCollection<AppInfoRecord> AppsForListView { get; } = new ObservableCollection<AppInfoRecord>();


  public ObservableCollection<AppInfoRecord> AppsForCollectionView { get; } = new ObservableCollection<AppInfoRecord>();
}
