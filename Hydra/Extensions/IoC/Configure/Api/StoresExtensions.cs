using Hydra.Infrastructure.Services.Gog;
using Hydra.Infrastructure.Services.Hydra;
using Hydra.Infrastructure.Services.Steam;
using Microsoft.Extensions.DependencyInjection;

namespace Hydra.Extensions.IoC.Configure.Api;

public static class StoresExtensions
{
    public static IServiceCollection AddStores(this IServiceCollection services)
    {
        services.AddSingleton<SteamServices>();
        services.AddSingleton<SteamUser>();
        services.AddSingleton<SteamStore>();
        
        services.AddSingleton<GogServices>();
        services.AddSingleton<GogStore>();

        services.AddSingleton<HydraServices>();
        services.AddSingleton<FontsManager>();

        services.AddHttpClient("Hydra-Library");
        
        return services;
    }
}