using Microsoft.Extensions.DependencyInjection;
using Polaris.Core.Interfaces;
using Polaris.Infrastructure.Settings;

namespace Polaris.Infrastructure.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddSettings(this IServiceCollection services)
    {
        services.AddSingleton<ISettingsService, JsonSettingsService>();
        
        return services;
    }

    public static IServiceCollection AddImmichApi(this IServiceCollection services)
    {
        // services.AddHttpClient("ImmichApi");
        // services.AddSingleton<ImmichApiClient>(...);
        // services.AddTransient<IAssetRepository, AssetRepository>();
        // services.AddTransient<IAlbumRepository, AlbumRepository>();
        
        return services;
    }
}