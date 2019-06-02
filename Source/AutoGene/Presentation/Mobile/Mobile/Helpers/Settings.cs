// Helpers/Settings.cs
using AutoGene.Mobile.Enums;
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace AutoGene.Mobile.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings ApplicationSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;

        #endregion


        public static string GeneralSettings
        {
            get
            {
                return ApplicationSettings.GetValueOrDefault<string>(SettingsKey, SettingsDefault);
            }
            set
            {
                ApplicationSettings.AddOrUpdateValue<string>(SettingsKey, value);
            }
        }

        public static T GetSetting<T>(AppSettings setting, T defaultValue = default(T))
        {
            return GetSetting(setting.ToString(), defaultValue);
        }

        public static void SetSetting<T>(AppSettings setting, T value)
        {
            SetSetting(setting.ToString(), value);
        }

        public static void RemoveSetting(AppSettings setting)
        {
            RemoveSetting(setting.ToString());
        }

        public static T GetSetting<T>(string key, T defaultValue = default(T))
        {
            var settingStringValue = ApplicationSettings.GetValueOrDefault<string>(key, string.Empty);

            if (string.IsNullOrEmpty(settingStringValue))
                return defaultValue;

            return JsonConvert.DeserializeObject<T>(settingStringValue);
        }

        public static void SetSetting<T>(string key, T value)
        {
            string stringValue;

            if (value == null)
                stringValue = string.Empty;
            else
                stringValue = JsonConvert.SerializeObject(value);

            ApplicationSettings.AddOrUpdateValue(key, stringValue);
        }

        public static void RemoveSetting(string key)
        {
            ApplicationSettings.Remove(key);
        }

    }
}