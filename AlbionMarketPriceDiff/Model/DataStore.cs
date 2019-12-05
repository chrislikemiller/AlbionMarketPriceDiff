using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlbionMarketPriceDiff.Model
{
    public class DataStore
    {
        public const string RESOURCES = "Resources";
        public const string ITEMS = "Useful Items";
        public const string FOODS = "Foods/Ingredients";
        public const string MISC = "Misc";
        public SelectableValue<IDictionary<string, string>> All { get; set; }
        public SelectableValue<IDictionary<string, string>> Foods { get; set; }
        public SelectableValue<IDictionary<string, string>> Items { get; set; }
        public SelectableValue<IDictionary<string, string>> Misc { get; set; }
        public SelectableValue<IDictionary<string, string>> Resources { get; set; }

        public IEnumerable<SelectableValue<IDictionary<string, string>>> GetAllSelected()
        {
            if (All.IsSelected)
                yield return All;
            if (Foods.IsSelected)
                yield return Foods;
            if (Items.IsSelected)
                yield return Items;
            if (Misc.IsSelected)
                yield return Misc;
            if (Resources.IsSelected)
                yield return Resources;
        }

        internal void UpdateSelection(IEnumerable<SelectableValue<string>> sources_)
        {
            foreach (var source in sources_)
            {
                switch (source.Value)
                {
                    case RESOURCES:
                        Resources.IsSelected = source.IsSelected;
                        break;
                    case ITEMS:
                        Items.IsSelected = source.IsSelected;
                        break;
                    case FOODS:
                        Foods.IsSelected = source.IsSelected;
                        break;
                    case MISC:
                        Misc.IsSelected = source.IsSelected;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
