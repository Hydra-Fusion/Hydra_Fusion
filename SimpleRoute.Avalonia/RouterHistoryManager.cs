using SimpleRoute.Avalonia.Interfaces;

namespace SimpleRoute.Avalonia;

public class RouterHistoryManager<TViewModelBase> : Router<TViewModelBase> where TViewModelBase : class, IRoutePage
{
    private int _historyIndex = -1;
    
    private List<TViewModelBase> _history = new();
    private List<string> _titles = new();
    
    protected readonly uint _historyMaxSize = 50;
    private string CurrentRouter = "";

    public event Action<string>? CurrentTitleChanged;
    
    public bool HasNext => _history.Count > 0 && _historyIndex < _history.Count - 1;
    public bool HasPrev => _historyIndex > -1;
    
    public IReadOnlyCollection<TViewModelBase> History => _history.AsReadOnly();

    public RouterHistoryManager(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        
    }
    
    public TViewModelBase? GetHistoryItem(int offset, out string title)
    {
        title = "";
        
        var newIndex = _historyIndex + offset;
        if (newIndex < 0 || newIndex > _history.Count - 1)
        {
            return default;
        }

        title = _titles.ElementAt(newIndex);
        return _history.ElementAt(newIndex);
    }
    
    public void Push(TViewModelBase item, string title = "")
    {
        if (HasNext)
        {
            _history = _history.Take(_historyIndex + 1).ToList();
            _titles =  _titles.Take(_historyIndex + 1).ToList();
        }
        _titles.Add(title);
        _history.Add(item);
        _historyIndex = _history.Count - 1;
        if (_history.Count > _historyMaxSize)
        {
            _titles.RemoveAt(0);
            _history.RemoveAt(0);
            
            _historyIndex--;
        }
    }
    
    public TViewModelBase? Go(int offset = 0)
    {
        if (offset == 0)
        {
            return default;
        }
        
        var viewModel = GetHistoryItem(offset, out var title);
        if (viewModel == null)
        {
            return default;
        }

        _historyIndex += offset;
        CurrentViewModel = viewModel;
        
        CurrentTitleChanged.Invoke(title);
        
        return viewModel;
    }
    
    public TViewModelBase? Back() => HasPrev ? Go(-1) : default;
    
    public TViewModelBase? Forward() => HasNext ? Go(1) : default;
    
    public override T GoTo<T>(string url, object body = null)
    {
        var destination = ResolveViewModel<T>(url, body);

        if (CurrentRouter == url)
            return destination;
        
        CurrentViewModel = destination;
        CurrentRouter = url;
        
        CurrentTitleChanged.Invoke("");
        
        Push(destination);
        return destination;
    }
    
    public T GoTo<T>(string url, string title, object body = null) where T : class, TViewModelBase, IRoutePage
    {
        var destination = ResolveViewModel<T>(url, body);

        if (CurrentRouter == url)
            return destination;
        
        CurrentViewModel = destination;
        CurrentRouter = url;
        
        CurrentTitleChanged.Invoke(title);
        
        Push(destination, title);
        return destination;
    }
}