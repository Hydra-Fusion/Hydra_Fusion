using Hydra_Launcher.Infrastructure.Services.Steam;

namespace Hydra.Infrastructure.Services.Steam;

public class Steam : ISteam
{
    public SteamStore Store { get; set; }
    public SteamUser User { get; set; }

    public Steam(SteamStore store, SteamUser user)
    {
        Store = store;
        User = user;
    }
    
}