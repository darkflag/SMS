using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SPClient = Microsoft.SharePoint.Client;

namespace SPBrowser.Extentions
{
    public static class FieldUserValueExtentions
    {
        public static string GetDisplayValue(this SPClient.FieldUserValue value)
        {
            if (value == null)
                return string.Empty;

            return string.Format("{0};#{1}", value.LookupId, value.LookupValue);
        }
    }
}
