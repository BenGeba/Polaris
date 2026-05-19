using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Polaris.Core.Interfaces;
using Polaris.Core.Models;
using Polaris.Ui.Interfaces;

namespace Polaris.Ui.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
    private readonly ISettingsService _settingsService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    public partial string ServerUrl { get; set; }

    [ObservableProperty]
    public partial string ApiKey { get; set; } = string.Empty;

    [ObservableProperty] 
    public partial string? ErrorMessage { get; set; }
    
    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    public LoginViewModel(IAuthService authService,
        ISettingsService settingsService,
        INavigationService navigationService)
    {
        _authService = authService;
        _settingsService = settingsService;
        _navigationService = navigationService;
        
        ServerUrl = settingsService.Current.ServerUrl;
    }

    [RelayCommand]
    private async Task LoginAsync(CancellationToken cancellationToken)
    {
        ErrorMessage = null;

        if (string.IsNullOrWhiteSpace(ServerUrl) || string.IsNullOrWhiteSpace(ApiKey))
        {
            ErrorMessage = "Invalid Server Url or API Key";
            return;
        }
        
        IsLoading = true;
        
        var isValid = await _authService.ValidateApiKeyAsync(ServerUrl, ApiKey, cancellationToken);

        if (!isValid)
        {
            ErrorMessage = "Invalid API Key";
            IsLoading = false;
            return;
        }

        await _settingsService.SaveAsync(new AppSettings
        {
            ServerUrl = ServerUrl,
            ApiKey = ApiKey
        });
        
        _navigationService.NavigateTo<TimelineViewModel>();
        IsLoading = false;
    }
}