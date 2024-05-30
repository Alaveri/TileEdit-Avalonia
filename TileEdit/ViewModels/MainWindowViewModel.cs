using Alaveri.Localization;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using TileEdit.Common;
using TileEdit.Models;
using TileEdit.Views;

namespace TileEdit.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
{
    private NewTilesetDialog? NewTilesetDialog { get; set; } 

    public ICommand ExitCommand { get; } 

    public ICommand NewCommand { get; }

    public ICommand OpenCommand { get; }

    public ObservableCollection<ITilesetEditorViewModel> TilesetEditors { get; } = [];

    private void AddNewTileset(ITileset tileset)
    {
        var editor = new TilesetEditorViewModel(tileset, "Untitled.tls", Configuration);
        TilesetEditors.Add(editor);
    }

    public async Task NewTileset(Window? window)
    {
        if (window == null)
            return;
        NewTilesetDialog = new NewTilesetDialog();
        var tileset = await NewTilesetDialog.ShowDialog<ITileset?>(window!);
        if (tileset != null)
            AddNewTileset(tileset);
        NewTilesetDialog = null;
    }

    public void OpenTileset()
    {
        Debug.WriteLine("Open tileset");
    }

    public void Close(Window? window)
    {
        window?.Close();
    }

    public ILanguageTranslator Translator { get; }

    public IUserConfiguration Configuration { get;}

    public void WindowInitialized(MainWindow window)
    {
        if (Design.IsDesignMode)
            return;
        Configuration.RestoreWindowState("MainWindow", window);
    }

    public void WindowClosing(MainWindow window)
    {
        if (Design.IsDesignMode)
            return;
        Configuration.StoreWindowState("MainWindow", window);
        Configuration.Save();
    }

    public MainWindowViewModel(ILanguageTranslator translator, IUserConfiguration configuration)
    {
        Translator = translator;
        NewCommand = new AsyncRelayCommand<Window>(NewTileset);
        ExitCommand = new RelayCommand<Window>(Close);
        OpenCommand = new RelayCommand(OpenTileset);
        Configuration = configuration;
    } 

    public MainWindowViewModel() : 
        this(
            new LanguageTranslator(new ResourceLanguageDataSource(Properties.en_US.ResourceManager)),
            new UserConfiguration()
        )
    {
    }
}
