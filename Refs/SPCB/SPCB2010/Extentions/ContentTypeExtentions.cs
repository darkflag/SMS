using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SPClient = Microsoft.SharePoint.Client;

namespace SPBrowser.Extentions
{
    public static class ContentTypeExtentions
    {
        public static string GetSettingsUrl(this SPClient.ContentType ct, TreeNode selectedNode)
        {
            // Link: <sitecollection|web>/_layouts/ManageContentType.aspx?ctype=0x0101009148F5A04DDD49CBA7127AADA5FB792B006973ACD696DC4858A76371B2FB2F439A
            // Link: <sitecollection|web>/_layouts/ManageContentType.aspx?List=%7BF798C4D9%2DEF29%2D4F8D%2DA1F1%2D4C70CFBAECE4%7D&ctype=0x010100A204AFB24228A94AB2D6195EB1705291

            if (selectedNode.Parent.Parent.Tag is SPClient.Site)
            {
                SPClient.Site site = selectedNode.Parent.Parent.Tag as SPClient.Site;
                return string.Format("{0}/_layouts/ManageContentType.aspx?ctype={1}", site.RootWeb.GetWebUrl(), ct.Id);
            }
            else if (selectedNode.Parent.Parent.Tag is SPClient.Web)
            {
                // <sitecollection>/<web>/_layouts/15/ManageContentType.aspx?ctype=0x00A7470EADF4194E2E9ED1031B61DA088403000BE6CEFFF1ACA6429D14B2B7E0A03FE2
                // <sitecollection>/<web>/_layouts/15/start.aspx#/_layouts/15/ManageContentType.aspx?ctype=0x00A7470EADF4194E2E9ED1031B61DA088403000BE6CEFFF1ACA6429D14B2B7E0A03FE2&Source=https%3A%2F%2Fbramdejager%2Esharepoint%2Ecom%2Fsub%2F%5Flayouts%2F15%2Fmngctype%2Easpx

                SPClient.Web web = selectedNode.Parent.Parent.Tag as SPClient.Web;
                return string.Format("{0}/_layouts/ManageContentType.aspx?ctype={1}", web.GetWebUrl(), ct.Id);
            }
            else if (selectedNode.Parent.Parent.Tag is SPClient.List)
            {
                SPClient.List list = selectedNode.Parent.Parent.Tag as SPClient.List;
                return string.Format("{0}/_layouts/ManageContentType.aspx?list={1}&ctype={2}", list.ParentWeb.GetWebUrl(), list.Id, ct.Id);
            }
            else
                return string.Empty;
        }
    }
}
