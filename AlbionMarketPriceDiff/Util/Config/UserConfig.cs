using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using AlbionMarketPriceDiff.Model;

namespace AlbionMarketPriceDiff.Util.Config
{
    public class UserConfig : RaisesPropertyChanged
    {
        private readonly string _configPath;

        public UserConfig(string configPath_)
        {
            _configPath = configPath_;
            Config = LoadConfig(_configPath);
        }

        public ConfigModel Config { get; private set; }

        public IEnumerable<SelectableValue<string>> Cities
        {
            get => Config.Cities;
            set
            {
                Config.Cities = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<(SelectableValue<string>, string)> Resources
        {
            get => Config.Resources;
            set
            {
                Config.Resources = value;
                OnPropertyChanged();
            }
        }

        public bool IsPremium
        {
            get => Config.IsPremium;
            set
            {
                Config.IsPremium = value;
                OnPropertyChanged();
            }
        }
        public int DefaultMaxDateInHours
        {
            get => Config.DefaultMaxDateInHours;
            set
            {
                Config.DefaultMaxDateInHours = value;
                OnPropertyChanged();
            }
        }

        // Todo: ordering type

        public void SaveConfig()
        {
            PersistConfig(Config);
        }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            PersistConfig(Config);
        }

        private ConfigModel LoadConfig(string configPath_)
        {
            if (!File.Exists(_configPath))
            {
                PersistConfig(ConfigModel.BrandNewConfig);
            }
            return File.ReadAllText(configPath_).FromJson<ConfigModel>();
        }

        private void PersistConfig(ConfigModel config_)
        {
            // todo: make it so that json does not save SelectableValue as its type but as its content for Resources
            var json = config_.ToJson();
            File.WriteAllText(_configPath, json);
        }
    }
}
