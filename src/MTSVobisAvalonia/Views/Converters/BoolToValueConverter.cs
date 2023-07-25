using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace MTSVobisAvalonia.Views.Converters
{
    public class BoolToValueConverter : IValueConverter
    {
        public object? IsTrue { get; set; }
        public object? IsFalse { get; set; }
        
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            switch (value)
            {
                case bool boolean:
                    return boolean ? IsTrue : IsFalse;
                
                default:
                    return IsFalse;
            }
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}