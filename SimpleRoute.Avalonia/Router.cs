using Microsoft.Extensions.DependencyInjection;
using SimpleRoute.Avalonia.Interfaces;
using SimpleRoute.Avalonia.Services;

namespace SimpleRoute.Avalonia;

public class Router<TViewModelBase> where TViewModelBase : class, IRoutePage
{
    private readonly IServiceProvider _serviceProvider;
    private TViewModelBase _currentViewModel = default!;

    public event Action<TViewModelBase>? CurrentViewModelChanged;

    public Router(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    protected TViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            if (_currentViewModel == value) return;
            _currentViewModel = value;
            CurrentViewModelChanged?.Invoke(value);
        }
    }

    public virtual T GoTo<T>(string url, object? body = null) where T : class, TViewModelBase, IRoutePage
    {
        var vm = ResolveViewModel<T>(url, body);
        CurrentViewModel = vm;
        return vm;
    }

    protected T ResolveViewModel<T>(string url, object? body) where T : class, TViewModelBase, IRoutePage
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Route cannot be null or empty.", nameof(url));

        var normalizedUrl = url.StartsWith("app://", StringComparison.OrdinalIgnoreCase)
            ? url
            : "app://" + url;

        var routerMapper = _serviceProvider.GetRequiredService<RouterMapper>();
        var vm = routerMapper.Resolve(normalizedUrl, body) as T
                 ?? throw new InvalidOperationException($"Route '{normalizedUrl}' could not be resolved.");

        return vm;
    }
}