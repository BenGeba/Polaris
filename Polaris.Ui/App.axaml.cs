using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Polaris.Core.Interfaces;
using Polaris.Infrastructure.DI;
using Polaris.Ui.DI;
using Polaris.Ui.Interfaces;
using Polaris.Ui.ViewModels;
using Polaris.Ui.Views;

namespace Polaris.Ui;

public partial class App : Application
{
    public IServiceProvider Services { get; private set; } = null!;
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        Services = services.BuildServiceProvider();
        
        var settingsService = Services.GetRequiredService<ISettingsService>();
        settingsService.LoadAsync().GetAwaiter().GetResult();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = Services.GetRequiredService<MainWindow>();
            // desktop.MainWindow = new MainWindow
            // {
            //     DataContext = new MainWindowViewModel()
            // };
            
            var navigation = Services.GetRequiredService<INavigationService>();
            if (settingsService.Current.IsConfigured)
            {
                navigation.NavigateTo<TimelineViewModel>();
            }
            else
            {
                navigation.NavigateTo<LoginViewModel>();
            }
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSettings();
        services.AddImmichApi();
        services.AddNavigation();
        services.AddViewModels();
    }
}