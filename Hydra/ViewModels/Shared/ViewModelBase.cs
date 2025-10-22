using System;
using CommunityToolkit.Mvvm.ComponentModel;
using DryIoc;
using SimpleRoute.Avalonia;

namespace Hydra.ViewModels.Shared;

public abstract class ViewModelBase : ObservableObject
{
    protected IServiceProvider ServiceProvider { get; private set; } = App.Container.Resolve<IServiceProvider>();
    protected RouterHistoryManager<ViewModelBasePage> Router { get; private set; } = App.Container.Resolve<RouterHistoryManager<ViewModelBasePage>>();
}