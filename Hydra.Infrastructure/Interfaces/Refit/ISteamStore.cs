using Hydra.Domain.Models.Internal.Details;
using Hydra.Domain.Models.Steam;
using Refit;

namespace Hydra.Infrastructure.Interfaces.Refit;

public interface ISteamStore
{
    
    [Get("/ISteamApps/GetAppList/v2/")]
    Task<ApiResponse<AppList>> GetSteamApps();
    
    [Get("/search/results")]
    Task<ApiResponse<string>> SearchSteamApps(
        [Query] string term, 
        [Query] int page, 
        [Query] int count = 50,
        [Query] string supportedlang = "",
        [Query] string category1 = "998");
}

public interface IProtonDb
{
    [Get("/api/v1/reports/summaries/{appId}.json")]
    public Task<ApiResponse<Proton>> GetCompatibilityProton(int appId);
}