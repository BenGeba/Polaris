using System;
using Microsoft.Extensions.DependencyInjection;
using Polaris.Ui.Interfaces;
using Polaris.Ui.ViewModels;

namespace Polaris.Ui.Services;

public class NavigationService(IServiceProvider services)
    : INavigationService
{
    private MainWindowViewModel? _shell;

    private MainWindowViewModel Shell => _shell ??= services.GetRequiredService<MainWindowViewModel>();

    public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
    {
        NavigateTo(typeof(TViewModel));
    }

    public void NavigateTo(Type viewModelType)
    {
        var viewModel = services.GetService(viewModelType) as ViewModelBase ??
                        throw new InvalidOperationException($"ViewModel of type {viewModelType} was not found.");

        if (viewModelType == typeof(LoginViewModel))
        {
            Shell.IsShellVisible = false;
            Shell.CurrentPage = viewModel;
            return;
        }

        if (!Shell.IsShellVisible)
        {
            Shell.IsShellVisible = true;
        }

        Shell.CurrentPage = viewModel;
        Shell.SyncActiveNav(viewModelType);
    }
}