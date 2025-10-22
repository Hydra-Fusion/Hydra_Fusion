using CommunityToolkit.Mvvm.ComponentModel;
using Hydra.ViewModels.Shared;
using SimpleRoute.Avalonia.Context;

namespace Hydra.ViewModels.Settings;

public partial class SettingsViewModel : ViewModelBasePage
{
    [ObservableProperty] 
    private string _title = string.Empty;
    
    public override void OnNavigatedTo(NavigationContext context)
    {
        _title = context.Query.GetValue("title");
    }
}