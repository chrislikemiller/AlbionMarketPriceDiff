using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace AlbionMarketPriceDiff.Util.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public bool Inverted { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value is bool b && 
                    !Inverted && b 
                ? Visibility.Visible 
                : Visibility.Collapsed;
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
