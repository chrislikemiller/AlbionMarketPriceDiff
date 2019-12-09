using AlbionMarketPriceDiff.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbionMarketPriceDiff.Util
{
    public static class Extensions
    {
        public static string ToJson<T>(this T item) => JsonConvert.SerializeObject(item, JsonParser.Settings);
        public static T FromJson<T>(this string item)
        {
            var result = JsonConvert.DeserializeObject<T>(item, JsonParser.Settings);
            return result;
        }

        public static IEnumerable<TradableMarketItem> OrderByType(this IEnumerable<TradableMarketItem> list_, PriceOrderingType priceOrderingType_)
        {
            return priceOrderingType_ switch
            {
                PriceOrderingType.AbsoluteProfit => list_.OrderByDescending(x => x.ProfitAbsolute),
                PriceOrderingType.PercentProfit  => list_.OrderByDescending(x => x.ProfitPercent),
                _                          => throw new ArgumentException($"Invalid ordering type {priceOrderingType_}"),
            };
        }
    }
}
