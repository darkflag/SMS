using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SPClient = Microsoft.SharePoint.Client;

namespace SPBrowser.Extentions
{
    public static class RoleDefinitionExtentions
    {
        //public static string GetUrl(this SPClient.File file)
        //{
        //    return null;
        //}

        public static string GetSettingsUrl(this SPClient.RoleDefinition role)
        {
            // [sitecollection|web]/_layouts/15/editrole.aspx?role=Full%20Control

            return string.Format("{0}/_layouts/15/editrole.aspx?role={1}", role.Context.Url, role.Name);
        }
    }
}
