using System;
using Hydra.Infrastructure.Interfaces.Refit;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Hydra.Extensions.IoC.Configure.Api;

public static class ApiConfigureExtensions
{
    public static IServiceCollection AddRefitConfigure(this IServiceCollection service)
    {
        service.AddRefitClient<IHydra>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://hydra-api-us-east-1.losbroxas.org"));
        
        service.AddRefitClient<IGogStore>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://catalog.gog.com"));

        service.AddRefitClient<IProtonDb>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://www.protondb.com"));
        
        return service;
    }
}