using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AsyncImageLoader;
using AsyncImageLoader.Loaders;
using Avalonia;
using AvaloniaEdit.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Hydra.Domain.Models.Internal.Details;
using Hydra.Infrastructure.Localizer;
using Hydra.Infrastructure.Services.Gog;
using Hydra.Infrastructure.Services.Steam;
using Hydra.Message;
using Hydra.Pages.App;
using Hydra.ViewModels.Shared;
using SimpleRoute.Avalonia.Context;

namespace Hydra.ViewModels.App;

public partial class AppDetailsViewModel : ViewModelBasePage
{
    private readonly SteamServices _steam;
    private readonly GogServices _gog;

    public ObservableCollection<string> Screenshots { get; } = new();

    [ObservableProperty] private bool _linux = false;
    [ObservableProperty] private bool _windows = false;
    [ObservableProperty] private bool _mac = false;
    
    
    [ObservableProperty] private string _publishers = "";
    
    [ObservableProperty] private bool _isLoading = true;
    [ObservableProperty] private bool _is404 = false;
    [ObservableProperty] private bool _isSuccess = false;
    [ObservableProperty] private Details? _details = null;
    [ObservableProperty] private IAsyncImageLoader _imageLoader = new AsyncImageLoader.Loaders.RamCachedWebImageLoader();
    [ObservableProperty] private bool _containsProtonDetails = false;
    
    
    private string gameId = string.Empty;
    
    public AppDetailsViewModel(SteamServices steam, GogServices gog)
    {
        _steam = steam;
        _gog = gog;
    }
    
    public override async Task OnNavigatedToAsync(NavigationContext context)
    {
        var store = context.Query.GetValue("Store");
        gameId = context.Query.GetValue("Id");

        switch (store.ToLower())
        {
            case "steam":
                Details = await GetDetailsBySteamAsync();
                break;
        }
        
        ContainsProtonDetails = Details?.Proton != null;
        Is404 = Details == null;
        IsSuccess = !Is404;
        IsLoading = false;
        
    }
    
    private async Task<Details?> GetDetailsBySteamAsync()
    {
        Details? details = null;

        if (uint.TryParse(gameId, out var result))
            details = await _steam.Store.GetAppDetailsAsync(result);
        
        ImageLoader = new DiskCachedWebImageLoader($"Cache/Steam/{details.Id}-{details.Name}");

        Publishers = string.Format(Localizer.Instance["game_details_publisher"].Replace("{{", "{").Replace("}}", "}"), string.Join(", ", details.Publishers));
        
        Screenshots.AddRange(details.Screenshots.Select(x => x.PathFull));

        Linux = details.Platforms.Linux;
        Windows = details.Platforms.Windows;
        Mac = details.Platforms.Mac;
        
        return details;
    }
    
    [RelayCommand]
    public async Task OpenConfig()
    {
        var config = await WeakReferenceMessenger.Default.Send(new AppDetailsConfigMessage(Details.Id, Details.Name));
    }
}