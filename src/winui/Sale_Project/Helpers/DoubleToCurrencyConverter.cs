using Microsoft.UI.Xaml.Data;
using System;
using System.Globalization;

namespace Sale_Project.Helpers;
public class DoubleToCurrencyConverter : IValueConverter
{
    /// <summary>
    /// Converts a double value to a currency formatted string.
    /// </summary>
    /// <param name="value">The value produced by the binding source.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>A currency formatted string if the value is a double; otherwise, the original value.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is double doubleValue)
        {
            return doubleValue.ToString("N", CultureInfo.CurrentCulture);
        }
        return value;
    }

    /// <summary>
    /// Converts a currency formatted string back to a double value.
    /// </summary>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>A double value if the conversion is successful; otherwise, the original value.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string strValue && double.TryParse(strValue, NumberStyles.Number, CultureInfo.CurrentCulture, out double result))
        {
            return result;
        }
        return value;
    }
}
