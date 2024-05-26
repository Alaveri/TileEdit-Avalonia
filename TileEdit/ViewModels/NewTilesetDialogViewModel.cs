using Alaveri.Core.Enumerations;
using Alaveri.Core.Enumerations.Extensions;
using Alaveri.Localization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Input;
using TileEdit.Common;
using TileEdit.Models;

namespace TileEdit.ViewModels;

public partial class NewTilesetDialogViewModel(ILanguageTranslator translator, IUserConfiguration configuration) : ViewModelBase, INewTilesetDialogViewModel
{
    public ICommand OkCommand { get; } = new RelayCommand<Window>((window) => window?.Close());

    public ICommand CancelCommand { get; } = new RelayCommand<Window>((window) => window?.Close());

    public ILanguageTranslator Translator { get; } = translator;

    public IUserConfiguration Configuration { get; } = configuration;

    public string Title => Translator.Translate("NewTilesetDialogTitle");

    public ObservableCollection<TilesetDimensions> TilesetTypes { get; } =
        new ObservableCollection<TilesetDimensions>(Enum.GetValues<TilesetDimensions>());

    public ObservableCollection<TilesetPixelFormat> PixelFormats { get; } =
        new ObservableCollection<TilesetPixelFormat>(Enum.GetValues<TilesetPixelFormat>());

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsFixedSize))] 
    private TilesetDimensions selectedTilesetType = TilesetDimensions.FixedSize;

    [ObservableProperty]
    private TilesetPixelFormat selectedPixelFormat = TilesetPixelFormat.Rgba8Bit;

    public bool IsFixedSize => SelectedTilesetType == TilesetDimensions.FixedSize;

    public int TileWidth { get; set; } = 16;

    public int TileHeight { get; set; } = 16;

    public NewTilesetDialogViewModel() :
        this(new LanguageTranslator(new ResourceLanguageDataSource(Properties.en_US.ResourceManager)), new UserConfiguration())
    {
    }
}
