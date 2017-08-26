using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPClient = Microsoft.SharePoint.Client;

namespace SPBrowser.Extentions
{
    public static class WebExtentions
    {
        /// <summary>
        /// Gets the URL for the current web.
        /// </summary>
        /// <param name="web"></param>
        /// <returns></returns>
        public static string GetUrl(this SPClient.Web web)
        {
            if (!web.IsPropertyAvailable("Url"))
            {
                web.Context.Load(web);
                web.Context.ExecuteQuery();
            }

            return web.Url;
        }

        /// <summary>
        /// Gets the Site Settings URL for the current web.
        /// </summary>
        /// <param name="web"></param>
        /// <returns>Returns Site Settings URL.</returns>
        public static string GetSettingsUrl(this SPClient.Web web)
        {
            return web.GetUrl() + "/_layouts/settings.aspx";
        }

        /// <summary>
        /// Returns the WebDav URL for the current web.
        /// </summary>
        /// <param name="web"></param>
        /// <remarks>A webdav URL looks like: \\webapplicationurl@SSL\DavWWWRoot\sites\sitecollection</remarks>
        /// <returns></returns>
        public static string GetSiteWebDavUrl(this SPClient.Web web)
        {
            Uri webUri = new Uri(web.GetUrl());
            string webDavUrl = string.Format("\\\\{0}@SSL\\DavWWWRoot{1}", webUri.DnsSafeHost, webUri.AbsolutePath.Replace('/','\\'));

            return webDavUrl;
        }

        /// <summary>
        /// Returns true, if the current web is an App web
        /// </summary>
        /// <param name="web"></param>
        /// <returns></returns>
        public static bool IsAppWeb(this SPClient.Web web)
        {
            return web.IsPropertyAvailable("AppInstanceId") && web.AppInstanceId != Guid.Empty;
        }

        /// <summary>
        /// Returns the REST endpoint for current web.
        /// </summary>
        /// <param name="web"></param>
        /// <example>http://server/site/_api/web</example>
        /// <returns></returns>
        public static Uri GetRestUrl(this SPClient.Web web)
        {
            return new Uri(string.Format("{0}/_api/web", web.GetUrl()));
        }
    }
}
