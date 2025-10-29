using Avalonia;
using Hydra.ViewModels.App;

namespace Hydra.Helpers;

public class BindingProxyAppDetails : AvaloniaObject
{
    public static readonly StyledProperty<AppDetailsViewModel?> DataProperty =
        AvaloniaProperty.Register<BindingProxyAppDetails, AppDetailsViewModel?>(nameof(Data));

    public AppDetailsViewModel? Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }
}