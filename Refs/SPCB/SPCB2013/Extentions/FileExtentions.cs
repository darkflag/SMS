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
        public static string GetUrl(this SPClient.File file)
        {
            Uri ctxUrl = new Uri(file.Context.Url.ToLower());
            return string.Format("{0}{1}", ctxUrl.GetServerUrl(), file.ServerRelativeUrl);
        }

        /// <summary>
        /// Checks if the file is an ASPX file?
        /// </summary>
        /// <param name="file"></param>
        /// <returns>Returns TRUE if the file is an ASPX file extention, else FALSE.</returns>
        public static bool IsAspxPage(this SPClient.File file)
        {
            return file.Name.EndsWith(".aspx", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
