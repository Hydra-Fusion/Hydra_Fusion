using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Hydra.Components.Home.GameComponent;

public partial class HomeGameComponent : UserControl
{
    public static readonly StyledProperty<string> ImageUrl 
        = AvaloniaProperty.Register<HomeGameComponent, string>(nameof(ImageUrl));
    
    public static readonly StyledProperty<string> Title
        = AvaloniaProperty.Register<HomeGameComponent, string>(nameof(Title));
    
    public static readonly StyledProperty<int> Dowloads
        = AvaloniaProperty.Register<HomeGameComponent, int>(nameof(Dowloads));
    
    public static readonly StyledProperty<int> Rating
        =  AvaloniaProperty.Register<HomeGameComponent, int>(nameof(Rating));
    
    public HomeGameComponent()
    {
        InitializeComponent();
    }
    
    public string GameName
    {
        get => GetValue(Title);
        set => SetValue(Title, value);
    }
    
    public string GameIcon
    {
        get => GetValue(ImageUrl);
        set => SetValue(ImageUrl, value);
    }
}