using Hydra.Domain.Models.Internal.Details;
using Hydra.Domain.Models.Steam;
using Hydra.Infrastructure.Extensions;
using Hydra.Infrastructure.Interfaces.Refit;
using SteamStorefrontAPI;

namespace Hydra.Infrastructure.Services.Steam;

public class SteamStore
{
    private readonly ISteamStore _store;
    private readonly IProtonDb _protonDb;

    public SteamStore(ISteamStore store, IProtonDb protonDb)
    {
        _store = store;
        _protonDb = protonDb;
    }

    public async Task<Details?> GetAppDetailsAsync(uint appid)
    {
        var result = await AppDetails.GetAsync((int)appid);
        
        var proton = await _protonDb.GetCompatibilityProton((int)appid);
        
        var response = new Details(result, proton.Content);
 

        return response;
    }

    
    public async Task SearchAsync(string query, int page, SteamLanguages language = SteamLanguages.Undefined)
    {
        var responseSteam = await _store.SearchSteamApps(query, page, 50, language.ToQuery());
    }
    
}