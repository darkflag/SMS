using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPBrowser.Extentions
{
    public static class UriExtentions
    {
        /// <summary>
        /// Returns the URL of the host/server.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Uri GetServerUrl(this Uri url)
        {
            return new Uri(string.Format("{0}://{1}{2}",
                                url.Scheme,
                                url.Host,
                                url.Port == 80 || url.Port == 443 ? "" : string.Format(":{0}", url.Port)
                ));
        }

        /// <summary>
        /// Removes the trailing forward-slash at the end.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Uri RemoveTrailingSlash(this Uri url)
        {
            // Remove trailing forward-slash
            if (url.ToString().EndsWith("/"))
                url = new Uri(url.ToString().Remove(url.ToString().Length - 1));

            return url;
        }

        /// <summary>
        /// Adds a relative URL to this URL.
        /// </summary>
        /// <param name="baseUri">Extended URI.</param>
        /// <param name="relativeUrl">Relative URL to add to current URI.</param>
        /// <returns>Returns full URL.</returns>
        public static string Combine(this Uri baseUri, string relativeUrl)
        {
            string siteCollectionUrl = string.Format("{0}://{1}{2}/",
                baseUri.Scheme,
                baseUri.DnsSafeHost,
                baseUri.LocalPath.Equals("/") ? string.Empty : baseUri.LocalPath);

            string webUrl = relativeUrl.StartsWith("/") ? relativeUrl.Substring(1) : relativeUrl;

            Uri url = new Uri(new Uri(siteCollectionUrl), webUrl);

            return url.ToString();
        }
    }
}
