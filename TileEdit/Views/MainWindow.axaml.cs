using Avalonia.Controls;
using TileEdit.Common;
using TileEdit.ViewModels;

namespace TileEdit.Views;

public partial class MainWindow : Window
{
    public MainWindow(IMainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        viewModel.WindowInitialized(this);
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        var viewModel = (IMainWindowViewModel?)DataContext;
        viewModel?.WindowClosing(this);
        base.OnClosing(e);
    }

    public MainWindow() : this(new MainWindowViewModel())
    {
    }
}