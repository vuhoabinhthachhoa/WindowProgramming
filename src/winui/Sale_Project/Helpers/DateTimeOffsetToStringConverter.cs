using Microsoft.UI.Xaml.Data;

namespace Sale_Project.Helpers;

public class DateTimeOffsetToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToString("yyyy-MM-dd");
        }

        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string dateString && DateTimeOffset.TryParse(dateString, out var dateTimeOffset))
        {
            return dateTimeOffset;
        }

        return DateTimeOffset.MinValue;
    }
}

