using Polaris.Core.Models;

namespace Polaris.Core.Interfaces;

public interface ISettingsService
{
    AppSettings Current { get; }
    
    Task SaveAsync(AppSettings settings);
    
    Task<AppSettings> LoadAsync();
}