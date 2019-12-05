using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AlbionMarketPriceDiff.Model;

namespace AlbionMarketPriceDiff.Util
{
    public interface IMarketService
    {
        Task<IEnumerable<IEnumerable<MarketItem>>> GetPrices(DataStore dataStore_, IEnumerable<string> cities_);
    }

    public class MarketService : IMarketService
    {
        private readonly HttpClient _httpClient;
        private readonly IRequestUrlBuilder _requestUrlBuilder;

        public MarketService(IRequestUrlBuilder requestUrlBuilder_, HttpClient httpClient_)
        {
            _httpClient = httpClient_;
            _requestUrlBuilder = requestUrlBuilder_;
        }

        public async Task<IEnumerable<IEnumerable<MarketItem>>> GetPrices(DataStore dataStore_, IEnumerable<string> cities_)
        {
            var tasks = new List<Task<IEnumerable<IEnumerable<MarketItem>>>>();
            foreach (var items in dataStore_.GetAllSelected())
            {
                tasks.Add(GetPricesInner(items, cities_));
            }
            var result = await Task.WhenAll(tasks);
            return result.SelectMany(x => x);

        }

        private async Task<IEnumerable<IEnumerable<MarketItem>>> GetPricesInner(SelectableValue<IDictionary<string, string>> items_, IEnumerable<string> cities_)
        {
            var list = new List<IEnumerable<MarketItem>>();
            foreach (var url in _requestUrlBuilder.GetUrls(items_.Value.Keys, cities_))
            {
                var result = await _httpClient.GetAsync(url);
                if (result.IsSuccessStatusCode)
                {
                    var json = await result.Content.ReadAsStringAsync();
                    list.Add(json.FromJson<IEnumerable<MarketItem>>());
                }
                else
                {
                    throw new HttpRequestException($"Request failed for {url}. Result is {result.StatusCode}. Reason is {result.ReasonPhrase}.");
                }
            }
            return list.Select(marketItems =>
            {
                foreach (var item in marketItems)
                {
                    item.ItemName = items_.Value[item.ItemId];
                }
                return marketItems;
            });
        }
    }
}
