using Alaveri.Localization;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TileEdit.Common;
using TileEdit.Models;

namespace TileEdit.ViewModels;

public interface INewTilesetDialogViewModel
{
    IUserConfiguration Configuration { get; }
    string Title { get; }
    ILanguageTranslator Translator { get; }
    ObservableCollection<TilesetDimensions> TilesetTypes { get; }
    ObservableCollection<TilesetPixelFormat> PixelFormats { get; }
    TilesetDimensions SelectedTilesetType { get; }
    TilesetPixelFormat SelectedPixelFormat { get; }
    ICommand OkCommand { get; }
    ICommand CancelCommand { get; }
    bool IsFixedSize { get; }
    int TileWidth { get; set; }
    int TileHeight { get; set; }
}