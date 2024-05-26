using Alaveri.Localization;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using TileEdit.Common;
using TileEdit.Views;

namespace TileEdit.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
{
    private NewTilesetDialog? NewTilesetDialog { get; set; } 

    public ICommand ExitCommand { get; } 

    public ICommand NewCommand { get; }

    public ICommand OpenCommand { get; }

    public async Task NewTileset(Window? window)
    {
        NewTilesetDialog = new NewTilesetDialog();
        await NewTilesetDialog.ShowDialog(window!);
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
        window.MainGrid.ColumnDefinitions[0].Width = new GridLength(Configuration.LeftPanelWidth);
        window.MainGrid.ColumnDefinitions[4].Width = new GridLength(Configuration.RightPanelWidth);
    }

    public void WindowClosing(MainWindow window)
    {
        if (Design.IsDesignMode)
            return;
        Configuration.StoreWindowState("MainWindow", window);
        Configuration.LeftPanelWidth = window.MainGrid.ColumnDefinitions[0].Width.Value;
        Configuration.RightPanelWidth = window.MainGrid.ColumnDefinitions[4].Width.Value;
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
