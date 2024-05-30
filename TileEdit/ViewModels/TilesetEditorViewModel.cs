using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEdit.Common;
using TileEdit.Models;

namespace TileEdit.ViewModels;

public class TilesetEditorViewModel(ITileset tileset, string filename, IUserConfiguration configuration) : ViewModelBase, ITilesetEditorViewModel
{
    public IUserConfiguration Configuration { get; } = configuration;

    public ITileset Tileset { get; } = tileset;

    public IImageEditorViewModel ImageEditor { get; } = new ImageEditorViewModel();

    public GridLength LeftPanelWidth
    {
        get => new(Configuration.LeftPanelWidth);
        set => Configuration.LeftPanelWidth = value.Value;
    }

    public GridLength RightPanelWidth
    {
        get => new(Configuration.RightPanelWidth);
        set => Configuration.RightPanelWidth = value.Value;
    }

    public string Filename { get; set; } = filename;

    public string DisplayName => Path.GetFileName(Filename);

    public TilesetEditorViewModel() : this(new Tileset(), string.Empty, new UserConfiguration())
    {
        //Configuration.LeftPanelWidth = window.MainGrid.ColumnDefinitions[0].Width.Value;
        //Configuration.RightPanelWidth = window.MainGrid.ColumnDefinitions[4].Width.Value;
        //window.MainGrid.ColumnDefinitions[0].Width = new GridLength(Configuration.LeftPanelWidth);
        //window.MainGrid.ColumnDefinitions[4].Width = new GridLength(Configuration.RightPanelWidth);

    }
}
