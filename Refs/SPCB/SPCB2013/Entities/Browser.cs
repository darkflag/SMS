using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPBrowser.Entities
{
    /// <summary>
    /// Represents a installed browser client.
    /// </summary>
    public class Browser
    {
        /// <summary>
        /// Display name of browser.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Path to executable.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Icon path with index of icon at the end.
        /// </summary>
        public string IconPath { get; set; }

        /// <summary>
        /// Browser version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Supports the browser in private mode?
        /// </summary>
        public bool SupportsInPrivate { get; set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public BrowserType Type { get; set; }
    }

    public enum BrowserType
    {
        Chrome,
        InternetExplorer,
        Edge,
        FirefoxMozilla,
        NonSupported
    }
}
