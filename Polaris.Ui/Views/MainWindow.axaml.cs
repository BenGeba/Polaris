using Avalonia.Controls;
using Polaris.Ui.ViewModels;

namespace Polaris.Ui.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        viewModel.Initialize();
    }
}