using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Hydra.ViewModels.App;

namespace Hydra.Converters;

public class AppDetailsConverterModelView : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value as AppDetailsViewModel;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}