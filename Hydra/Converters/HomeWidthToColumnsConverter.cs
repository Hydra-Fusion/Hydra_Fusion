using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Hydra.Converters;

public class HomeWidthToColumnsConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double width)
        {
            return width switch
            {
                < 900 => 2,
                < 1400 => 3,
                < 2000 => 4,
                _ => 5
            };
        }
        return 1;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}