using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Hydra.Extensions.IoC;

public class HydraIoC
{
    public IServiceCollection Services = new ServiceCollection();
    
    public static HydraIoC NewBuilder()
        => new HydraIoC();

    public IContainer Build()
    {
        var DryIoc = new Container()
            .WithDependencyInjectionAdapter(Services);
        
        DryIoc.Container.Populate(Services);

        return DryIoc.Container;
    }
}