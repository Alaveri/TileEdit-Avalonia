using Alaveri.Core;
using Alaveri.Core.Extensions;
using Avalonia.Data.Converters;
using System;

namespace TileEdit.ViewModels;

public class IntEntryConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        return value?.AsDecimal(1);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        var val = SafeConvert.ToInt32(value ?? 1, 1);
        return val; 
    }
}
