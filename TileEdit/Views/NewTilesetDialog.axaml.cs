using Avalonia.Controls;
using Avalonia.Input;
using TileEdit.ViewModels;

namespace TileEdit.Views;
public partial class NewTilesetDialog : Window
{
    public NewTilesetDialog() : this(new NewTilesetDialogViewModel()) { }

    public NewTilesetDialog(INewTilesetDialogViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
