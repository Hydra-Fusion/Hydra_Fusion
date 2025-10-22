using System;
using Microsoft.Extensions.DependencyInjection;

namespace Hydra.IoC.Configure;

public static class ServicesConfigureExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        /*services.AddHttpClient("Hydra-Library", client =>
        {
            client.Timeout = TimeSpan.FromSeconds(10);
            client.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (X11; Linux x86_64; rv:143.0) Gecko/20100101 Firefox/143.0"
            );
        });*/
        
        
        services
            .AddRouterAvalonia()
            .AddAllControls();
        
        return services;
    }
}