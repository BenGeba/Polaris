using CommunityToolkit.Mvvm.ComponentModel;
using IconPacks.Avalonia.Material;

namespace Polaris.Ui.ViewModels;

/// <summary>
/// Empty state for nav targets that aren't built yet (Map, Sharing, Favorites, …).
/// Title/icon/message are set by the shell from the selected NavigationItem.
/// </summary>
public partial class PlaceholderViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial string Title { get; set; } = string.Empty;

    [ObservableProperty]
    public partial PackIconMaterialKind Icon { get; set; }

    [ObservableProperty]
    public partial string Message { get; set; } = string.Empty;
}
