using System;
using System.Collections.Generic;
using System.Text;

namespace AlbionMarketPriceDiff.Model
{
    public class TradableMarketItem
    {
        public MarketItem FromItem { get; set; }
        public MarketItem ToItem { get; set; }

        public double ProfitPercent => Math.Round(Math.Abs(100 - FromItem.SellPriceMin / (double)ToItem.SellPriceMin * 100), 1);
        public long ProfitAbsolute => ToItem.SellPriceMin - FromItem.SellPriceMin;
    }
}
