using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SPClient = Microsoft.SharePoint.Client;

namespace SPBrowser.Extentions
{
    public static class UserExtentions
    {
        //public static string GetUserUrl(this SPClient.User user)
        //{
        //    return null;
        //}

        public static string GetSettingsUrl(this SPClient.User user)
        {
            // <sitecollection|web>/_layouts/userdisp.aspx?ID=10
            return string.Format("{0}/_layouts/userdisp.aspx?ID={1}", user.Context.Url, user.Id);
        }
    }
}
