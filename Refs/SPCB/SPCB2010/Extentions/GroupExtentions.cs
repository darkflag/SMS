using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SPClient = Microsoft.SharePoint.Client;

namespace SPBrowser.Extentions
{
    public static class GroupExtentions
    {
        //public static string GetGroupUrl(this SPClient.Group group)
        //{
        //    return null;
        //}

        public static string GetSettingsUrl(this SPClient.Group group)
        {
            // <sitecollection|web>/_layouts/userdisp.aspx?ID=10

            Uri ctxUrl = new Uri(group.Context.Url.ToLower());
            return string.Format("{0}/_layouts/userdisp.aspx?ID={1}", ctxUrl.GetServerUrl(), group.Id);
        }
    }
}
