using Avalonia.Controls;
using TileEdit.Common;
using TileEdit.Models;
using TileEdit.ViewModels;

namespace TileEdit.Views;
public partial class TilesetEditor : UserControl
{
    public IUserConfiguration Configuration { get; }

    public TilesetEditor(ITileset tileset, string filename, IUserConfiguration configuration)
    {
        InitializeComponent();
        DataContext = new TilesetEditorViewModel(tileset, filename, configuration);
        Configuration = configuration;
        //MainGrid.ColumnDefinitions[0].Width = MainGrid.ColumnDefinitions[0].Width;
        //MainGrid.ColumnDefinitions[4].Width = MainGrid.ColumnDefinitions[4].Width;
    }

    public TilesetEditor() : this(new Tileset(), string.Empty, new UserConfiguration())
    {
    }
}
