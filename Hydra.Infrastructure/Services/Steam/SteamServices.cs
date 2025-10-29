using Hydra.Infrastructure.Interfaces.Steam;

namespace Hydra.Infrastructure.Services.Steam;

public class SteamServices : ISteam
{
    public SteamStore Store { get; set; }
    public SteamUser User { get; set; }

    public SteamServices(SteamStore store, SteamUser user)
    {
        Store = store;
        User = user;
    }
    
}