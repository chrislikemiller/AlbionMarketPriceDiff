using System;
using System.Collections.Generic;
using System.Text;
using AlbionMarketPriceDiff.Model;
using Newtonsoft.Json;

namespace AlbionMarketPriceDiff.Util.Config
{
    public class ConfigModel
    {
        [JsonProperty("cities")]
        public IEnumerable<SelectableValue<string>> Cities { get; set; }

        [JsonProperty("resources")]
        public IEnumerable<(SelectableValue<string>, string)> Resources { get; set; }

        [JsonProperty("is_premium")]
        public bool IsPremium { get; set; }

        [JsonProperty("default_max_date_hrs")]
        public int DefaultMaxDateInHours { get; set; }

        public static ConfigModel BrandNewConfig => new ConfigModel()
        {
            Cities = new[]
            {
                new SelectableValue<string> { Value = "Black Market", IsSelected = false },
                new SelectableValue<string> { Value = "Caerleon", IsSelected = false },
                new SelectableValue<string> { Value = "Martlock", IsSelected = false },
                new SelectableValue<string> { Value = "Bridgewatch", IsSelected = false},
                new SelectableValue<string> { Value = "Lymhurst", IsSelected = false },
                new SelectableValue<string> { Value = "Fortsterling", IsSelected = false },
                new SelectableValue<string> { Value = "Thetford", IsSelected = false },
            },
            Resources = new List<(SelectableValue<string>, string)>()
            {
                (new SelectableValue<string> { Value = "Resources", IsSelected = true }, @"Resources\resources.txt"),
                (new SelectableValue<string> { Value = "Useful items", IsSelected = false }, @"Resources\items.txt"),
                (new SelectableValue<string> { Value = "Foods", IsSelected = false }, @"Resources\food.txt"),
                (new SelectableValue<string> { Value = "Misc", IsSelected = false }, @"Resources\misc.txt"),
                (new SelectableValue<string> { Value = "All", IsSelected = false }, @"Resources\all.txt"),
            },
            IsPremium = false,
            DefaultMaxDateInHours = 12
        };

        public object this[string key_]
        {
            get
            {
                return GetValue(key_);
            }
            set
            {
                SetValue(key_, value);
            }
        }

        private object GetValue(string key_)
        {
            return key_ switch
            {
                nameof(Cities) => Cities,
                nameof(Resources) => Resources,
                nameof(IsPremium) => IsPremium,
                nameof(DefaultMaxDateInHours) => DefaultMaxDateInHours,
                _ => throw new KeyNotFoundException($"Key not found in config: {key_}")
            };
        }

        // TODO: look into making this as generic as humanly possible
        private void SetValue(string key_, object value_)
        {
            switch (key_)
            {
                case nameof(Cities):
                    Cities = (IEnumerable<SelectableValue<string>>)value_;
                    break;
                case nameof(Resources):
                    Resources = (IEnumerable<(SelectableValue<string>, string)>)value_;
                    break;
                case nameof(IsPremium):
                    IsPremium = (bool)value_;
                    break;
                case nameof(DefaultMaxDateInHours):
                    DefaultMaxDateInHours = (int)value_;
                    break;
                default:
                    throw new KeyNotFoundException($"Key not found in config: {key_}");
            }
        }

    }
}
