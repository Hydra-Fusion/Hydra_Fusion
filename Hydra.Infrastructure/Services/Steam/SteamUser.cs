using GameFinder.RegistryUtils;
using GameFinder.StoreHandlers.Steam;
using GameFinder.StoreHandlers.Steam.Models.ValueTypes;
using Hydra.Domain.Models.Internal.Details;
using NexusMods.Paths;

namespace Hydra.Infrastructure.Services.Steam;

public class SteamUser
{
    private SteamHandler _handler;
    private SteamStore _store;

    public SteamUser(SteamStore store)
    {
        _store = store;
        _handler = new SteamHandler(FileSystem.Shared, OperatingSystem.IsWindows() ? WindowsRegistry.Shared : null);
    }

    public async Task<List<Details>> FindeGames()
    {
        var games = _handler.FindAllGames();

        List<Details> response = new List<Details>();

        foreach (var game in games)
        {
            if(!game.IsT0)
                continue;
            
            var steamGame = game.AsT0;
            
            var path = steamGame.Path.GetFullPath();
            var name = steamGame.Name ?? string.Empty;

            if (name.Contains("Proton", StringComparison.OrdinalIgnoreCase)
                || name.Contains("Steamworks", StringComparison.OrdinalIgnoreCase)
                || name.Contains("Runtime", StringComparison.OrdinalIgnoreCase)
                || name.Contains("SteamVR", StringComparison.OrdinalIgnoreCase)
                || name.Contains("Steam Input", StringComparison.OrdinalIgnoreCase))
                continue;
            
            var details = await _store.GetAppDetailsAsync((uint)game.AsT0.AppId);
            
            if (details != null)
            {
                details.GamaLocation = path;
                
                if(response.FirstOrDefault(x => x.Id == details.Id) == null)
                    response.Add(details);
            }
        }

        return response;
    }

    public async Task<ProtonWinePrefix?> GetProtonPrefix(uint appId)
    {
        SteamGame? steamGame = _handler.FindOneGameById((AppId)appId, out var errors);
        
        if (steamGame is null) return null;

        return steamGame.GetProtonPrefix();
    }
}