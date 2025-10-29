using Hydra.Domain.Models.Internal.Details;
using Refit;

namespace Hydra.Infrastructure.Interfaces.Refit;

public interface IProtonDb
{
    [Get("/api/v1/reports/summaries/{appId}.json")]
    public Task<ApiResponse<Proton>> GetCompatibilityProton(int appId);
}