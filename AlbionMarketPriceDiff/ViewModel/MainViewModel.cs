using AlbionMarketPriceDiff.Model;
using AlbionMarketPriceDiff.Util;
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
    public class MainViewModel : RaisesPropertyChanged
    {
        private readonly IMarketService _marketService;
        private readonly DataStore _dataStore;
        private ObservableCollection<TradableMarketItem> _items;
        private int _dateAgeInHours = 24;
        private PriceOrderingType priceOrderingType;
        private bool _isLoading;

        public MainViewModel(IMarketService marketService_, DataStore dataStore_)
        {
            _marketService = marketService_;
            _dataStore = dataStore_;
            MarketItems = new ObservableCollection<TradableMarketItem>();
            GetPricesCommand = new RelayCommand(async () =>
            {
                IsLoading = true;
                try
                {
                    var allItems = new List<MarketItem>();
                    _dataStore.UpdateSelection(Sources);
                    foreach (var items in await _marketService.GetPrices(_dataStore, SelectedCities))
                    {
                        allItems.AddRange(items);
                    }

                    var now = DateTime.Now;
                    var timespan = TimeSpan.FromHours(DateAgeInHours);
                    var groupedItems = allItems
                        .Where(x => IsDateFreshEnough(x, now, timespan))
                        .GroupBy(x => x.ItemId);

                    var temp = new List<TradableMarketItem>();
                    foreach (var group in groupedItems)
                    {
                        var list = group.ToList();
                        for (int i = 0; i < list.Count; i++)
                        {
                            for (int j = 0; j < list.Count; j++)
                            {
                                if (i != j && list[i].SellPriceMin < list[j].SellPriceMin)
                                {
                                    temp.Add(new TradableMarketItem { FromItem = list[i], ToItem = list[j] });
                                }
                            }
                        }
                    }
                    MarketItems = new ObservableCollection<TradableMarketItem>(temp.OrderByType(PriceOrderingType));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                IsLoading = false;
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

        public IEnumerable<SelectableValue<string>> Cities { get; } = new SelectableValue<string>[]
        {
            new SelectableValue<string> { Value = "Black Market", IsSelected = false },
            new SelectableValue<string> { Value = "Caerleon", IsSelected = false },
            new SelectableValue<string> { Value = "Bridgewatch", IsSelected = false},
            new SelectableValue<string> { Value = "Martlock", IsSelected = false },
            new SelectableValue<string> { Value = "Thetford", IsSelected = false },
            new SelectableValue<string> { Value = "Lymhurst", IsSelected = false },
            new SelectableValue<string> { Value = "Fortsterling", IsSelected = false },
        };

        public int DateAgeInHours
        {
            get => _dateAgeInHours;
            set
            {
                _dateAgeInHours = value;
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

        public IEnumerable<string> SelectedCities => Cities.Where(x => x.IsSelected).Select(x => x.Value);
        public IEnumerable<SelectableValue<string>> Sources { get; } = new SelectableValue<string>[]
        {
            new SelectableValue<string> { Value = DataStore.RESOURCES, IsSelected = true },
            new SelectableValue<string> { Value = DataStore.ITEMS, IsSelected = false },
            new SelectableValue<string> { Value = DataStore.FOODS, IsSelected = false },
            new SelectableValue<string> { Value = DataStore.MISC, IsSelected = false },
        };

        public ICommand GetPricesCommand { get; set; }

        private bool IsDateFreshEnough(MarketItem marketItem_, DateTime now_, TimeSpan span_)
            => now_.Subtract(marketItem_.SellPriceMinDate.LocalDateTime).TotalMilliseconds <= span_.TotalMilliseconds;

        private bool IsDateFreshEnough(TradableMarketItem marketItem_, DateTime now_, TimeSpan span_)
            => IsDateFreshEnough(marketItem_.FromItem, now_, span_) && IsDateFreshEnough(marketItem_.ToItem, now_, span_);
    }
}
