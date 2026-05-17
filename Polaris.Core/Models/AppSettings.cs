namespace Polaris.Core.Models;

public class AppSettings
{
    public string ServerUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public AppTheme Theme { get; set; } = AppTheme.System;
    public ThumbnailSize ThumbnailSize { get; set; } = ThumbnailSize.Medium;
    public int MaxParallelUploads { get; set; } = 4;
    
    public bool IsConfigured => !string.IsNullOrEmpty(ServerUrl) && !string.IsNullOrEmpty(ApiKey);
}

public enum AppTheme
{
    System,
    Light,
    Dark
}

public enum ThumbnailSize
{
    Small,
    Medium,
    Large
}