using System;
using Microsoft.UI.Xaml.Data;

namespace Sale_Project.Helpers;

/// <summary>
/// Converts a boolean employment status to a string representation and vice versa.
/// </summary>
public class EmploymentStatusConverter : IValueConverter
{
    /// <summary>
    /// Converts a boolean value to a string representation of employment status.
    /// </summary>
    /// <param name="value">The boolean value to convert.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>A string "Employed" if the value is true, otherwise "Resigned".</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool boolValue)
        {
            return boolValue ? "Employed" : "Resigned"; // True -> Employed, False -> Resigned
        }
        return "Resigned"; // Default to "Resigned" if not a boolean
    }

    /// <summary>
    /// Converts a string representation of employment status back to a boolean value.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>True if the value is "Employed", otherwise false.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string strValue)
        {
            return strValue == "Employed"; // Convert "Employed" back to true, "Resigned" to false
        }
        return false; // Default to false if not "Employed"
    }
}
