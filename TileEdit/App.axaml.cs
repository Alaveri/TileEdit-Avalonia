using Alaveri.Configuration;
using Alaveri.Localization;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using TileEdit.Common;
using TileEdit.ViewModels;
using TileEdit.Views;

namespace TileEdit;

public partial class App : Application
{
    protected IServiceProvider ServiceProvider { get; private set; }

    public App()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();
    }

    protected static void ConfigureServices(IServiceCollection services)
    {
        services
            .AddSingleton<IUserConfiguration>(Configuration.Load<UserConfiguration>(ConfigurationType.User, "Alaveri", "TileEdit"))
            .AddSingleton<IMainWindowViewModel, MainWindowViewModel>()
            .AddSingleton<ILanguageTranslator>(new LanguageTranslator(new ResourceLanguageDataSource(Properties.en_US.ResourceManager)));
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = 
                new MainWindow(ServiceProvider.GetService<IMainWindowViewModel>() ?? new MainWindowViewModel());
        }
        base.OnFrameworkInitializationCompleted();
    }
}
