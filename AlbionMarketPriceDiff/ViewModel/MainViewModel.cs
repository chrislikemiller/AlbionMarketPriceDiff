using AlbionMarketPriceDiff.Model;
using AlbionMarketPriceDiff.Util;
using AlbionMarketPriceDiff.Util.Config;
using AlbionMarketPriceDiff.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace AlbionMarketPriceDiff.ViewModel
{
    public sealed class MainViewModel : RaisesPropertyChanged, IDisposable
    {
        private readonly IMarketService _marketService;
        private readonly UserConfig _userConfig;

        private ObservableCollection<TradableMarketItem> _items;
        private IEnumerable<(SelectableValue<string>, IEnumerable<(string, string)>)> _loadedResources;
        private PriceOrderingType priceOrderingType;
        private bool _isLoading;

        public MainViewModel(IMarketService marketService_, UserConfig userConfig_)
        {
            MarketItems = new ObservableCollection<TradableMarketItem>();
            _marketService = marketService_;
            _userConfig = userConfig_;
            _loadedResources = ParseResources(userConfig_);

            GetPricesCommand = new RelayCommand(async () =>
            {
                IsLoading = true;
                try
                {
                    var allItems = new List<MarketItem>();
                    foreach (var items in await _marketService.GetPrices(_loadedResources, SelectedCities))
                    {
                        allItems.AddRange(items);
                    }

                    var now = DateTime.Now;
                    var timespan = TimeSpan.FromHours(DefaultMaxDateInHours);
                    var groupedItems = allItems
                        .Where(x => IsDateFreshEnough(x, now, timespan))
                        .GroupBy(x => x.ItemId);
                    MarketItems = new ObservableCollection<TradableMarketItem>(
                        CollectTradableItems(groupedItems)
                        .OrderByType(PriceOrderingType));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    IsLoading = false;
                }
            });
            OpenConfigWindowCommand = new RelayCommand(() =>
            {
                var configWindow = new ConfigWindow()
                {
                    DataContext = _userConfig
                };
                configWindow.ShowDialog();
            });
        }

        public ObservableCollection<TradableMarketItem> MarketItems
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<string> SelectedCities => Cities.Where(x => x.IsSelected).Select(x => x.Value);

        public IEnumerable<SelectableValue<string>> Cities => _userConfig.Cities;

        public int DefaultMaxDateInHours
        {
            get => _userConfig.DefaultMaxDateInHours;
            set
            {
                _userConfig.DefaultMaxDateInHours = value;
                OnPropertyChanged();
            }
        }

        public PriceOrderingType PriceOrderingType
        {
            get => priceOrderingType;
            set
            {
                priceOrderingType = value;
                OnPropertyChanged();

                MarketItems = new ObservableCollection<TradableMarketItem>(MarketItems.OrderByType(PriceOrderingType));
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<SelectableValue<string>> Sources => _loadedResources.Select(x => x.Item1);
        public ICommand GetPricesCommand { get; set; }
        public ICommand OpenConfigWindowCommand { get; set; }

        private bool IsDateFreshEnough(MarketItem marketItem_, DateTime now_, TimeSpan span_)
            => now_.Subtract(marketItem_.SellPriceMinDate.LocalDateTime).TotalMilliseconds <= span_.TotalMilliseconds;

        private IEnumerable<(SelectableValue<string>, IEnumerable<(string, string)>)> ParseResources(UserConfig userConfig_)
        {
            foreach (var (selectable, path) in userConfig_.Resources)
            {
                var lines = File.ReadAllLines(path);
                var split = lines
                    .Select(x => x.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
                    .Select(x => (x[1], x.Length == 2 ? x[1] : x[2]));
                yield return (selectable, split);
            }
        }

        private static IEnumerable<TradableMarketItem> CollectTradableItems(IEnumerable<IGrouping<string, MarketItem>> groupedItems)
        {
            foreach (var group in groupedItems)
            {
                var list = group.ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (i != j && list[i].SellPriceMin < list[j].SellPriceMin)
                        {
                            yield return new TradableMarketItem { FromItem = list[i], ToItem = list[j] };
                        }
                    }
                }
            }
        }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            _userConfig?.SaveConfig();
        }

        public void Dispose()
        {
            _userConfig?.SaveConfig();
        }
    }
}
