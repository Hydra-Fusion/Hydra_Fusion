using Hydra.ViewModels;
using Hydra.ViewModels.Catalog;
using Hydra.ViewModels.Download;
using Hydra.ViewModels.Settings;
using Hydra.ViewModels.Shared;
using Microsoft.Extensions.DependencyInjection;
using SimpleRoute.Avalonia.Extensions;

namespace Hydra.IoC.Configure;

public static class RouterExtensions
{
    public static IServiceCollection AddRouterAvalonia(this IServiceCollection services)
    {
        /*
        services.AddSingleton<HistoryRouter<ViewModelBase>>(s => 
            new HistoryRouter<ViewModelBase>(t => 
                (ViewModelBase)s.GetRequiredService(t)));
        */

        services.AddAvaloniaRouter<ViewModelBasePage>(options =>
        {
            options.Register<HomeViewModel>("home");
            options.Register<CatalogViewModel>("catalog");
            options.Register<DownloadViewModel>("download");
            options.Register<SettingsViewModel>("settings");
        });
        
        return services;
    }
}