using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Hydra.Domain.Models.Hydra.Api;
using Hydra.ViewModels.Components;

namespace Hydra.UI.Components.Home;

public partial class HomeGameComponent : UserControl
{
    public static readonly StyledProperty<HotGameModel?> GameProperty =
        AvaloniaProperty.Register<HomeGameComponent, HotGameModel?>(nameof(Game));

    public HotGameModel? Game
    {
        get => GetValue(GameProperty);
        set => SetValue(GameProperty, value);
    }

    public HomeGameComponent()
    {
        InitializeComponent();
    }
    
    private void OnCardClicked(object? sender, PointerPressedEventArgs e)
    {
        if (Game == null)
            return;
        
        var vm = new HomeGameViewModel();
        vm.Navigate(Game);
    }
}