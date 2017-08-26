using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPClient = Microsoft.SharePoint.Client;

namespace SPBrowser.Extentions
{
    public static class SiteExtentions
    {
        public static string GetSiteSettingsUrl(this SPClient.Site site)
        {
            return site.RootWeb.GetUrl() + "/_layouts/settings.aspx";
        }

        /// <summary>
        /// Returns the REST endpoint for current site.
        /// </summary>
        /// <param name="site"></param>
        /// <example>http://server/site/_api/web</example>
        /// <returns></returns>
        public static Uri GetRestUrl(this SPClient.Site site)
        {            
            return new Uri(string.Format("{0}/_api/site", site.Url));
        }

        /// <summary>
        /// Gets or sets a Boolean value that specifies whether the user is a site collection administrator.
        /// </summary>
        /// <param name="site"></param>
        /// <returns>true if the user is a site collection administrator; otherwise, false.</returns>
        public static bool IsCurrentUserAdmin(this SPClient.Site site)
        {
            bool isAdmin = false;

            site.Context.Load(site.RootWeb.CurrentUser);
            site.Context.ExecuteQuery();

            isAdmin = site.RootWeb.CurrentUser.IsSiteAdmin;

            return isAdmin;
        }
    }
}
