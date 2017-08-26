using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SPClient = Microsoft.SharePoint.Client;

namespace SPBrowser.Extentions
{
    public static class FeatureExtentions
    {
        public static string GetFeatureName(this SPClient.Feature feature)
        {
            SPFeature f = SPHelp.GetFeature(feature.DefinitionId);

            if (f == null)
                return string.Format("{0} (unknown feature)", feature.DefinitionId);
            else
                return string.Format("{0} ({1})", f.DisplayName, f.InternalName);
        }

        public static bool IsHidden(this SPClient.Feature feature)
        {
            SPFeature f = SPHelp.GetFeature(feature.DefinitionId);

            if (f == null)
                return false;
            else
                return f.Hidden;
        }

        public static bool IsCustom(this SPClient.Feature feature)
        {
            SPFeature f = SPHelp.GetFeature(feature.DefinitionId);

            if (f == null)
                return false;
            else
                return f.IsCustomDefinition;
        }

        public static string GetSettingsUrl(this SPClient.Feature feature, TreeNode selectedNode)
        {
            // Link: <sitecollection|web>/_layouts/ManageFeatures.aspx
            // Link: <sitecollection>/_layouts/ManageFeatures.aspx?Scope=Site

            if (selectedNode.Parent.Parent.Tag is SPClient.Site)
            {
                SPClient.Site site = selectedNode.Parent.Parent.Tag as SPClient.Site;
                return string.Format("{0}/_layouts/ManageFeatures.aspx?Scope=Site", site.RootWeb.GetWebUrl());
            }
            else
            {
                SPClient.Web web = selectedNode.Parent.Parent.Tag as SPClient.Web;
                return string.Format("{0}/_layouts/ManageFeatures.aspx", web.GetWebUrl());
            }
        }
    }
}
