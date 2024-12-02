using System;
using Microsoft.UI.Xaml.Data;

namespace Sale_Project.Helpers;

public class BusinessStatusConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool boolValue)
        {
            return boolValue ? "Active" : "Inactive"; // True -> Employed, False -> Resigned
        }
        return "Resigned"; // Default to "Resigned" if not a boolean
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string strValue)
        {
            return strValue == "Active"; // Convert "Employed" back to true, "Resigned" to false
        }
        return false; // Default to false if not "Employed"
    }
}
