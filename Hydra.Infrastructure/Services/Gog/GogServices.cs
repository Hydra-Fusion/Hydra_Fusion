namespace Hydra.Infrastructure.Services.Gog;

public class GogServices
{
    public GogStore Store { get; set; }

    public GogServices(GogStore store)
    {
        Store = store;
    }
}