using Microsoft.Win32;
using SPBrowser.Entities;
using SPBrowser.Extentions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace SPBrowser.Utils
{
    /// <summary>
    /// Represents utility class for browsers.
    /// </summary>
    class BrowserUtil
    {
        private const string MICROSOFT_EDGE_PATH = "microsoft-edge:";

        /// <summary>
        /// Gets local installed browsers.
        /// </summary>
        /// <seealso cref="https://robbsadler.wordpress.com/2014/09/25/getting-installed-browsers-and-version-c/"/>
        /// <returns>Returns list of browsers.</returns>
        public static List<Browser> GetBrowsers()
        {
            //on 64bit the browsers are in a different location
            RegistryKey browserKeys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Clients\StartMenuInternet");

            if (browserKeys == null)
                browserKeys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet");

            string[] browserNames = browserKeys.GetSubKeyNames();

            var browsers = new List<Browser>();

            for (int i = 0; i < browserNames.Length; i++)
            {
                string browserName = browserNames[i];

                try
                {
                    RegistryKey browserKey = browserKeys.OpenSubKey(browserName);
                    RegistryKey browserKeyPath = browserKey.OpenSubKey(@"shell\open\command");
                    RegistryKey browserIconPath = browserKey.OpenSubKey(@"DefaultIcon");

                    string path = (string)browserKeyPath.GetValue(null).ToString().StripQuotes();
                    string iconPath = (string)browserIconPath.GetValue(null).ToString().StripQuotes();

                    Browser browser = new Browser()
                    {
                        Name = (string)browserKey.GetValue(null),
                        Path = path,
                        IconPath = iconPath,
                        SupportsInPrivate = SupportsBrowserPrivateMode(GetBrowserType(path)),
                        Type = GetBrowserType(path)
                    };

                    // Only add existing browsers
                    if (File.Exists(browser.Path))
                    {
                        browser.Version = FileVersionInfo.GetVersionInfo(browser.Path).FileVersion;

                        browsers.Add(browser);
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.LogException(string.Format("Error while retrieving '{0}' browser details. Browser is silently dropped while adding to Browser collection.", browserName), ex);
                }
            }

            //add Edge browser, when running Windows 7, 8, 8.1, 10
            string edgeBrowserExecutable = $"{Environment.GetFolderPath(Environment.SpecialFolder.Windows)}\\SystemApps\\Microsoft.MicrosoftEdge_8wekyb3d8bbwe\\MicrosoftEdge.exe";
            if (File.Exists(edgeBrowserExecutable))
            {
                browsers.Add(new Browser()
                {
                    Name = "Edge",
                    Path = MICROSOFT_EDGE_PATH,
                    IconPath = edgeBrowserExecutable,
                    SupportsInPrivate = false,
                    Type = BrowserType.Edge
                });
            }

            return browsers;
        }

        /// <summary>
        /// Gets the browser by name.
        /// </summary>
        /// <param name="name">Display name of the browser to retrieve.</param>
        /// <returns>Returns the browser object, otherwise null.</returns>
        public static Browser GetBrowser(string name)
        {
            List<Browser> browsers = GetBrowsers();

            return browsers.SingleOrDefault(b => b.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Gets the default browser.
        /// </summary>
        /// <returns></returns>
        public static Browser GetSystemDefaultBrowser()
        {
            Browser browser = null;
            RegistryKey regKey = null;

            try
            {
                //set the registry key we want to open
                regKey = Registry.ClassesRoot.OpenSubKey("HTTP\\shell\\open\\command", false);

                //get rid of the enclosing quotes
                string path = regKey.GetValue(null).ToString().ToLower().Replace("" + (char)34, "");

                //check to see if the value ends with .exe (this way we can remove any command line arguments)
                if (!path.EndsWith("exe"))
                    //get rid of all command line arguments (anything after the .exe must go)
                    path = path.Substring(0, path.LastIndexOf(".exe") + 4);

                List<Browser> browsers = GetBrowsers();
                browser = browsers.SingleOrDefault(b => b.Path.Equals(path, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception ex)
            {
                LogUtil.LogException("Error while retrieving default browser setting.", ex);
            }
            finally
            {
                //check and see if the key is still open, if so
                //then close it
                if (regKey != null)
                    regKey.Close();
            }

            //return the value
            return browser;
        }

        /// <summary>
        /// Opens URL in browser.
        /// </summary>
        /// <param name="useAlternativeBrowser">Use the alternative browser configured in the <see cref="Configuration"/> object.</param>
        /// <param name="url">URL to open in browser.</param>
        public static void StartBrowserWithUrl(Browser browser, string url, bool inPrivateMode = false)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(url, "URL is empty, can't start browser without URL.");
            }

            // Support Edge browser and other browsers
            if (browser.Path == MICROSOFT_EDGE_PATH)
            {
                Process.Start(browser.Path + url);
            }
            else
            {
                if (!File.Exists(browser.Path))
                {
                    string message = string.Format("Can't locate the browser executable '{0}'.", browser.Path);
                    LogUtil.LogMessage(message, LogLevel.Exception);
                    throw new ApplicationException(message);
                }

                string arguments = GetExecutableArguments(browser.Type, url, inPrivateMode);

                // Start browser with arguments like url and private mode
                Process.Start(browser.Path, arguments);
            }

        }

        /// <summary>
        /// Gets the arguments for opening the browser, either in private mode or not.
        /// </summary>
        /// <param name="browser">Path to the browser executable (.exe) in Program Files.</param>
        /// <param name="privateMode">Run the browser in private mode?</param>
        /// <param name="url">URL to launch.</param>
        /// <remarks>
        /// Start in Private browsing...
        ///  Internet Explorer: "C:\Program Files (x86)\Internet Explorer\iexplore.exe" URL -private
        ///  Google Chrome: "C:\Program Files (x86)\Google\Chrome\Application\chrome.exe" URL --incognito
        ///  Mozilla Firefox: "C:\Program Files (x86)\Mozilla Firefox\firefox.exe" -private-window URL
        /// </remarks>
        private static string GetExecutableArguments(BrowserType browser, string url, bool privateMode)
        {
            string args = string.Empty;

            switch (browser)
            {
                case BrowserType.InternetExplorer:
                    args = string.Format("{0} {1}", url, privateMode ? "-private" : string.Empty);
                    break;
                case BrowserType.Chrome:
                    args = string.Format("{0} {1}", url, privateMode ? "--incognito" : string.Empty);
                    break;
                case BrowserType.FirefoxMozilla:
                    args = string.Format("{1} {0}", url, privateMode ? "-private-window" : string.Empty);
                    break;
                default:
                    break;
            }

            return args.Trim();
        }

        /// <summary>
        /// Is private mode supported by the browser.
        /// </summary>
        /// <param name="browserPath">Path to the browser executable (.exe) in Program Files.</param>
        /// <returns>Returns TRUE when private mode is supported, based on the executable path. Else FALSE.</returns>
        public static bool SupportsBrowserPrivateMode(BrowserType browser)
        {
            switch (browser)
            {
                case BrowserType.InternetExplorer:
                case BrowserType.Chrome:
                case BrowserType.FirefoxMozilla:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Gets the <see cref="Icon"/> for the browser.
        /// </summary>
        /// <param name="browser"></param>
        /// <returns></returns>
        public static Icon GetBrowserIcon(Browser browser)
        {
            string path = browser.IconPath;

            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            if (path.Contains(','))
                path = path.Split(',')[0];

            if (File.Exists(path))
                return Icon.ExtractAssociatedIcon(path);
            else
                return null;
        }

        /// <summary>
        /// Gets the type of the browser.
        /// </summary>
        /// <param name="browserPath">The browser path.</param>
        /// <returns></returns>
        public static BrowserType GetBrowserType(string browserPath)
        {
            switch (Path.GetFileName(browserPath).ToLower())
            {
                case "iexplore.exe":
                    return BrowserType.InternetExplorer;
                case "chrome.exe":
                    return BrowserType.Chrome;
                case "firefox.exe":
                    return BrowserType.FirefoxMozilla;
                default:
                    return BrowserType.NonSupported;
            }
        }
    }
}