using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SPClient = Microsoft.SharePoint.Client;

namespace SPBrowser.Extentions
{
    public static class FolderExtentions
    {
        public static string GetFolderUrl(this SPClient.Folder folder)
        {
            Uri ctxUrl = new Uri(folder.Context.Url.ToLower());

            if (!folder.IsPropertyAvailable("ServerRelativeUrl"))
            {
                folder.Context.Load(folder, f => f.ServerRelativeUrl);
                folder.Context.ExecuteQuery();
            }

            return string.Format("{0}{1}", ctxUrl.GetServerUrl(), folder.ServerRelativeUrl);
        }

        //public static string GetSettingsUrl(this SPClient.Folder folder, TreeNode selectedNode)
        //{
        //    return null;
        //}
    }
}
