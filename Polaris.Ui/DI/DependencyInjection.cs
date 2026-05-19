using Microsoft.Extensions.DependencyInjection;
using Polaris.Ui.Interfaces;
using Polaris.Ui.Services;
using Polaris.Ui.ViewModels;
using Polaris.Ui.Views;

namespace Polaris.Ui.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<MainWindow>();
        
        services.AddTransient<TimelineViewModel>();
        services.AddTransient<ExploreViewModel>();
        services.AddTransient<LoginViewModel>();
        // services.AddTransient<AlbumsViewModel>();
        // services.AddTransient<PeopleViewModel>();
        // services.AddTransient<MapViewModel>();
        // services.AddTransient<SettingsViewModel>();
        
        return services;
    }
    
    public static IServiceCollection AddNavigation(this IServiceCollection services)
    {
        services.AddSingleton<INavigationService,  NavigationService>();
        
        return services;
    }
}