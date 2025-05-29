// Converters/BoolToStringConverter.cs
using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace AppOne.Mobile.Converters
{
    public class BoolToStringConverter : IValueConverter
    {
        public string TrueString { get; set; } = "Tak"; // Domyślna wartość
        public string FalseString { get; set; } = "Nie"; // Domyślna wartość

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? TrueString : FalseString;
            }
            return FalseString; // Lub rzuć wyjątek, lub zwróć Binding.DoNothing
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                return string.Equals(stringValue, TrueString, StringComparison.OrdinalIgnoreCase);
            }
            return false; // Lub rzuć wyjątek
        }
    }
}
