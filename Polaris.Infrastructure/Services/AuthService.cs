using Polaris.Core.Interfaces;
using Polaris.Infrastructure.Factories;

namespace Polaris.Infrastructure.Services;

public class AuthService(ImmichClientFactory clientFactory) : IAuthService
{
    public async Task<bool> ValidateApiKeyAsync(string serverUrl, string apiKey, CancellationToken cancellationToken = default)
    {
        try
        {
            var client = clientFactory.Create(serverUrl, apiKey);
            var user = await client.Users.Me.GetAsync(cancellationToken: cancellationToken);

            return user is not null;
        }
        catch
        {
            return false;
        }
    }
}