using AlbionMarketPriceDiff.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlbionMarketPriceDiff.Model
{
    public class SelectableValue<T> : RaisesPropertyChanged
    {
        private bool _isSelected;

        [JsonProperty("value")]
        public T Value { get; set; }
        
        [JsonProperty("is_selected")]
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public static implicit operator SelectableValue<T>(T val)
        {
            return new SelectableValue<T> { Value = val };
        }
    }
}
