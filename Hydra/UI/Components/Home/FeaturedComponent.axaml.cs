using Avalonia;
using Avalonia.Controls;
using Hydra.Domain.Models.Hydra.Api;

namespace Hydra.UI.Components.Home;

public partial class FeaturedComponent : UserControl
{
    public static readonly StyledProperty<FeaturedModel?> FeaturedProperty =
        AvaloniaProperty.Register<HomeGameComponent, FeaturedModel?>(nameof(Featured));
    
    public FeaturedModel? Featured
    {
        get => GetValue(FeaturedProperty);
        set => SetValue(FeaturedProperty, value);
    }
    
    public FeaturedComponent()
    {
        InitializeComponent();
    }
}