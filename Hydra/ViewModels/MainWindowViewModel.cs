using CommunityToolkit.Mvvm.ComponentModel;
using Hydra.ViewModels.Catalog;
using Hydra.ViewModels.Download;
using Hydra.ViewModels.Settings;
using Hydra.ViewModels.Shared;

namespace Hydra.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBasePage _content = default;

    public MainWindowViewModel()
    {
        Router.CurrentViewModelChanged += viewModel => Content = viewModel;

        Router.GoTo<HomeViewModel>("home");
    }
    
    public void GoToHome() => Router.GoTo<HomeViewModel>("home");
    public void GoToCatalog() => Router.GoTo<CatalogViewModel>("catalog");
    public void GoToDownload() => Router.GoTo<DownloadViewModel>("download");
    public void GoToSettings() => Router.GoTo<SettingsViewModel>("settings?title=settings");
}