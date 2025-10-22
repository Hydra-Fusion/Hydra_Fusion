using Hydra.Domain.Models.Gog;
using Refit;

namespace Hydra.Infrastructure.Interfaces.Refit;

public interface IGogStore
{
    [Get("/v1/catalog")]
    public Task<ApiResponse<GogSearchResponse>> SearchAsync(
        [AliasAs("query")] string query,
        [AliasAs("page")] int page,
        [AliasAs("limit")] int limit = 50,
        [AliasAs("productType")] string productType = "in:game",
        [AliasAs("order")] string order = "desc:score"
    );
}