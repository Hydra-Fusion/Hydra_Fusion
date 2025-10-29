using CommunityToolkit.Mvvm.ComponentModel;
using Hydra.ViewModels.Catalog;
using Hydra.ViewModels.Download;
using Hydra.ViewModels.Settings;
using Hydra.ViewModels.Shared;

namespace Hydra.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBasePage _content = default;
    [ObservableProperty] private string _title;
    [ObservableProperty] private string _windowTitle;
    [ObservableProperty] private bool _hasPrev = false;

    public MainWindowViewModel()
    {
        Router.CurrentViewModelChanged += viewModel =>
        {
            Content = viewModel;
            HasPrev = Router.HasPrev;
        };
        
        Router.CurrentTitleChanged += title =>
        {
            Title = title;
            WindowTitle = string.IsNullOrWhiteSpace(title) ? "Hydra Launcher" : $"Hydra Launcher - {title}";
        };

        Router.GoTo<HomeViewModel>("home", "Início");
    }
    
    public void Back()
        => Router.Back();
    
    public void GoToHome() => Router.GoTo<HomeViewModel>("home", "Início");
    public void GoToCatalog() => Router.GoTo<CatalogViewModel>("catalog", "Catálogo");
    public void GoToDownload() => Router.GoTo<DownloadViewModel>("download", "Downloads");
    public void GoToSettings() => Router.GoTo<SettingsViewModel>("settings", "Configurações");
}