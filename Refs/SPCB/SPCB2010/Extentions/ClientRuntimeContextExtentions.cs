using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPClient = Microsoft.SharePoint.Client;

namespace SPBrowser.Extentions
{
    public static class ClientRuntimeContextExtentions
    {
        /// <summary>
        /// Returns true when connected site is at least the provided SharePoint version.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static bool IsMinimalServerVersion(this SPClient.ClientRuntimeContext context, ServerVersion version)
        {
            if (context.ServerSchemaVersion.Major < (int)version)
                return false;
            else
                return true;
        }
    }

    public enum ServerVersion
    {
        SharePoint2010 = 14,
        SharePoint2013 = 15
    }
}
