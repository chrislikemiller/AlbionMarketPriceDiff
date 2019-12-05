using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace AlbionMarketPriceDiff.Util.Converters
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTimeOffset dateTime)
            {
                var now = DateTime.Now;
                var diff = now.Subtract(dateTime.LocalDateTime);
                if (diff.TotalHours < 1)
                    return $"{(int)diff.TotalMinutes} minutes ago";
                if (diff.TotalDays < 1)
                    return $"{(int)diff.TotalHours} hours ago";
                return $"{(int)diff.TotalDays} days ago";
            }
            return "No value";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
