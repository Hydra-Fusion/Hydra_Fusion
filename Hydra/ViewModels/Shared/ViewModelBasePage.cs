using System;
using CommunityToolkit.Mvvm.ComponentModel;
using DryIoc;
using SimpleRoute.Avalonia;
using SimpleRoute.Avalonia.Context;
using SimpleRoute.Avalonia.Interfaces;

namespace Hydra.ViewModels.Shared;

public abstract class ViewModelBasePage : ObservableObject, IRoutePage
{
    protected IServiceProvider ServiceProvider { get; private set; } = App.Container.Resolve<IServiceProvider>();
    protected RouterHistoryManager<ViewModelBasePage> Router { get; private set; } = App.Container.Resolve<RouterHistoryManager<ViewModelBasePage>>();

    public abstract void OnNavigatedTo(NavigationContext context);
}