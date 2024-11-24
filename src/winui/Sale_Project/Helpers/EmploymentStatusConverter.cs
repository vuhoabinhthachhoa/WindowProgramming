using System;
using Microsoft.UI.Xaml.Data;

namespace Sale_Project.Helpers;

public class EmploymentStatusConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool boolValue)
        {
            return boolValue ? "Employed" : "Resigned"; // True -> Employed, False -> Resigned
        }
        return "Resigned"; // Default to "Resigned" if not a boolean
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string strValue)
        {
            return strValue == "Employed"; // Convert "Employed" back to true, "Resigned" to false
        }
        return false; // Default to false if not "Employed"
    }
}
