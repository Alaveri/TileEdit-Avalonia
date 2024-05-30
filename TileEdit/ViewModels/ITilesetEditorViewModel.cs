using Avalonia.Controls;
using TileEdit.Models;

namespace TileEdit.ViewModels;

public interface ITilesetEditorViewModel
{
    ITileset Tileset { get; }
    string Filename { get; set; }
    string DisplayName { get; }
    GridLength LeftPanelWidth { get; set; }
    GridLength RightPanelWidth { get; set; }
}