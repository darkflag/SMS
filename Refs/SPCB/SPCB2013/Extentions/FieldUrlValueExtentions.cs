using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SPClient = Microsoft.SharePoint.Client;

namespace SPBrowser.Extentions
{
    public static class FieldUrlValueExtentions
    {
        public static string GetDisplayValue(this SPClient.FieldUrlValue value)
        {
            if (value == null)
                return string.Empty;

            return value.Url;
        }
    }
}
