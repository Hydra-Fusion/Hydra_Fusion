using SimpleRoute.Avalonia.Interfaces;

namespace SimpleRoute.Avalonia.Configuration;

public class RouterConfig
{
    private readonly Dictionary<string, Type> _routes = new Dictionary<string, Type>();
    
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

    internal Dictionary<string, Type> GetMapRoute()
        => _routes;
}