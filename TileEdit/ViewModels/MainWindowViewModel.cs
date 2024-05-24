using Alaveri.Localization;
using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.Windows.Input;
using TileEdit.Common;
using TileEdit.Views;

namespace TileEdit.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
{
    public ICommand ExitCommand { get; } 

    public ICommand NewCommand { get; }

    public ICommand OpenCommand { get; }

    public void NewTileset()
    {
        Debug.WriteLine("New tileset");
    }

    public void OpenTileset()
    {
        Debug.WriteLine("Open tileset");
    }

    public void Close(Window window)
    {
        window?.Close();
    }

    public ILanguageTranslator Translator { get; }

    public IUserConfiguration UserConfiguration { get;}

    public void WindowInitialized(MainWindow window)
    {
        if (Design.IsDesignMode)
            return;
        UserConfiguration.RestoreWindowState("MainWindow", window);
        window.MainGrid.ColumnDefinitions[0].Width = new GridLength(UserConfiguration.LeftPanelWidth);
        window.MainGrid.ColumnDefinitions[4].Width = new GridLength(UserConfiguration.RightPanelWidth);
    }

    public void WindowClosing(MainWindow window)
    {
        if (Design.IsDesignMode)
            return;
        UserConfiguration.StoreWindowState("MainWindow", window);
        UserConfiguration.LeftPanelWidth = window.MainGrid.ColumnDefinitions[0].Width.Value;
        UserConfiguration.RightPanelWidth = window.MainGrid.ColumnDefinitions[4].Width.Value;
        UserConfiguration.Save();
    }

    public MainWindowViewModel(ILanguageTranslator translator, IUserConfiguration configuration)
    {
        Translator = translator;
        NewCommand = ReactiveCommand.Create(NewTileset);
        ExitCommand = ReactiveCommand.Create<Window>(Close);
        OpenCommand = ReactiveCommand.Create(OpenTileset);
        UserConfiguration = configuration;
    } 

    public MainWindowViewModel() : 
        this(
            new LanguageTranslator(new ResourceLanguageDataSource(Properties.en_US.ResourceManager)),
            new UserConfiguration()
        )
    {
    }
}
