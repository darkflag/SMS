using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;

namespace SPBrowser.Utils
{
    public class ConfigUtil
    {
        /// <summary>
        /// Gets and/or creates the appSetting in the app.config.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetOrCreateAppSetting(string key, int defaultValue)
        {
            return int.Parse(GetOrCreateAppSetting(key, (object) defaultValue).ToString());
        }

        /// <summary>
        /// Gets and/or creates the appSetting in the app.config.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetOrCreateAppSetting(string key, string defaultValue = "")
        {
            return GetOrCreateAppSetting(key, (object) defaultValue).ToString();
        }

        /// <summary>
        /// Gets and/or creates the appSetting in the app.config.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool GetOrCreateAppSetting(string key, bool defaultValue)
        {
            return bool.Parse(GetOrCreateAppSetting(key, (object) defaultValue).ToString());
        }

        /// <summary>
        /// Gets and/or creates the appSetting in the app.config.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static LogLevel GetOrCreateAppSetting(string key, LogLevel defaultValue)
        {
            string value = GetOrCreateAppSetting(key, (object)defaultValue).ToString();

            LogLevel level = (LogLevel)Enum.Parse(typeof(LogLevel), value);

            return level;
        }

        /// <summary>
        /// Gets and/or creates the appSetting in the app.config.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private static object GetOrCreateAppSetting(string key, object defaultValue)
        {
            object value = null;

            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;

                if (settings[key] == null)
                {
                    if(defaultValue == null)
                        settings.Add(key, string.Empty);
                    else
                        settings.Add(key, defaultValue.ToString());
                    
                    configFile.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);

                    value = defaultValue;
                }
                else
                {
                    value = settings[key].Value;
                }

            }
            catch (ConfigurationErrorsException ex)
            {
                LogUtil.LogException(string.Format("Error writing app settings for key '{0}' with value '{1}'.", key, value), ex);
                throw;
            }

            return value;
        }

        /// <summary>
        /// Adds and/or updates the appSetting in the app.config.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddOrUpdateAppSetting(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;

                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }

                configFile.Save(ConfigurationSaveMode.Modified);

                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException ex)
            {
                LogUtil.LogException(string.Format("Error writing app settings for key '{0}' with value '{1}'.", key, value), ex);
                throw;
            }
        }

        /// <summary>
        /// Reads app settings from original app.config and migrates settings to current configuration.
        /// </summary>
        /// <param name="path">Filename of the original app.config.</param>
        public static void UpgradeAppSettingsAfterRename(string path)
        {
            // Check if app.config exists
            if (!System.IO.File.Exists(path))
                return;

            // Read app.config
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            LogUtil.LogMessage("Upgrading appSettings from {0}.", path);

            // Read appSettings from app.config and update current configuration
            foreach (XmlNode item in doc.SelectSingleNode("/configuration/appSettings").SelectNodes("add"))
            {
                string key = item.Attributes.GetNamedItem("key").Value;
                string value = item.Attributes.GetNamedItem("value").Value;

                AddOrUpdateAppSetting(key, value);

                LogUtil.LogMessage("Upgraded appSetting '{0}' with '{1}'.", key, value);
            }

            // Rename app.config after upgrade
            System.IO.File.Move(path, path + ".old");
        }
    }
}
