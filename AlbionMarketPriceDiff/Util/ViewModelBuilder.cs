using AlbionMarketPriceDiff.Util.Config;
using AlbionMarketPriceDiff.ViewModel;
using System.Net.Http;

namespace AlbionMarketPriceDiff.Util
{
    static class ViewModelBuilder
    {
        private const string CONFIG_PATH = "config.json";

        public static MainViewModel Build()
        {
            var marketService = new MarketService(new RequestUrlBuilder(), new HttpClient());
            var userConfig = new UserConfig(CONFIG_PATH);
            return new MainViewModel(marketService, userConfig);
        }
    }
}
