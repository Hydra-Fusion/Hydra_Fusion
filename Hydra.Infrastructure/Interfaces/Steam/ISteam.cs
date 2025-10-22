using Hydra.Infrastructure.Services.Steam;

namespace Hydra_Launcher.Infrastructure.Services.Steam;

public interface ISteam
{
    SteamStore Store { get; set; }
    SteamUser User { get; set; }
}