using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AlbionMarketPriceDiff.Model;
using AlbionMarketPriceDiff.Util.Config;

namespace AlbionMarketPriceDiff.Util
{
    public interface IMarketService
    {
        Task<IEnumerable<IEnumerable<MarketItem>>> GetPrices(IEnumerable<(SelectableValue<string>, IEnumerable<(string, string)>)> loadedResources_, IEnumerable<string> selectedCities_);
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

        public async Task<IEnumerable<IEnumerable<MarketItem>>> GetPrices(IEnumerable<(SelectableValue<string>, IEnumerable<(string, string)>)> resources_, IEnumerable<string> selectedCities_)
        {
            var tasks = new List<Task<IEnumerable<IEnumerable<MarketItem>>>>();
            foreach (var (_, resourceMap) in resources_)
            {
                tasks.Add(GetPricesInner(resourceMap, selectedCities_));
            }
            var result = await Task.WhenAll(tasks);
            return result.SelectMany(x => x);
        }

        private async Task<IEnumerable<IEnumerable<MarketItem>>> GetPricesInner(IEnumerable<(string, string)> items_, IEnumerable<string> cities_)
        {
            var list = new List<IEnumerable<MarketItem>>();
            foreach (var url in _requestUrlBuilder.GetUrls(items_.Select(x => x.Item1), cities_))
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
                    item.ItemName = items_.FirstOrDefault(x => x.Item1 == item.ItemId).Item2 ?? item.ItemId;
                }
                return marketItems;
            });
        }
    }
}
