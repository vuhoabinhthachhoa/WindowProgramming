using System;
using Microsoft.UI.Xaml.Data; // Using directive for WinUI 3 value conversion.
using Windows.UI.Xaml.Data; // This might be a leftover using directive if the project was ported from UWP to WinUI.

namespace Sale_Project.Helpers;

/// <summary>
/// Converts DateOnly to DateTimeOffset and vice versa for binding purposes in XAML.
/// </summary>
public class DateOnlyToDateTimeOffsetConverter : IValueConverter
{
    /// <summary>
    /// Converts a DateOnly object to a DateTimeOffset object for display in the UI.
    /// </summary>
    /// <param name="value">The DateOnly object to convert.</param>
    /// <param name="targetType">The target type of the conversion (not used).</param>
    /// <param name="parameter">Additional parameters (not used).</param>
    /// <param name="language">The language of the conversion (not used).</param>
    /// <returns>A DateTimeOffset representation of the DateOnly object.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is DateOnly date)
        {
            // Convert DateOnly to DateTimeOffset by creating a DateTime with the minimum time.
            return new DateTimeOffset(date.ToDateTime(TimeOnly.MinValue));
        }
        return value; // Returns the original value if it is not a DateOnly.
    }

    /// <summary>
    /// Converts a DateTimeOffset object back to a DateOnly object when data flows back to the source.
    /// </summary>
    /// <param name="value">The DateTimeOffset object to convert back.</param>
    /// <param name="targetType">The target type of the conversion (not used).</param>
    /// <param name="parameter">Additional parameters (not used).</param>
    /// <param name="language">The language of the conversion (not used).</param>
    /// <returns>A DateOnly representation of the DateTimeOffset object.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is DateTimeOffset dateTimeOffset)
        {
            // Convert DateTimeOffset back to DateOnly by extracting the Date part of the DateTime.
            return DateOnly.FromDateTime(dateTimeOffset.DateTime);
        }
        return value; // Returns the original value if it is not a DateTimeOffset.
    }
}
