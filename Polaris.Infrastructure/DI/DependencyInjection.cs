using Microsoft.Extensions.DependencyInjection;
using Polaris.Api.Client;
using Polaris.Core.Interfaces;
using Polaris.Infrastructure.Factories;
using Polaris.Infrastructure.Services;
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
        services.AddHttpClient();
        services.AddSingleton<ImmichClientFactory>();

        services.AddSingleton<ImmichApiClient>(sp =>
        {
            var settings = sp.GetRequiredService<ISettingsService>();
            var factory = sp.GetRequiredService<ImmichClientFactory>();

            return factory.Create(settings.Current.ServerUrl, settings.Current.ApiKey);
        });

        services.AddTransient<IAuthService, AuthService>();
        // services.AddTransient<IAssetRepository, AssetRepository>();
        // services.AddTransient<IAlbumRepository, AlbumRepository>();

        return services;
    }
}