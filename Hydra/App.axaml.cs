using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DryIoc;
using Hydra.IoC;
using Hydra.IoC.Configure;
using Hydra.ViewModels;
using Hydra.ViewModels.Download;
using Hydra.ViewModels.Shared;
using SimpleRoute.Avalonia;

namespace Hydra;

public partial class App : Application
{
    public static IContainer? Container { get; private set; }
    public static RouterHistoryManager<ViewModelBasePage> Router { get; private set; }
    private MainWindow? _mainWindow;
    private TrayIcon? _trayIcon;
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        var builder = HydraIoC.NewBuilder();

        builder.Services.
            AddServices();

        Container = builder.Build();
        
        Router = Container.Resolve<RouterHistoryManager<ViewModelBasePage>>();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            _mainWindow = new MainWindow();

            desktop.MainWindow = _mainWindow;

            ConfigureTrayIcon();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void ConfigureTrayIcon()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var nativeMenu = new NativeMenu();
            
            nativeMenu.Items.Add(CreateMenuItem("Início", () => GoTo<HomeViewModel>()));
            nativeMenu.Items.Add(CreateMenuItem("Downloads", () => GoTo<DownloadViewModel>()));
            nativeMenu.Items.Add(new NativeMenuItemSeparator());
            nativeMenu.Items.Add(CreateMenuItem("Saír do Hydra", () => desktop.Shutdown()));
            
            _trayIcon = new TrayIcon()
            {
                ToolTipText = "Hydra Launcher Lite",
                Icon = new WindowIcon("Assets/icon.png"),
                Menu = nativeMenu,
                IsVisible = true
            };

            _trayIcon.Clicked += (_, _) =>
            {
                if (_mainWindow!.IsVisible)
                {
                    _mainWindow.Hide();
                    return;
                }
                    
                _mainWindow.Show();
                _mainWindow.Activate();
            };
        }
        
        NativeMenuItem CreateMenuItem(string title, Action? func = null)
        {
            var menuItem = new NativeMenuItem(title);
            
            if(func != null)
                menuItem.Click += (_, _) => func();

            return menuItem;
        }

        void GoTo<T>() where T : ViewModelBasePage
        {
            if (!_mainWindow!.IsVisible)
            {
                _mainWindow.Show();
                _mainWindow.Activate();
            }
            
            Router.GoTo<T>("home");
        }
    }
    
    
}