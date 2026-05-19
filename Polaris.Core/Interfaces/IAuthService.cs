namespace Polaris.Core.Interfaces;

public interface IAuthService
{
    Task<bool> ValidateApiKeyAsync(string serverUrl, string apiKey, CancellationToken cancellationToken = default);
}