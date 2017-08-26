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
            if (url.OriginalString.EndsWith("/"))
                url = new Uri(url.OriginalString.Remove(url.OriginalString.Length - 1));

            return url;
        }
    }
}
