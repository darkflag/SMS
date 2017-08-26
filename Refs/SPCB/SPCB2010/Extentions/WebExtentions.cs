using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPClient = Microsoft.SharePoint.Client;

namespace SPBrowser.Extentions
{
    public static class WebExtentions
    {
        public static string GetWebUrl(this SPClient.Web web)
        {
            if (!web.IsPropertyAvailable("Title"))
            {
                web.Context.Load(web);
                web.Context.ExecuteQuery();
            }

            return string.Format("{0}/{1}", web.Context.Url, web.ServerRelativeUrl.ToLower().Replace(new Uri(web.Context.Url.ToLower()).AbsolutePath, ""));
        }

        public static string GetSiteSettingsUrl(this SPClient.Web web)
        {
            return web.GetWebUrl() + "/_layouts/settings.aspx";
        }

        public static bool ParseUser(this SPClient.Web web, string username, out SPClient.User user)
        {
            user = null;

            if (string.IsNullOrEmpty(username))
                return false;

            try 
            { 
                user = web.EnsureUser(username);

                if (!user.IsPropertyAvailable("LoginName"))
                {
                    web.Context.Load(user);
                    web.Context.ExecuteQuery();
                }
            }
            catch { return false; }
            
            return true;
        }
    }
}
