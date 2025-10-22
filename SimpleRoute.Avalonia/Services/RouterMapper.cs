using Microsoft.Extensions.DependencyInjection;
using SimpleRoute.Avalonia.Configuration;
using SimpleRoute.Avalonia.Context;
using SimpleRoute.Avalonia.Interfaces;

namespace SimpleRoute.Avalonia.Services;

public class RouterMapper
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<string, Type> _routes;

    public RouterMapper(IServiceProvider serviceProvider, RouterConfig routerConfig)
    {
        _serviceProvider = serviceProvider;
        _routes = routerConfig.GetMapRoute();
    }

    public void Register<TViewModel>(string route) where TViewModel : class, IRoutePage
    {
        if (string.IsNullOrWhiteSpace(route))
            throw new ArgumentException("A URL não pode ser nula ou vazia.", nameof(route));

        var normalizedUrl = route.StartsWith("app://", StringComparison.OrdinalIgnoreCase)
            ? route
            : "app://" + route;

        var uri = new Uri(normalizedUrl);

        var key = $"{uri.Host}{uri.AbsolutePath}";
        _routes[key] = typeof(TViewModel);
    }

    public IRoutePage Resolve(string route, object body)
    {
        if (string.IsNullOrWhiteSpace(route))
            throw new ArgumentException("A URL não pode ser nula ou vazia.", nameof(route));

        var normalizedUrl = route.StartsWith("app://", StringComparison.OrdinalIgnoreCase)
            ? route
            : "app://" + route;

        var uri = new Uri(normalizedUrl);
        var key = $"{uri.Host}{uri.AbsolutePath}";

        if (_routes.TryGetValue(key, out var type))
        {
            var vm = _serviceProvider.GetService(type) as IRoutePage;
            if (vm == null)
                throw new InvalidOperationException($"O tipo {type.Name} não está registrado no container.");
            
            vm.OnNavigatedTo(new NavigationContext(body, normalizedUrl));

            return vm;
        }

        throw new NotSupportedException($"Route '{route}' is not supported.");
    }
}