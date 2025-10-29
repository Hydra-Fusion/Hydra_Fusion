using Hydra.Domain.Models.Hydra.Api;
using Refit;

namespace Hydra.Infrastructure.Interfaces.Refit;

public interface IHydra
{
    [Get("/catalogue/featured")]
    Task<ApiResponse<ICollection<FeaturedModel>>> FeatureAsync();
    
    [Get("/catalogue/hot")]
    Task<ApiResponse<List<HotGameModel>>> HotGameAsync([Query] int take = 12, [Query] int skip = 0);
    
    [Post("/catalogue/search")]
    Task<ApiResponse<SearchHydraResponse>> SearchAsync([Body] SearchHydraRequest request);
}