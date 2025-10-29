using Hydra.Extensions.IoC.Configure.Api;
using Microsoft.Extensions.DependencyInjection;

namespace Hydra.Extensions.IoC.Configure;

public static class ServicesConfigureExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddMemoryCache()
            .AddStores()
            .AddRouterAvalonia()
            .AddAllControls()
            .AddRefitConfigure();
        
        return services;
    }
}