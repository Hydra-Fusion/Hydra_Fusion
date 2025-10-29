using Hydra.Infrastructure.Services.Steam;

namespace Hydra.Infrastructure.Interfaces.Steam;

public interface ISteam
{
    SteamStore Store { get; set; }
    SteamUser User { get; set; }
}