using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SPClient = Microsoft.SharePoint.Client;

namespace SPBrowser.Extentions
{
    public static class FieldExtentions
    {
        public static string GetSettingsUrl(this SPClient.Field field, TreeNode selectedNode)
        {
            // <sitecollection|web>/_layouts/FldEditEx.aspx?field=Instructie
            // <sitecollection|web>/_layouts/FldEdit.aspx?List=%7BCEBB8CB0%2DC088%2D4BEE%2DBF17%2DE6A8CD5F6C9F%7D&Field=Title

            if (selectedNode.Parent.Parent.Tag is SPClient.Site)
            {
                // <sitecollection>/_layouts/15/fldedit.aspx?field=%5FEndDate&Source=%2F%5Flayouts%2F15%2Fmngfield%2Easpx%3FFilter%3DAll%2520Groups

                SPClient.Site site = selectedNode.Parent.Parent.Tag as SPClient.Site;
                return string.Format("{0}/_layouts/fldedit.aspx?field={1}", site.RootWeb.GetWebUrl(), field.InternalName);
            }
            else if (selectedNode.Parent.Parent.Tag is SPClient.Web)
            {
                // <sitecollection>/<web>/_layouts/15/fldedit.aspx?field=Sub%5Fx0020%5FSite%5Fx0020%5FColumn&Source=%2Fsub%2F%5Flayouts%2F15%2Fmngfield%2Easpx%3FFilter%3DAll%2520Groups
                
                SPClient.Web web = selectedNode.Parent.Parent.Tag as SPClient.Web;
                return string.Format("{0}/_layouts/fldedit.aspx?field={1}", web.GetWebUrl(), field.InternalName);
            }
            else if (selectedNode.Parent.Parent.Tag is SPClient.List)
            {
                // <sitecollection>/<web>/_layouts/15/FldEditEx.aspx?List=%7B051E4502%2D504E%2D49C8%2DA815%2DF46BFD61911D%7D&Field=Modified

                SPClient.List list = selectedNode.Parent.Parent.Tag as SPClient.List;
                return string.Format("{0}/_layouts/FldEditEx.aspx?list={1}&field={2}", list.ParentWeb.GetWebUrl(), list.Id, field.InternalName);
            }
            else
                return string.Empty;
        }
    }
}
