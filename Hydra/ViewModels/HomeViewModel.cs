using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Hydra.Domain.Models.Hydra.Api;
using Hydra.Infrastructure.Interfaces.Refit;
using Hydra.Infrastructure.Services.Gog;
using Hydra.Infrastructure.Services.Hydra;
using Hydra.Infrastructure.Services.Steam;
using Hydra.ViewModels.Shared;
using Lucene.Net.Util;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SimpleRoute.Avalonia.Context;

namespace Hydra.ViewModels;

public partial class HomeViewModel : ViewModelBasePage
{
    public ObservableCollection<HotGameModel> Games { get; } = new();

    [ObservableProperty] private FeaturedModel _featured = new FeaturedModel();
    
    private readonly IHydra _hydra;
    
    [ObservableProperty]  private bool _isLoading = true;
    [ObservableProperty] private bool _isSuccess = false;
    [ObservableProperty] private bool _is404 = false;
    
    public HomeViewModel(IHydra hydra)
    {
        _hydra = hydra;
    }

    public override async Task OnNavigatedToAsync(NavigationContext context)
    {
        var games = await _hydra.HotGameAsync();
        var featuredResult = await _hydra.FeatureAsync();
        
        if(games.IsSuccessStatusCode && games.Content != null)
            Games.AddRange(games.Content); 
        
        if(featuredResult.IsSuccessStatusCode && featuredResult.Content != null)
            Featured = featuredResult.Content.FirstOrDefault();

        if (!featuredResult.IsSuccessStatusCode && !games.IsSuccessStatusCode)
            Is404 = true;
        else
            IsSuccess = true;

        IsLoading = false;
    }
}