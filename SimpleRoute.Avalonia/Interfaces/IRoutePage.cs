using SimpleRoute.Avalonia.Context;

namespace SimpleRoute.Avalonia.Interfaces;

public interface IRoutePage
{
    public void OnNavigatedTo(NavigationContext context);
    
    public Task OnNavigatedToAsync(NavigationContext context);
}
