using Alaveri.Localization;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TileEdit.Views;

namespace TileEdit.ViewModels;

public interface IMainWindowViewModel
{
    ICommand ExitCommand { get; }
    ICommand NewCommand { get; }
    ICommand OpenCommand { get; }
    ILanguageTranslator Translator { get; }
    ObservableCollection<ITilesetEditorViewModel> TilesetEditors { get; }

    void WindowClosing(MainWindow window);
    void WindowInitialized(MainWindow window);
}