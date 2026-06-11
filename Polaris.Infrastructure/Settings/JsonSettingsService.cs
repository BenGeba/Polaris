using System.Text.Json;
using System.Text.Json.Serialization;
using Polaris.Core.Interfaces;
using Polaris.Core.Models;

namespace Polaris.Infrastructure.Settings;

public class JsonSettingsService : ISettingsService
{
    private static readonly string SettingsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "Polaris",
        "settings.json");
    
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };


    public AppSettings Current { get; private set; } = new();
    
    public async Task SaveAsync(AppSettings settings)
    {
        var directory = Path.GetDirectoryName(SettingsPath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        
        await using var stream = File.Create(SettingsPath);
        await JsonSerializer.SerializeAsync(stream, settings, JsonSerializerOptions).ConfigureAwait(false);
        
        Current = settings;
    }

    public async Task<AppSettings> LoadAsync()
    {
        if (!File.Exists(SettingsPath))
        {
            Current = new AppSettings();
            return Current;
        }

        try
        {
            await using var stream = File.OpenRead(SettingsPath);
            Current = await JsonSerializer.DeserializeAsync<AppSettings>(stream, JsonSerializerOptions)
                          .ConfigureAwait(false) ??
                      new AppSettings();
        }
        catch (Exception e)
        {
            Current = new AppSettings();
        }

        return Current;
    }
}