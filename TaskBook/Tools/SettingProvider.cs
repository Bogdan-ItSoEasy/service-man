using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TaskBook.Tools
{
    static class SettingProvider
    {
        public static void SetSetting(string setting, string value)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = config.AppSettings.Settings;

            if (settings[setting] == null)
                settings.Add(setting, value);
            else
                settings[setting].Value = value;

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
            OnSettingsUpdate();
        }

        public static string GetSetting(string setting)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = config.AppSettings.Settings;
            
            return settings[setting] != null? settings[setting].Value : "";
        }

        public static void SetAutoRun(string autorun, string value)
        {
            var oldValue = ConfigurationManager.AppSettings["autorun"];


            if (oldValue == null || value != oldValue)
            {
                SetSetting("autorun", value);
                UpdateAutorun();
            }
        }
        
        public static void UpdateAutorun()
        {
            RegProvider.SetAutoRunProperty();
        }

        public static event EventHandler SettingsUpdate;

        private static void OnSettingsUpdate()
        {
            SettingsUpdate?.Invoke(null, EventArgs.Empty);
        }
    }
}
