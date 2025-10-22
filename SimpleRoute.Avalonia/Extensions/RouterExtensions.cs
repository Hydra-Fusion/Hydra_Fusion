using Microsoft.Extensions.DependencyInjection;
using SimpleRoute.Avalonia.Configuration;
using SimpleRoute.Avalonia.Interfaces;
using SimpleRoute.Avalonia.Services;

namespace SimpleRoute.Avalonia.Extensions;

public static class RouterExtensions
{
    public static IServiceCollection AddAvaloniaRouter<ViewModelBase>(this IServiceCollection services, Action<RouterConfig> options) where ViewModelBase : class, IRoutePage
    {
        var config = new RouterConfig();
        
        options(config);
        
        services.AddSingleton(config);
        services.AddSingleton<RouterMapper>();
        services.AddSingleton<RouterHistoryManager<ViewModelBase>>();
        
        
        return services;
    }
}