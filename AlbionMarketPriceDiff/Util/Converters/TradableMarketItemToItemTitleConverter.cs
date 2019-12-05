using AlbionMarketPriceDiff.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace AlbionMarketPriceDiff.Util.Converters
{
    public class TradableMarketItemToItemTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TradableMarketItem marketItem)
            {
                return $"{marketItem.FromItem.ItemName} ({marketItem.FromItem.ItemId})";
            }
            throw new ArgumentException("Item title is of wrong type or null.");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
