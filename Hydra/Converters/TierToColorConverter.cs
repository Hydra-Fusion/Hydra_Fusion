using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Hydra.Converters;

public class TierToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value?.ToString()?.ToLower() switch
        {
            "platinum" => Brushes.LightGray,
            "gold" => new SolidColorBrush(Color.FromRgb(255, 215, 0)),
            "silver" => new SolidColorBrush(Color.FromRgb(192, 192, 192)),
            "bronze" => new SolidColorBrush(Color.FromRgb(205, 127, 50)),
            "native" => Brushes.Green,
            _ => Brushes.Gray
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}