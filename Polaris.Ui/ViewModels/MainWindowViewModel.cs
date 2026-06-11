using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IconPacks.Avalonia.Material;
using Polaris.Core.Interfaces;
using Polaris.Core.Models;
using Polaris.Ui.Interfaces;
using Polaris.Ui.Models;

namespace Polaris.Ui.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly ISettingsService _settingsService;

    [ObservableProperty]
    public partial ViewModelBase? CurrentPage { get; set; }

    [ObservableProperty]
    public partial bool IsShellVisible { get; set; }

    [ObservableProperty]
    public partial bool AlbumsExpanded { get; set; }

    [ObservableProperty]
    public partial string UserServer { get; set; } = string.Empty;

    // Mock user per the design prototype — replaced once /users/me is wired up.
    public string UserName => "Ben Geba";
    public string UserInitials => "BG";

    // Storage card mock values from the design (Sidebar.jsx).
    public string StorageText => "604.7 GiB of 1.8 TiB used";
    public double StoragePercent => 604.7 / (1.8 * 1024) * 100;

    public IReadOnlyList<NavigationItem> TopItems { get; }
    public IReadOnlyList<NavigationItem> LibraryUpperItems { get; }
    public IReadOnlyList<NavigationItem> LibraryLowerItems { get; }
    public IReadOnlyList<SubAlbum> SubAlbums { get; }

    private readonly NavigationItem _albumsItem;

    public MainWindowViewModel(INavigationService navigationService, ISettingsService settingsService)
    {
        _navigationService = navigationService;
        _settingsService = settingsService;

        AlbumsExpanded = true;

        TopItems =
        [
            new NavigationItem("Photos", PackIconMaterialKind.Image, typeof(TimelineViewModel)),
            new NavigationItem("Explore", PackIconMaterialKind.Magnify, typeof(ExploreViewModel)),
            new NavigationItem("Map", PackIconMaterialKind.MapOutline, typeof(PlaceholderViewModel),
                "Weltkarte mit Markern für jedes Foto, das Standortdaten enthält."),
            new NavigationItem("Sharing", PackIconMaterialKind.AccountGroupOutline, typeof(PlaceholderViewModel),
                "Geteilte Alben und Links — von dir geteilt und mit dir geteilt."),
        ];

        _albumsItem = new NavigationItem("Albums", PackIconMaterialKind.ImageMultipleOutline,
            typeof(AlbumsViewModel), isExpandable: true);

        LibraryUpperItems =
        [
            new NavigationItem("Favorites", PackIconMaterialKind.HeartOutline, typeof(PlaceholderViewModel),
                "Fotos und Videos, die du als Favorit markiert hast."),
            _albumsItem,
        ];

        LibraryLowerItems =
        [
            new NavigationItem("Tags", PackIconMaterialKind.TagMultipleOutline, typeof(PlaceholderViewModel),
                "Manuelle und KI-generierte Tags zum schnellen Filtern deiner Bibliothek."),
            new NavigationItem("Utilities", PackIconMaterialKind.BriefcaseOutline, typeof(PlaceholderViewModel),
                "Duplikat-Suche, Hilfsmittel und experimentelle Werkzeuge."),
            new NavigationItem("Archive", PackIconMaterialKind.ArchiveOutline, typeof(PlaceholderViewModel),
                "Archivierte Inhalte. Sie tauchen nicht in der Timeline auf."),
            new NavigationItem("Locked Folder", PackIconMaterialKind.LockOutline, typeof(PlaceholderViewModel),
                "Privater, mit PIN geschützter Bereich für sensible Fotos."),
            new NavigationItem("Trash", PackIconMaterialKind.TrashCanOutline, typeof(PlaceholderViewModel),
                "Gelöschte Inhalte werden nach 30 Tagen automatisch endgültig entfernt."),
        ];

        // Mock sub-albums per the design (Sidebar.jsx).
        SubAlbums =
        [
            new SubAlbum("Produktbilder", MockGradients.Diagonal("#cfc4a8", "#8a7a5a")),
            new SubAlbum("Haus Sachv…", MockGradients.Diagonal("#5a7a9a", "#2a4a6a")),
            new SubAlbum("Hochzeit Ch…", MockGradients.Diagonal("#6a8a4a", "#2a4a1a")),
        ];
    }

    private IEnumerable<NavigationItem> AllItems =>
        TopItems.Concat(LibraryUpperItems).Concat(LibraryLowerItems);

    [RelayCommand]
    private void Navigate(NavigationItem item)
    {
        if (ReferenceEquals(item, _albumsItem))
        {
            AlbumsExpanded = !AlbumsExpanded;
        }

        SetActive(item);
        _navigationService.NavigateTo(item.ViewModelType);

        if (CurrentPage is PlaceholderViewModel placeholder && item.PlaceholderMessage is not null)
        {
            placeholder.Title = item.Title;
            placeholder.Icon = item.Icon;
            placeholder.Message = item.PlaceholderMessage;
        }
    }

    [RelayCommand]
    private void SelectSubAlbum(SubAlbum album)
    {
        SetActive(null);
        foreach (var sub in SubAlbums)
        {
            sub.IsActive = ReferenceEquals(sub, album);
        }

        _navigationService.NavigateTo(typeof(AlbumsViewModel));
    }

    [RelayCommand]
    private void OpenSettings()
    {
        SetActive(null);
        _navigationService.NavigateTo<SettingsViewModel>();
    }

    [RelayCommand]
    private async System.Threading.Tasks.Task LogoutAsync()
    {
        await _settingsService.SaveAsync(new AppSettings { ServerUrl = _settingsService.Current.ServerUrl });
        _navigationService.NavigateTo<LoginViewModel>();
    }

    /// <summary>
    /// Keeps the sidebar's active state in sync when navigation is triggered
    /// outside the sidebar (e.g. app start, login).
    /// </summary>
    public void SyncActiveNav(Type viewModelType)
    {
        if (viewModelType == typeof(PlaceholderViewModel) || viewModelType == typeof(SettingsViewModel))
        {
            return;
        }

        SetActive(AllItems.FirstOrDefault(i => i.ViewModelType == viewModelType));
    }

    private void SetActive(NavigationItem? item)
    {
        foreach (var nav in AllItems)
        {
            nav.IsActive = ReferenceEquals(nav, item);
        }

        foreach (var sub in SubAlbums)
        {
            sub.IsActive = false;
        }
    }

    partial void OnIsShellVisibleChanged(bool value)
    {
        if (!value)
        {
            return;
        }

        UserServer = Uri.TryCreate(_settingsService.Current.ServerUrl, UriKind.Absolute, out var uri)
            ? uri.Host
            : _settingsService.Current.ServerUrl;
    }
}
