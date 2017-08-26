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
    }
}
