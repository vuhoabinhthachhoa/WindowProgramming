using System;
using Microsoft.UI.Xaml.Data;
using Windows.UI.Xaml.Data;

namespace Sale_Project.Helpers;

public class DateOnlyToDateTimeOffsetConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is DateOnly date)
        {
            // Convert DateOnly to DateTimeOffset
            return new DateTimeOffset(date.ToDateTime(TimeOnly.MinValue));
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is DateTimeOffset dateTimeOffset)
        {
            // Convert DateTimeOffset to DateOnly
            return DateOnly.FromDateTime(dateTimeOffset.DateTime);
        }
        return value;
    }
}
