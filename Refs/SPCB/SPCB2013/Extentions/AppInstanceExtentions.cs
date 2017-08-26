using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SPClient = Microsoft.SharePoint.Client;

namespace SPBrowser.Extentions
{
    public static class AppInstanceExtentions
    {
        public static string GetAppRedirectUrl(this SPClient.AppInstance appInstance, TreeNode selectedNode)
        {
            // How do I use the appredirect page in the URL? You can use the app redirect page by URL-encoding, as follows:
            // https://sitecollection/web/_layouts/15/appredirect.aspx?instance_id=<AppClientId>&redirect_uri=<redirectURL>

            SPClient.Web web = (SPClient.Web)selectedNode.Parent.Parent.Tag;

            return string.Format("{0}/_layouts/15/appredirect.aspx?instance_id={1}",
                web.GetUrl(),
                appInstance.Id);
        }
    }
}
