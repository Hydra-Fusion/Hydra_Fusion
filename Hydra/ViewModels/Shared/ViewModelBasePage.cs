using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using DryIoc;
using SimpleRoute.Avalonia;
using SimpleRoute.Avalonia.Context;
using SimpleRoute.Avalonia.Interfaces;

namespace Hydra.ViewModels.Shared;

public abstract class ViewModelBasePage : ObservableObject, IRoutePage
{
    protected IServiceProvider ServiceProvider { get; private set; } = Hydra.App.Container.Resolve<IServiceProvider>();
    protected RouterHistoryManager<ViewModelBasePage> Router { get; private set; } = Hydra.App.Container.Resolve<RouterHistoryManager<ViewModelBasePage>>();

    public virtual void OnNavigatedTo(NavigationContext context) { }

    public virtual Task OnNavigatedToAsync(NavigationContext context)
        => Task.CompletedTask;
}