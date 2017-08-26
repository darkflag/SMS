using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace SPBrowser.Utils
{
    public class ProductUtil
    {
        private const string RELEASE_PREFIX = "Released:";
        private const string RELEASE_RSS_FEED_URL = "https://spcb.codeplex.com/project/feeds/rss?ProjectRSSFeed=codeplex%3a%2f%2frelease%2fspcb";

        /// <summary>
        /// Checks if new version is available.
        /// </summary>
        /// <returns></returns>
        public static bool IsNewUpdateAvailable(out Version newVersion, out Uri downloadUrl, out string updateTitle)
        {
            Regex regVersion = new Regex(@"v([0-9]|\.)+");

            try
            {
                foreach (var feedItem in GetReleases())
                {
                    Match result = regVersion.Match(feedItem.Title.Text);

                    if (result.Success)
                    {
                        Version version = new Version(result.Value.Replace('v', ' '));
                        if (version > GetCurrentProductVersion())
                        {
                            newVersion = version;
                            downloadUrl = feedItem.Links[0].Uri;
                            updateTitle = feedItem.Title.Text.Replace(RELEASE_PREFIX, "").Trim();

                            return true;
                        }
                    }
                }
            }
            catch (Exception) { }

            newVersion = null;
            downloadUrl = null;
            updateTitle = null;

            return false;
        }

        /// <summary>
        /// Gets the list of releases for this product from Codeplex.
        /// </summary>
        /// <returns></returns>
        private static List<SyndicationItem> GetReleases()
        {
            var feedUrl = RELEASE_RSS_FEED_URL;
            var reader = XmlReader.Create(feedUrl);
            var feed = SyndicationFeed.Load(reader);

            return feed.Items.Where(f =>
                    f.Title.Text.StartsWith(RELEASE_PREFIX) &&
                    f.Title.Text.Contains(GetProductVersionInfo().FileDescription))
                .ToList();
        }

        /// <summary>
        /// Gets the current product version.
        /// </summary>
        /// <returns></returns>
        public static Version GetCurrentProductVersion()
        {
            FileVersionInfo fvi = GetProductVersionInfo();

            return new Version(fvi.FileVersion);
        }

        /// <summary>
        /// Gets the File Version Information for the current assembly.
        /// </summary>
        /// <returns></returns>
        public static FileVersionInfo GetProductVersionInfo()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi;
        }
    }
}
