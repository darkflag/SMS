using Microsoft.SharePoint.Client.UserProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPClient = Microsoft.SharePoint.Client;

namespace SPBrowser.Extentions
{
    public static class PersonPropertiesExtentions
    {
        public static string GetUserProfileUrl(this PersonProperties props)
        {
            if (props.IsPropertyAvailable("UserUrl"))
                return props.UserUrl;
            else
                return string.Empty;
        }

        //public static string GetSiteSettingsUrl(this PersonProperties props)
        //{
        //    return props.GetWebUrl() + "/_layouts/settings.aspx";
        //}
    }
}
