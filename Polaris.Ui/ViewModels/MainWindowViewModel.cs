using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using Polaris.Ui.Interfaces;
using Polaris.Ui.Models;

namespace Polaris.Ui.ViewModels;

public partial class MainWindowViewModel(INavigationService navigationService) : ViewModelBase
{
    [ObservableProperty]
    public partial ViewModelBase? CurrentPage { get; set; }

    [ObservableProperty]
    public partial NavigationItem? SelectedNavItem { get; set; }
    
    public IReadOnlyList<NavigationItem> NavigationItems { get; } = 
        [new("Photos", Icon: "CameraIcon", ViewModelType: typeof(TimelineViewModel)), new("Explore", Icon: "LupenIcon", ViewModelType: typeof(ExploreViewModel))];

    public void Initialize()
    {
        SelectedNavItem = NavigationItems[0];
    }

    partial void OnSelectedNavItemChanged(NavigationItem? value)
    {
        if (value != null)
        {
            navigationService.NavigateTo(value.ViewModelType);
        }
    }
}