using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPClient = Microsoft.SharePoint.Client;

namespace SPBrowser.Extentions
{
    public static class ListExtentions
    {
        public static string GetListUrl(this SPClient.List list)
        {
            return list.RootFolder.GetFolderUrl();
        }

        public static string GetSettingsUrl(this SPClient.List list)
        {
            // <sitecollection|web>/_layouts/listedit.aspx?List=%7B66FF645D%2D2BCA%2D40FD%2DB560%2D1EF157C8B6E7%7D
            Uri ctxUrl = new Uri(list.Context.Url.ToLower());

            return string.Format("{0}{1}/_layouts/listedit.aspx?List={2}",
                ctxUrl.GetServerUrl(),
                list.ParentWebUrl,
                list.Id);
        }

        /// <summary>
        /// Returns the WebDav URL for the current list.
        /// </summary>
        /// <param name="web"></param>
        /// <remarks>A webdav URL looks like: \\webapplicationurl@SSL\DavWWWRoot\sites\sitecollection</remarks>
        /// <returns></returns>
        public static string GetListWebDavUrl(this SPClient.List list)
        {
            Uri listUri = new Uri(list.GetListUrl());
            string webDavUrl = string.Format("\\\\{0}@SSL\\DavWWWRoot{1}", listUri.DnsSafeHost, listUri.AbsolutePath.Replace('/', '\\'));

            return webDavUrl;
        }

        /// <summary>
        /// Returns the REST endpoint for current web.
        /// </summary>
        /// <param name="list"></param>
        /// <example>http://server/site/_api/web</example>
        /// <returns></returns>
        public static Uri GetRestUrl(this SPClient.List list)
        {
            return new Uri(string.Format("{0}/_api/web/lists(guid'{1}')", list.ParentWebUrl, list.Id));
        }

        /// <summary>
        /// Gets the file name for the <see cref="SPClient.List.ImageUrl"/>
        /// </summary>
        /// <param name="list"></param>
        /// <returns>Returns the file name for the list image.</returns>
        public static string GetImageUrlFileName(this SPClient.List list)
        {
            string filename = System.IO.Path.GetFileName(list.ImageUrl);

            if (filename.Contains('?'))
                filename = filename.Remove(System.IO.Path.GetFileName(list.ImageUrl).IndexOf('?'));

            return filename;
        }
    }
}
