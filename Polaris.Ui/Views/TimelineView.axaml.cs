using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Polaris.Ui.Models;
using Polaris.Ui.ViewModels;

namespace Polaris.Ui.Views;

public partial class TimelineView : UserControl
{
    public TimelineView()
    {
        InitializeComponent();
    }

    private TimelineViewModel? ViewModel => DataContext as TimelineViewModel;

    private void Thumb_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Control { DataContext: TimelineThumb thumb } && ViewModel is { } vm)
        {
            vm.HandleThumbClick(thumb, e.KeyModifiers.HasFlag(KeyModifiers.Shift));
            e.Handled = true;
        }
    }

    private void ScrubMonth_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Control { DataContext: TimelineMonth month } || ViewModel is not { } vm)
        {
            return;
        }

        vm.SetActiveMonth(month);

        var index = -1;
        for (var i = 0; i < vm.Months.Count; i++)
        {
            if (ReferenceEquals(vm.Months[i], month))
            {
                index = i;
                break;
            }
        }

        if (index >= 0 && MonthsHost.ContainerFromIndex(index) is { } container)
        {
            container.BringIntoView();
        }
    }

    private void Scroller_ScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        if (ViewModel is not { } vm)
        {
            return;
        }

        // The first month group whose bottom edge is still below the top of the
        // viewport (with a small margin) is the active one.
        for (var i = 0; i < vm.Months.Count; i++)
        {
            if (MonthsHost.ContainerFromIndex(i) is not { } container)
            {
                continue;
            }

            var bottom = container.TranslatePoint(new Point(0, container.Bounds.Height), Scroller);
            if (bottom is { Y: > 60 })
            {
                vm.SetActiveMonth(vm.Months[i]);
                return;
            }
        }
    }
}
