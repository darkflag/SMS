using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.Framework.Utilities.Extensions
{
    public static class UriExtensions
    {
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
    }
}
