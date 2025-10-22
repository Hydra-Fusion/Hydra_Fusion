namespace Hydra.Infrastructure.Services.Hydra;

public class HydraServices
{
    public FontsManager Sources { get; set; }

    public HydraServices(FontsManager sources)
    {
        Sources = sources; 
    }
}