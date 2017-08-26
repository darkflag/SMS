using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPClient = Microsoft.SharePoint.Client;

namespace SPBrowser.Extentions
{
    public static class ViewExtentions
    {
        public static string GetViewUrl(this SPClient.View view)
        {
            Uri ctxUrl = new Uri(view.Context.Url.ToLower());
            return string.Format("{0}{1}", ctxUrl.GetServerUrl(), view.ServerRelativeUrl);
        }

        //public static string GetSettingsUrl(this SPClient.View view)
        //{
        //    // <sitecollection|web>/_layouts/??
        //    Uri ctxUrl = new Uri(view.Context.Url.ToLower());

        //    return string.Format("{0}{1}/_layouts/listedit.aspx?List={2}",
        //        ctxUrl.GetServerUrl(),
        //        view.ParentWebUrl,
        //        view.Id);
        //}
    }
}
