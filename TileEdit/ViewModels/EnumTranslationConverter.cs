using Alaveri.Core.Enumerations.Extensions;
using Alaveri.Localization;
using Avalonia.Data.Converters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace TileEdit.ViewModels;

public class EnumTranslationConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        var serviceProvider = (Avalonia.Application.Current as App)?.ServiceProvider;
        return value is Enum enumValue ? serviceProvider?.GetService<ILanguageTranslator>()?.Translate(enumValue.GetIdentifier()) : value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
