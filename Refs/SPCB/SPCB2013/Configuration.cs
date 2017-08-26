using SPBrowser.Entities;
using SPBrowser.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPBrowser
{
    /// <summary>
    /// All configuration for the use of the program are added below.
    /// ALL configuration is being read from the app.config (app settings) 
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// The current active <see cref="Configuration"/> object.
        /// </summary>
        public static Configuration Current
        {
            get
            {
                if (_current == null)
                    _current = new Configuration();

                return _current;
            }
        }
        private static Configuration _current;

        /// <summary>
        /// Indicates the number of log files to keep when truncating the logs directory.
        /// </summary>
        public int LogTruncateAfterNumberOfFiles
        {
            get { return _logTruncateAfterNumberOfFiles; }
            set { ConfigUtil.AddOrUpdateAppSetting(LOG_TRUNCATE_AFTER_NUMBER_OF_FILES_KEY, value.ToString()); _logTruncateAfterNumberOfFiles = value; }
        }
        private int _logTruncateAfterNumberOfFiles;
        private const string LOG_TRUNCATE_AFTER_NUMBER_OF_FILES_KEY = "LogTruncateAfterNumberOfFiles";

        /// <summary>
        /// Gets the last used version of the SharePoint Client Browser.
        /// </summary>
        public Version LastUsedVersion
        {
            get { return _lastUsedVersion; }
            set { ConfigUtil.AddOrUpdateAppSetting(LAST_USED_VERSION_KEY, value.ToString()); _lastUsedVersion = value; }
        }
        private Version _lastUsedVersion;
        private const string LAST_USED_VERSION_KEY = "LastUsedVersion";

        /// <summary>
        /// Enable check for updates on application startup.
        /// </summary>
        public bool CheckUpdatesOnStartup
        {
            get { return _checkUpdatesOnStartup; }
            set { ConfigUtil.AddOrUpdateAppSetting(CHECK_UPDATES_ON_STARTUP_KEY, value.ToString()); _checkUpdatesOnStartup = value; }
        }
        private bool _checkUpdatesOnStartup;
        private const string CHECK_UPDATES_ON_STARTUP_KEY = "CheckUpdatesOnStartup";

        /// <summary>
        /// When enabled it loads all object properties, which might need additional permissions from the current user.
        /// </summary>
        public bool LoadAllProperties
        {
            get { return _loadAllProperties; }
            set { ConfigUtil.AddOrUpdateAppSetting(LOAD_ALL_PROPERTIES_KEY, value.ToString()); _loadAllProperties = value; }
        }
        private bool _loadAllProperties;
        private const string LOAD_ALL_PROPERTIES_KEY = "LoadAllProperties";

        /// <summary>
        /// Defines the logging level, default set to <see cref="Utils.LogLevel.Exception"/>.
        /// </summary>
        public LogLevel LogLevel
        {
            get { return _logLevel; }
            set { ConfigUtil.AddOrUpdateAppSetting(LOG_LEVEL_KEY, value.ToString()); _logLevel = value; }
        }
        private LogLevel _logLevel;
        /// <summary>
        /// AppSettings key for the <see cref="LogLevel"/>.
        /// </summary>
        internal const string LOG_LEVEL_KEY = "LogLevel";

        /// <summary>
        /// Initiates the configuration object by loading all values from the app.config file.
        /// </summary>
        public Configuration()
        {
            _logTruncateAfterNumberOfFiles = ConfigUtil.GetOrCreateAppSetting(LOG_TRUNCATE_AFTER_NUMBER_OF_FILES_KEY, Constants.LOGS_TRUNCATE_AFTER_FILES);
            _lastUsedVersion = string.IsNullOrEmpty(ConfigUtil.GetOrCreateAppSetting(LAST_USED_VERSION_KEY)) ? null : new Version(ConfigUtil.GetOrCreateAppSetting(LAST_USED_VERSION_KEY));
            _checkUpdatesOnStartup = ConfigUtil.GetOrCreateAppSetting(CHECK_UPDATES_ON_STARTUP_KEY, true);
            Browser systemBrowser = BrowserUtil.GetSystemDefaultBrowser();
            _loadAllProperties = ConfigUtil.GetOrCreateAppSetting(LOAD_ALL_PROPERTIES_KEY, false);
            _logLevel = ConfigUtil.GetOrCreateAppSetting(LOG_LEVEL_KEY, LogLevel.Exception);
        }
    }
}
