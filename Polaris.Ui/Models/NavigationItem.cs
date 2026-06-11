using System;
using CommunityToolkit.Mvvm.ComponentModel;
using IconPacks.Avalonia.Material;

namespace Polaris.Ui.Models;

/// <summary>
/// A sidebar navigation entry. Items whose target is the shared
/// <see cref="ViewModels.PlaceholderViewModel"/> carry their empty-state
/// text in <see cref="PlaceholderMessage"/>.
/// </summary>
public partial class NavigationItem(
    string title,
    PackIconMaterialKind icon,
    Type viewModelType,
    string? placeholderMessage = null,
    bool isExpandable = false) : ObservableObject
{
    public string Title { get; } = title;
    public PackIconMaterialKind Icon { get; } = icon;
    public Type ViewModelType { get; } = viewModelType;
    public string? PlaceholderMessage { get; } = placeholderMessage;
    public bool IsExpandable { get; } = isExpandable;

    [ObservableProperty]
    public partial bool IsActive { get; set; }
}
