using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlbionMarketPriceDiff.Util
{
    public interface IRequestUrlBuilder
    {
        string GetUrl(IEnumerable<string> itemIds_, IEnumerable<string> cities_);
        IEnumerable<string> GetUrls(IEnumerable<string> itemIds_, IEnumerable<string> cities_);
    }

    public class RequestUrlBuilder : IRequestUrlBuilder
    {
        private const string BASE_URL = @"https://www.albion-online-data.com/api/v2/stats/prices/";
        public string GetUrl(IEnumerable<string> itemIds_, IEnumerable<string> cities_)
        {
            return $"{BASE_URL}{Concat(itemIds_)}?locations={Concat(cities_)}";
        }

        public IEnumerable<string> GetUrls(IEnumerable<string> itemIds_, IEnumerable<string> cities_)
        {
            var list = new List<string>();
            int length = 0;
            foreach (var item in itemIds_)
            {
                list.Add(item);
                length += item.Length;
                if (length > 2000)
                {
                    yield return GetUrl(list, cities_);
                    list.Clear();
                    length = 0;
                }
            }
        }

        private string Concat(IEnumerable<string> list_) => string.Join(",", list_);
    }
}