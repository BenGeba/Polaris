using System;
using Polaris.Ui.ViewModels;

namespace Polaris.Ui.Interfaces;

public interface INavigationService
{
    void NavigateTo<TViewModel>() where TViewModel : ViewModelBase;
    void NavigateTo(Type viewModelType);
}