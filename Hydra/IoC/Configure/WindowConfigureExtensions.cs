using Avalonia.Controls;
using Hydra.Pages;
using Hydra.Pages.Catalog;
using Hydra.Pages.Download;
using Hydra.Pages.Settings;
using Hydra.ViewModels;
using Hydra.ViewModels.Catalog;
using Hydra.ViewModels.Download;
using Hydra.ViewModels.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Hydra.IoC.Configure;

public static class WindowConfigureExtensions
{
    public static IServiceCollection AddAllControls(this IServiceCollection services)
    {
        services
            .AddControl<MainWindow>()
            .AddControl<HomePage, HomeViewModel>()
            .AddControl<CatalogPage, CatalogViewModel>()
            .AddControl<DownloadPage, DownloadViewModel>()
            .AddControl<SettingsPage, SettingsViewModel>();
        
        return services;
    }

    public static IServiceCollection AddControl<TControl, TModel>(this IServiceCollection services) where TControl : Control, new() where TModel : class
    {
        services.AddTransient<TModel>();
        services.AddTransient<TControl>(x => new TControl()
        {
            DataContext = x.GetRequiredService<TModel>()
        });
        
        return services;
    }
    
    public static IServiceCollection AddControl<TControl>(this IServiceCollection services) where TControl : Control
    {
        services.AddTransient<TControl>();
        
        return services;
    }
}