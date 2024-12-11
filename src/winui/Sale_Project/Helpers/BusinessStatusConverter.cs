using System;
using Microsoft.UI.Xaml.Data;

namespace Sale_Project.Helpers;

/// <summary>
/// Converts a boolean value to a business status string and vice versa.
/// </summary>
public class BusinessStatusConverter : IValueConverter
{
    /// <summary>
    /// Converts a boolean value to a business status string.
    /// </summary>
    /// <param name="value">The boolean value to convert.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>Returns "Active" if the value is true, "Inactive" if the value is false, and "Resigned" if the value is not a boolean.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool boolValue)
        {
            return boolValue ? "Active" : "Inactive"; // True -> Employed, False -> Resigned
        }
        return "Resigned"; // Default to "Resigned" if not a boolean
    }

    /// <summary>
    /// Converts a business status string back to a boolean value.
    /// </summary>
    /// <param name="value">The business status string to convert.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>Returns true if the value is "Active", false otherwise.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string strValue)
        {
            return strValue == "Active"; // Convert "Employed" back to true, "Resigned" to false
        }
        return false; // Default to false if not "Employed"
    }
}
