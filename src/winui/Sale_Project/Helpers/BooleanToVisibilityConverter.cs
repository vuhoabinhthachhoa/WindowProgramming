using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;


namespace Sale_Project.Helpers;
/// <summary>
/// Converts a boolean value to a Visibility enumeration value.
/// </summary>
public class BooleanToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// Converts a boolean value to a Visibility enumeration value.
    /// </summary>
    /// <param name="value">The boolean value to convert.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to invert the boolean value before conversion.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>Visibility.Visible if the boolean value is true; otherwise, Visibility.Collapsed.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool boolValue)
        {
            bool invert = parameter != null && bool.TryParse(parameter.ToString(), out bool paramValue) && paramValue;
            return (boolValue ^ invert) ? Visibility.Visible : Visibility.Collapsed;
        }
        return Visibility.Visible;
    }

    /// <summary>
    /// This method is not supported and will throw a NotSupportedException if called.
    /// </summary>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">An optional parameter to use during the conversion.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>Throws a NotSupportedException.</returns>
    /// <exception cref="NotSupportedException">Always thrown by this method.</exception>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotSupportedException("BooleanToVisibilityConverter does not support ConvertBack.");
    }
}


