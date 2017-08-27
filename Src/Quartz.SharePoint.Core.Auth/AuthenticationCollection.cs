using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.SharePoint.Core.Auth
{
    public class AuthenticationCollection : List<Authentication>
    {
        /// <summary>
        /// Gets the <see cref="Authentication"/> based on the <paramref name="siteUrl"/>.
        /// </summary>
        /// <remarks>
        /// Only retrieves <see cref="Authentication"/> objects where the <see cref="SiteAuthentication.IsLoaded"/> is set to <see cref="True"/>.
        /// </remarks>
        /// <param name="siteUrl">Url for the site-collection to retrieve.</param>
        /// <returns>Returns the <see cref="Authentication"/> referred by the <paramref name="siteUrl"/>.</returns>
        /// <exception cref="ArgumentNullException">Raised when <paramref name="siteUrl"/> is null or empty.</exception>
        public Authentication this[string siteUrl]
        {
            get
            {
                if (string.IsNullOrEmpty(siteUrl))
                    throw new ArgumentNullException(siteUrl);
                return this.SingleOrDefault(x => Uri.Compare(x.Url, new Uri(siteUrl), UriComponents.HttpRequestUrl, UriFormat.SafeUnescaped, StringComparison.InvariantCultureIgnoreCase) == 0);
            }
        }
    }
}
