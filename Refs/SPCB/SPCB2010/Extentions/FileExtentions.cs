using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SPClient = Microsoft.SharePoint.Client;

namespace SPBrowser.Extentions
{
    public static class FileExtentions
    {
        public static string GetFileUrl(this SPClient.File file)
        {
            Uri ctxUrl = new Uri(file.Context.Url.ToLower());
            return string.Format("{0}{1}", ctxUrl.GetServerUrl(), file.ServerRelativeUrl);
        }

        //public static string GetSettingsUrl(this SPClient.Folder folder, TreeNode selectedNode)
        //{
        //    return null;
        //}
    }
}
