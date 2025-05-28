// Lokalizacja: datawedge_MAUI_SampleApp/Converters/BoolToStringConverter.cs
#nullable enable
using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace datawedge_MAUI_SampleApp.Converters
{
    public class BoolToStringConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool boolValue && parameter is string stringParameter)
            {
                var parameters = stringParameter.Split('|');
                if (parameters.Length == 2)
                {
                    return boolValue ? parameters[1] : parameters[0]; // Zwraca drugi parametr dla true, pierwszy dla false
                }
            }
            return string.Empty; // Domyślna wartość, jeśli coś pójdzie nie tak
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // Konwersja wsteczna zazwyczaj nie jest potrzebna dla tego typu konwertera
            throw new NotImplementedException();
        }
    }
}
