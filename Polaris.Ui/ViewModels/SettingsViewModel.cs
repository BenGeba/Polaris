using System;
using Avalonia;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Polaris.Core.Interfaces;
using Polaris.Core.Models;
using Polaris.Ui.Interfaces;

namespace Polaris.Ui.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly ISettingsService _settingsService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    public partial string Theme { get; set; }

    [ObservableProperty]
    public partial string ThumbnailSize { get; set; }

    [ObservableProperty]
    public partial int MaxParallelUploads { get; set; }

    public string ServerUrl { get; }

    public string ApiKeyMasked => "•••••••••••••••";

    public SettingsViewModel(ISettingsService settingsService, INavigationService navigationService)
    {
        _settingsService = settingsService;
        _navigationService = navigationService;

        var current = settingsService.Current;
        ServerUrl = current.ServerUrl;
        Theme = current.Theme.ToString();
        ThumbnailSize = current.ThumbnailSize.ToString();
        MaxParallelUploads = current.MaxParallelUploads;
    }

    [RelayCommand]
    private void SetTheme(string theme)
    {
        Theme = theme;
        _settingsService.Current.Theme = Enum.Parse<AppTheme>(theme);
        Persist();

        if (Application.Current is { } app)
        {
            app.RequestedThemeVariant = _settingsService.Current.Theme switch
            {
                AppTheme.Light => ThemeVariant.Light,
                AppTheme.Dark => ThemeVariant.Dark,
                _ => ThemeVariant.Default,
            };
        }
    }

    [RelayCommand]
    private void SetThumbnailSize(string size)
    {
        ThumbnailSize = size;
        _settingsService.Current.ThumbnailSize = Enum.Parse<ThumbnailSize>(size);
        Persist();
    }

    [RelayCommand]
    private void IncreaseUploads() => SetUploads(MaxParallelUploads + 1);

    [RelayCommand]
    private void DecreaseUploads() => SetUploads(MaxParallelUploads - 1);

    private void SetUploads(int value)
    {
        MaxParallelUploads = Math.Clamp(value, 1, 16);
        _settingsService.Current.MaxParallelUploads = MaxParallelUploads;
        Persist();
    }

    [RelayCommand]
    private async System.Threading.Tasks.Task LogoutAsync()
    {
        await _settingsService.SaveAsync(new AppSettings { ServerUrl = _settingsService.Current.ServerUrl });
        _navigationService.NavigateTo<LoginViewModel>();
    }

    private void Persist() => _ = _settingsService.SaveAsync(_settingsService.Current);
}
