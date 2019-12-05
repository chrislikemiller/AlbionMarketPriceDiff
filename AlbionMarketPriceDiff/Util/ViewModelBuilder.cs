using AlbionMarketPriceDiff.Model;
using AlbionMarketPriceDiff.Util;
using AlbionMarketPriceDiff.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbionMarketPriceDiff.Util
{
    static class ViewModelBuilder
    {
        private const string ROOT = "Resources\\";
        private const string ALL_FILEPATH = ROOT + "all.txt";
        private const string FOODS_FILEPATH = ROOT + "food.txt";
        private const string ITEMS_FILEPATH = ROOT + "items.txt";
        private const string MISC_FILEPATH = ROOT + "misc.txt";
        private const string RESOURCES_FILEPATH = ROOT + "resources.txt";

        public static MainViewModel Build()
        {
            var marketService = new MarketService(new RequestUrlBuilder(), new System.Net.Http.HttpClient());
            var dataStore = new DataStore
            {
                All = Parse(ALL_FILEPATH, enabled_: false),
                Foods = Parse(FOODS_FILEPATH, enabled_: true),
                Items = Parse(ITEMS_FILEPATH, enabled_: true),
                Misc = Parse(MISC_FILEPATH, enabled_: true),
                Resources = Parse(RESOURCES_FILEPATH, enabled_: true)
            };
            return new MainViewModel(marketService, dataStore);
        }

        private static SelectableValue<IDictionary<string, string>> Parse(string path_, bool enabled_)
        {
            var lines = File.ReadAllLines(path_);
            var split = lines
                .Select(x => x.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
                .ToDictionary(x => x[1], x => x.Length == 2 ? x[1] : x[2]);
            return new SelectableValue<IDictionary<string, string>> { Value = split, IsSelected = enabled_ };
        }
    }
}
