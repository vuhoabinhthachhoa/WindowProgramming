using Microsoft.UI.Xaml.Data;
using System;
using System.Globalization;

namespace Sale_Project.Helpers;
public class DoubleToCurrencyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is double doubleValue)
        {
            return doubleValue.ToString("N", CultureInfo.CurrentCulture);
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string strValue && double.TryParse(strValue, NumberStyles.Number, CultureInfo.CurrentCulture, out double result))
        {
            return result;
        }
        return value;
    }
}