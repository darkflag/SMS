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
                return string.Format("{0}/_layouts/fldedit.aspx?field={1}", site.RootWeb.GetUrl(), field.InternalName);
            }
            else if (selectedNode.Parent.Parent.Tag is SPClient.Web)
            {
                // <sitecollection>/<web>/_layouts/15/fldedit.aspx?field=Sub%5Fx0020%5FSite%5Fx0020%5FColumn&Source=%2Fsub%2F%5Flayouts%2F15%2Fmngfield%2Easpx%3FFilter%3DAll%2520Groups
                
                SPClient.Web web = selectedNode.Parent.Parent.Tag as SPClient.Web;
                return string.Format("{0}/_layouts/fldedit.aspx?field={1}", web.GetUrl(), field.InternalName);
            }
            else if (selectedNode.Parent.Parent.Tag is SPClient.List)
            {
                // <sitecollection>/<web>/_layouts/15/FldEditEx.aspx?List=%7B051E4502%2D504E%2D49C8%2DA815%2DF46BFD61911D%7D&Field=Modified

                SPClient.List list = selectedNode.Parent.Parent.Tag as SPClient.List;
                return string.Format("{0}/_layouts/FldEditEx.aspx?list={1}&field={2}", list.ParentWeb.GetUrl(), list.Id, field.InternalName);
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// Returns the REST endpoint for current field.
        /// </summary>
        /// <param name="field"></param>
        /// <example>http://server/site/_api/web/Fields</example>
        /// <example>http://server/site/_api/web/AvailableFields</example>
        /// <example>http://server/site/_api/web/AvailableFields(guid'56747800-d36e-4625-abe3-b1bc74a7d5f8')</example>
        /// <example>http://server/site/_api/web/lists(guid'81c57897-96ad-4b39-94d4-092be56d562d')/fields(guid'1d22ea11-1e32-424e-89ab-9fedbadb6ce1')</example>
        /// <returns></returns>
        public static Uri GetRestUrl(this SPClient.Field field, TreeNode selectedNode)
        {
            if (selectedNode.Parent.Parent.Tag is SPClient.Site)
            {
                SPClient.Site site = selectedNode.Parent.Parent.Tag as SPClient.Site;
                return new Uri(string.Format("{0}/_api/Web/AvailableFields(guid'{1}')", site.RootWeb.GetUrl(), field.Id));
            }
            else if (selectedNode.Parent.Parent.Tag is SPClient.Web)
            {
                SPClient.Web web = selectedNode.Parent.Parent.Tag as SPClient.Web;
                return new Uri(string.Format("{0}/_api/Web/AvailableFields(guid'{1}')", web.GetUrl(), field.Id));
            }
            else if (selectedNode.Parent.Parent.Tag is SPClient.List)
            {
                SPClient.List list = selectedNode.Parent.Parent.Tag as SPClient.List;
                return new Uri(string.Format("{0}/_api/Web/Lists(guid'{1}')/Fields(guid'{2}')", list.ParentWeb.GetUrl(), list.Id, field.Id));
            }
            else
                return null;
        }
    }
}
