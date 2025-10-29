using System;
using CommunityToolkit.Mvvm.Input;
using Hydra.Domain.Models.Hydra.Api;
using Hydra.ViewModels.App;
using Hydra.ViewModels.Shared;

namespace Hydra.ViewModels.Components;

public partial class HomeGameViewModel : ViewModelBase
{
    public void Navigate(HotGameModel model)
    {
        Router.GoTo<AppDetailsViewModel>($"app?Store={model.Shop}&Id={model.ObjectId}", model.Title);
    }
}