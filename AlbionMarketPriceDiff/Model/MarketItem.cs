using System;
using AlbionMarketPriceDiff.Util;
using Newtonsoft.Json;

namespace AlbionMarketPriceDiff.Model
{
    public class MarketItem
    {
        [JsonProperty("item_id")]
        public string ItemId { get; set; }

        public string ItemName { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("quality")]
        public long Quality { get; set; }

        [JsonProperty("sell_price_min")]
        public long SellPriceMin { get; set; }

        [JsonProperty("sell_price_min_date")]
        public DateTimeOffset SellPriceMinDate { get; set; }

        [JsonProperty("sell_price_max")]
        public long SellPriceMax { get; set; }

        [JsonProperty("sell_price_max_date")]
        public DateTimeOffset SellPriceMaxDate { get; set; }

        [JsonProperty("buy_price_min")]
        public long BuyPriceMin { get; set; }

        [JsonProperty("buy_price_min_date")]
        public DateTimeOffset BuyPriceMinDate { get; set; }

        [JsonProperty("buy_price_max")]
        public long BuyPriceMax { get; set; }

        [JsonProperty("buy_price_max_date")]
        public DateTimeOffset BuyPriceMaxDate { get; set; }

        public static MarketItem[] FromJson(string json) => JsonConvert.DeserializeObject<MarketItem[]>(json, JsonParser.Settings);
    }

}