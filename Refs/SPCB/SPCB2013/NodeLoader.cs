using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.InformationPolicy;
using Microsoft.SharePoint.Client.Search.Query;
using Microsoft.SharePoint.Client.Taxonomy;
using Microsoft.SharePoint.Client.UserProfiles;
using SPBrowser.Entities;
using SPBrowser.Extentions;
using SPBrowser.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using SPClient = Microsoft.SharePoint.Client;
using SPTenantAdmin = Microsoft.Online.SharePoint.TenantAdministration;
using SPTenantMngt = Microsoft.Online.SharePoint.TenantManagement;

namespace SPBrowser
{
    public static class NodeLoader
    {
        private const char DELIMITER = '\r';

        public delegate void ItemLoadedEventHandler(object sender, ItemLoadedEventArgs e);
        public static event ItemLoadedEventHandler ItemLoaded;
        public delegate void UserProfilesBatchCompletedEventHandler(object sender, BatchCompletedEventArgs e);
        public static event UserProfilesBatchCompletedEventHandler UserProfilesBatchCompleted;

        public static void LoadTenant(TreeNode parentNode, SPClient.ClientContext ctx, MainForm form)
        {
            try
            {
                SPTenantAdmin.Tenant tenant = new SPTenantAdmin.Tenant(ctx);
                ctx.Load(tenant);
                ctx.ExecuteQuery();

                // Load tenant node
                TreeNode tenantNode = AddTreeNode(parentNode, ctx.Url, tenant, Constants.IMAGE_TENANT, "Represents a SharePoint Online tenant.", form.mnContextTenant);

                // Load Office 365 Tenant node
                TreeNode rootWebNode = LoadOffice365Tenant(tenantNode, ctx, form);

                // Add loading nodes
                AddLoadingNode(tenantNode, "Site Collections", "Returns all sites in the tenant.", Constants.IMAGE_SITE, NodeLoadType.Sites);
                AddLoadingNode(tenantNode, "Site Properties", "Returns the properties for all sites in the tenant.", Constants.IMAGE_SITE_PROPERTIES, NodeLoadType.SiteProperties);
                AddLoadingNode(tenantNode, "Deleted Site Properties", "Returns a list of properties of deleted sites for the current tenant.", Constants.IMAGE_DELETED_SITE_PROPERTIES, NodeLoadType.DeletedSiteProperties);
                AddLoadingNode(tenantNode, "App Catalog", "Shows all apps within the App Catalog.", Constants.IMAGE_APP_INSTANCES, NodeLoadType.Apps);
                AddLoadingNode(tenantNode, "Web Templates", "Returns all of the web templates for the site collections in the tenant.", Constants.IMAGE_WEB_TEMPLATES, NodeLoadType.TenantWebTemplates);
                AddLoadingNode(tenantNode, "Log Entries", "Retrieves the logs for this tenant, currently not implemented by Microsoft.", Constants.IMAGE_TENANT_ERROR, NodeLoadType.LogEntries);

                // Expand tenant node
                tenantNode.Expand();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        public static TreeNode LoadOffice365Tenant(TreeNode parentNode, SPClient.ClientContext ctx, MainForm form)
        {
            TreeNode tenantNode = null;

            try
            {
                SPTenantMngt.Office365Tenant tenant = new SPTenantMngt.Office365Tenant(ctx);
                ctx.Load(tenant);
                ctx.ExecuteQuery();

                // Load tenant node
                tenantNode = AddTreeNode(parentNode, "Office 365 Tenant", tenant, Constants.IMAGE_TENANT, "Represents a Office 365 tenant.", form.mnContextItem);

                // Add loading nodes
                AddLoadingNode(tenantNode, "External Users", "Returns all external users in the tenant.", Constants.IMAGE_EXTERNAL_USERS, NodeLoadType.ExternalUsers);

                // Expand tenant node
                tenantNode.Expand();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }

            return tenantNode;
        }

        public static TreeNode LoadSites(TreeNode parentNode, SPTenantAdmin.Tenant tenant, MainForm form)
        {
            TreeNode siteNode = null;

            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);

                // Add site collections
                SPTenantAdmin.SPOSitePropertiesEnumerable spp = null;

                int total = 0;
                int current = 0;

                // Iterate site collections
                while (spp == null || spp.Count > 0)
                {
                    spp = tenant.GetSiteProperties(current, false);
                    ctx.Load(spp);
                    ctx.ExecuteQuery();

                    total += spp.Count;

                    foreach (SPTenantAdmin.SiteProperties sp in spp)
                    {
                        AddLoadingNode(parentNode, sp.Url, "Represents a site collection within a tenant, including a top-level website and all its subsites.", Constants.IMAGE_SITE, NodeLoadType.Site);

                        // Update progress
                        current++;
                        ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                    }
                }

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // TODO: Add content type hub to collection of sites. This is OOTB not supported to open via Tenant context, build a workaround.
                //// Add Content Type Hub site collection
                //string ctHubUrl = string.Format("{0}/sites/contenttypehub", tenant.RootSiteUrl);
                //AddLoadingNode(parentNode, ctHubUrl, "Represents a collection of sites in a tenant, including a top-level website and all its subsites.", Constants.IMAGE_SITE, LoadType.Site);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }

            return siteNode;
        }

        public static TreeNode LoadSite(TreeNode parentNode, SPTenantAdmin.Tenant tenant, string siteUrl, MainForm form, List<string> subwebs = null)
        {
            TreeNode siteNode = null;

            try
            {
                // Get site collection
                SPClient.Site site = tenant.GetSiteByUrl(siteUrl);

                // Get current tenant
                TenantAuthentication tenantAuthn = Globals.Tenants[tenant.RootSiteUrl];

                TreeNode tenantNode = parentNode.Parent;
                siteNode = NodeLoader.LoadSite(tenantNode, tenantAuthn.ClientContext, site, form, subwebs);
                siteNode.Expand();

                // Replace existing loading node
                parentNode = siteNode;
                tenantNode.Nodes.RemoveAt(tenantNode.Nodes.OfType<TreeNode>().FirstOrDefault(n => n.Text.Contains(site.Url) && n.Tag.ToString().Equals(NodeLoadType.Site.ToString())).Index);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }

            return siteNode;
        }

        public static TreeNode LoadSite(TreeNode parentNode, SPClient.ClientContext ctx, SPClient.Site site, MainForm form, List<string> subwebs = null)
        {
            TreeNode siteNode = null;

            try
            {
                // Load site collection
                ctx.Load(site,
#if CLIENTSDKV161
                    s => s.ShowPeoplePickerSuggestionsForGuestUsers,
                    s => s.StatusBarLink,
                    s => s.StatusBarText,
#endif
                    s => s.Url);

                if (Configuration.Current.LoadAllProperties || ctx.Site.IsCurrentUserAdmin())
                {
                    ctx.Load(site,
                        s => s.Audit,
                        s => s.CanUpgrade,
                        s => s.UpgradeInfo,
                        s => s.Usage);
                }

                ctx.ExecuteQuery();

                // Load site collection node
                siteNode = AddTreeNode(parentNode, site.Url, site, Constants.IMAGE_SITE, "Represents a collection of sites in a Web application, including a top-level website and all its subsites.", form.mnContextSite);
                siteNode.Expand();

                // Load site collection root web node
                TreeNode rootWebNode = LoadWeb(siteNode, site.RootWeb, form);

                // Add specific webs
                if (subwebs != null)
                {
                    foreach (string webUrl in subwebs)
                    {
                        try
                        {
                            SPClient.Web subweb = site.OpenWeb(webUrl);
                            LoadWeb(siteNode, subweb, form);
                        }
                        catch (Exception ex)
                        {
                            // Show and log message 
                            string message = string.Format("Web {0} does not exist.", webUrl);
                            MessageBox.Show(message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            LogUtil.LogException(message, ex);
                        }
                    }
                }

                // Add properties
                AddTreeNode(siteNode, "Audit", site.Audit, Constants.IMAGE_SITE_AUDIT, string.Empty, form.mnContextItem);
                AddTreeNode(siteNode, "Usage", site.IsPropertyAvailable("Usage") ? site.Usage : null, Constants.IMAGE_SITE_USAGE, "Provides fields that are used to access information about site collection usage.", form.mnContextItem);
                AddTreeNode(siteNode, "Upgrade Info", site.IsPropertyAvailable("UpgradeInfo") ? site.UpgradeInfo : null, Constants.IMAGE_SITE_UPGRADE_INFO, "Represents the site upgrade information.", form.mnContextItem);

                // Add loading nodes
                AddLoadingNode(siteNode, "Site Columns", "Gets the collection of field objects that represents all the fields in the site collection.", Constants.IMAGE_SITE_COLUMN, NodeLoadType.SiteFields);
                AddLoadingNode(siteNode, "Content Types", "Gets the collection of content types for the site collection.", Constants.IMAGE_CONTENT_TYPE, NodeLoadType.SiteContentTypes);
                AddLoadingNode(siteNode, "Site Collection Features", "Gets the site collection features for the site collection.", Constants.IMAGE_FEATURE, NodeLoadType.SiteFeatures);
                AddLoadingNode(siteNode, "Site Administrators", "Site Collection Administrators are given full control over all Web sites in the site collection.", Constants.IMAGE_SITE_ADMINS, NodeLoadType.SiteCollectionAdmins);
                AddLoadingNode(siteNode, "Event Receivers", "Provides event receivers for events that occur at the scope of the site collection.", Constants.IMAGE_EVENT_RECEIVER, NodeLoadType.SiteEventReceivers);
                AddLoadingNode(siteNode, "Term Stores", "Represents a collection of TermStore objects.", Constants.IMAGE_TERM_STORE, NodeLoadType.TermStores);
                AddLoadingNode(siteNode, "User Profiles", "Use the PeopleManager object and other objects in the UserProfiles namespace to access user profiles and user properties from custom solutions for SharePoint Server 2013 and apps for SharePoint.", Constants.IMAGE_USER_PROFILE, NodeLoadType.UserProfiles);
                AddLoadingNode(siteNode, "Recycle Bin", "Gets a value that specifies the collection of Recycle Bin items for the site collection.", Constants.IMAGE_RECYCLE_BIN, NodeLoadType.SiteRecycleBin);
                AddLoadingNode(siteNode, "Web Templates", "Returns the collection of site definitions that are available for creating websites within the site collection.", Constants.IMAGE_WEB_TEMPLATES, NodeLoadType.SiteWebTemplates);
                AddLoadingNode(siteNode, "User Custom Actions", "Gets a value that specifies the collection of user custom actions for the site collection.", Constants.IMAGE_USER_CUSTOM_ACTIONS, NodeLoadType.SiteUserCustomActions);

                if (!site.Context.IsMinimalServerVersion(ServerVersion.SharePoint2013))
                {
                    siteNode.ImageKey = Constants.IMAGE_SITE_WARNING;
                    siteNode.SelectedImageKey = Constants.IMAGE_SITE_WARNING;

                    MessageBox.Show(string.Format("You are NOT connecting to a SharePoint 2013 site ({0}), this could result in errors. Please be aware! The application will continue loading the site as normal.", site.RootWeb.GetUrl()), form.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }

            return siteNode;
        }

        public static TreeNode LoadWeb(TreeNode parentNode, SPClient.Web web, MainForm form)
        {
            SPClient.ClientContext ctx = GetClientContext(parentNode);
            ctx.Load(web);

            if (Configuration.Current.LoadAllProperties || ctx.Site.IsCurrentUserAdmin())
            {
                ctx.Load(web,
                    w => w.AllowDesignerForCurrentUser,
                    w => w.AllowMasterPageEditingForCurrentUser,
                    w => w.AllowRevertFromTemplateForCurrentUser,
                    w => w.EffectiveBasePermissions,
                    w => w.HasUniqueRoleAssignments,
                    w => w.RegionalSettings,
                    w => w.RegionalSettings.TimeZone,
                    w => w.SaveSiteAsTemplateEnabled,
                    w => w.SupportedUILanguageIds,
                    w => w.ThemeInfo,
#if CLIENTSDKV160UP
                    w => w.AllowCreateDeclarativeWorkflowForCurrentUser,
                    w => w.AllowSaveDeclarativeWorkflowAsTemplateForCurrentUser,
                    w => w.AllowSavePublishDeclarativeWorkflowForCurrentUser,
                    w => w.ContainsConfidentialInfo,
                    w => w.DataLeakagePreventionStatusInfo,
                    w => w.DesignerDownloadUrlForCurrentUser,
                    w => w.MembersCanShare,
                    w => w.ThirdPartyMdmEnabled,
#endif
#if CLIENTSDKV161
                    w => w.RequestAccessEmail, // TODO: move RequestAccessEmail property to v16, but seems it causing issues with SP2016 at the moment.
                    w => w.AllowAutomaticASPXPageIndexing,
                    w => w.DisableAppViews,
                    w => w.DisableFlows,
                    w => w.ExcludeFromOfflineClient,
                    w => w.NotificationsInOneDriveForBusinessEnabled,
                    w => w.NotificationsInSharePointEnabled,
                    w => w.PreviewFeaturesEnabled,
                    w => w.SiteLogoDescription,
                    w => w.TenantTagPolicyEnabled,
                    w => w.ThemedCssFolderUrl,
#endif
                    w => w.ShowUrlStructureForCurrentUser);
            }

            ctx.ExecuteQuery();

            TreeNode node = AddTreeNode(parentNode, string.Format("{0} ({1})", web.Title, web.ServerRelativeUrl), web, Constants.IMAGE_SITE, "Represents a Microsoft SharePoint Foundation Web site.", form.mnContextItem);
            if (web.IsAppWeb())
            {
                node.ImageKey = Constants.IMAGE_APP_WEB;
                node.SelectedImageKey = Constants.IMAGE_APP_WEB;
                node.ForeColor = Color.DarkGray;
            }

            // Add properties
            AddTreeNode(node, "Theme Info", web.ThemeInfo, Constants.IMAGE_THEME_INFO, "Represents the theming information for a site. This includes information like colors, fonts, border radii sizes, and so on.", form.mnContextItem);
            TreeNode regionalSettingsNode = AddTreeNode(node, "Regional Settings", web.RegionalSettings, Constants.IMAGE_REGIONAL_SETTINGS, "Gets the regional settings that are currently implemented on the website.", form.mnContextItem);


#if CLIENTSDKV160UP
            // Add properties for SDKv16 
            AddTreeNode(node, "Data Leakage Prevention Status Info", web.DataLeakagePreventionStatusInfo, Constants.IMAGE_DATA_LEAKAGE_PREVENTATION_STATUS_INFO, "", form.mnContextItem);
#endif

            // Add loading nodes
            AddLoadingNode(node, "Webs", "Returns the collection of child sites of the current site based on the specified query.", Constants.IMAGE_WEB, NodeLoadType.WebSubWebs);
            AddLoadingNode(node, "Site Columns", "Gets the collection of field objects that represents all the fields in the Web site.", Constants.IMAGE_SITE_COLUMN, NodeLoadType.WebFields);
            AddLoadingNode(node, "Content Types", "Gets the collection of content types for the Web site.", Constants.IMAGE_CONTENT_TYPE, NodeLoadType.WebContentTypes);
            AddLoadingNode(node, "Lists", "Gets the collection of all lists that are contained in the Web site available to the current user based on the current user’s permissions.", Constants.IMAGE_LIST, NodeLoadType.WebLists);
            AddLoadingNode(node, "Site Features", "Gets a value that specifies the collection of features that are currently activated in the site.", Constants.IMAGE_FEATURE, NodeLoadType.WebFeatures);
            LoadNavigation(node, web.Navigation, form);
            AddLoadingNode(node, "App Instances", "Retrieves the app instances installed at the specified runtime context and web object.", Constants.IMAGE_APP_INSTANCES, NodeLoadType.WebAppInstances);
            AddLoadingNode(node, "Site Users", "Gets the UserInfo list of the site collection that contains the Web site.", Constants.IMAGE_SITE_USER, NodeLoadType.WebUsers);
            AddLoadingNode(node, "Associated Groups", "Gets the associated Visitors, Members and Owners groups of the website.", Constants.IMAGE_SITE_GROUP, NodeLoadType.WebAssociatedGroups);
            AddLoadingNode(node, "Site Groups", "Gets the collection of groups for the site collection.", Constants.IMAGE_SITE_GROUP, NodeLoadType.WebGroups);
            AddLoadingNode(node, "Workflow Associations (2010)", "Gets a value that specifies the collection of all workflow associations for the site.", Constants.IMAGE_WORKFLOW_ASSOCIATION, NodeLoadType.WebWorkflowAssociations);
            AddLoadingNode(node, "Workflow Templates (2010)", "Gets a value that specifies the collection of all workflow associations for the site.", Constants.IMAGE_WORKFLOW_TEMPLATE, NodeLoadType.WebWorkflowTemplates);
            AddLoadingNode(node, "Workflow Subscriptions (2013)", "Gets a value that specifies the collection of all workflow subscriptions for the site.", Constants.IMAGE_WORKFLOW_ASSOCIATION, NodeLoadType.WebWorkflowSubscriptions);
            AddLoadingNode(node, "Workflow Definitions (2013)", "Gets a value that specifies the collection of all workflow templates for the site.", Constants.IMAGE_WORKFLOW_TEMPLATE, NodeLoadType.WebWorkflowDefinitions);
            AddLoadingNode(node, "Workflow Instances (2013)", "", Constants.IMAGE_WORKFLOW_INSTANCE, NodeLoadType.WebWorkflowInstances); // TODO: description
            AddLoadingNode(node, "Event Receivers", "Gets the collection of event receiver definitions that are currently available on the website.", Constants.IMAGE_EVENT_RECEIVER, NodeLoadType.WebEventReceivers);
            AddLoadingNode(node, "Properties", "Gets a collection of metadata for the Web site.", Constants.IMAGE_PROPERTY, NodeLoadType.WebProperties);
            AddLoadingNode(node, "List Templates", "Gets a value that specifies the collection of list definitions and list templates available for creating lists on the site.", Constants.IMAGE_LIST_TEMPLATES, NodeLoadType.WebListTemplates);
            AddLoadingNode(node, "Role Assignments", "Gets the role assignments for the securable object.", Constants.IMAGE_ROLE_ASSIGNMENT, NodeLoadType.WebRoleAssignments);
            AddLoadingNode(node, "Role Definitions", "Gets the collection of role definitions for the website.", Constants.IMAGE_ROLE_DEFINITIONS, NodeLoadType.WebRoleDefinitions);
            AddLoadingNode(node, "Push Notification Subscribers", "Specifies the collection of push notification subscribers for the site, and cannot be NULL.", Constants.IMAGE_PUSH_NOTIFICATION_SUBSCRIBER, NodeLoadType.WebPushNotificationSubscribers);
            AddLoadingNode(node, "Recycle Bin", "Gets the recycle bin of the website.", Constants.IMAGE_RECYCLE_BIN, NodeLoadType.WebRecycleBin);
            AddLoadingNode(node, "Web Templates", "Returns a collection of site templates available for the site.", Constants.IMAGE_WEB_TEMPLATES, NodeLoadType.WebTemplates);
            AddLoadingNode(node, "User Custom Actions", "Gets a value that specifies the collection of user custom actions for the site.", Constants.IMAGE_USER_CUSTOM_ACTIONS, NodeLoadType.WebUserCustomActions);
            AddLoadingNode(node, "Project Policy", "A policy associated with a web site or email mailbox.", Constants.IMAGE_PROJECT_POLICY, NodeLoadType.ProjectPolicy);

            // Add (loading) nodes to Regional Settings
            AddTreeNode(regionalSettingsNode, "Current Time Zone", web.RegionalSettings.TimeZone, Constants.IMAGE_REGIONAL_SETTINGS, "Gets and sets the time zone that is used on the current web.", form.mnContextItem);
            AddLoadingNode(regionalSettingsNode, "Time Zones", "Gets the collection of time zones used in a server farm.", Constants.IMAGE_TIME_ZONES, NodeLoadType.TimeZones);

#if CLIENTSDKV160UP
            // Add loading nodes for SDKv16
            AddLoadingNode(node, "App Tiles", string.Empty, Constants.IMAGE_APP_TILE, NodeLoadType.AppTiles);
#endif
#if CLIENTSDKV161UP
            AddLoadingNode(node, "Alerts", string.Empty, Constants.IMAGE_ALERT, NodeLoadType.WebAlerts);
#endif

            // Add child collections
            LoadFolder(node, web.RootFolder, form, true);

            return node;
        }

        public static void LoadSubWebs(TreeNode parentNode, SPClient.WebCollection webs, MainForm form)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(webs);
                ctx.ExecuteQuery();

                int total = webs.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add webs to parent node
                foreach (SPClient.Web subweb in webs)
                {
                    LoadWeb(parentNode, subweb, form);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(string.Format("Error while loading subwebs for {0} ({1})", parentNode.Text, parentNode.FullPath), ex);

                AddLoadingNode(parentNode, NodeLoadType.WebSubWebs);
            }
        }

        public static void LoadFeatures(TreeNode parentNode, SPClient.FeatureCollection features, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
#if CLIENTSDKV150
                ctx.Load(features);
#else
                ctx.Load(features,
                    items => items.Include(
                        item => item.DefinitionId,
                        item => item.DisplayName));
#endif
                ctx.ExecuteQuery();

                int total = features.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add features to parent node
                foreach (SPClient.Feature feature in features)
                {
                    TreeNode node = AddTreeNode(parentNode, feature.GetFeatureName(), feature, Constants.IMAGE_FEATURE, string.Empty, form.mnContextItem);

                    if (feature.IsCustom())
                    {
                        node.ImageKey = Constants.IMAGE_FEATURE_CUSTOM;
                        node.SelectedImageKey = Constants.IMAGE_FEATURE_CUSTOM;
                        node.ToolTipText = "Custom user defined feature definition.";
                    }
                    else
                    {
                        node.ImageKey = Constants.IMAGE_FEATURE;
                        node.SelectedImageKey = Constants.IMAGE_FEATURE;
                        node.ToolTipText = "Feature definition.";
                    }

                    if (feature.IsHidden())
                    {
                        node.ForeColor = Color.Gray;
                    }

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadSiteColumns(TreeNode parentNode, SPClient.FieldCollection fields, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                IEnumerable<Field> result = ctx.LoadQuery(fields.IncludeWithDefaultProperties(
#if CLIENTSDKV161UP
                    f => f.NoCrawl,
#endif
                    f => f.SchemaXmlWithResourceTokens));
                ctx.ExecuteQuery();

                int total = result.Count();
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add fields to parent node
                foreach (SPClient.Field field in result)
                {
                    TreeNode node = AddTreeNode(parentNode, string.Format("{0} ({1})", field.Title, field.Group), field, Constants.IMAGE_SITE_COLUMN, "Represents a field in a list on a Microsoft SharePoint Foundation Web site.", form.mnContextItem);
                    if (field.Hidden)
                        node.ForeColor = Color.Gray;

                    // Add group node
                    TreeNode groupNode = parentNode.Nodes.OfType<TreeNode>().SingleOrDefault(n => n.Text.Equals(field.Group));
                    if (groupNode == null)
                    {
                        groupNode = new TreeNode(field.Group);
                        groupNode.ToolTipText = "Field Group";
                        groupNode.ImageKey = Constants.IMAGE_SITE_COLUMN_GROUP;
                        groupNode.SelectedImageKey = Constants.IMAGE_SITE_COLUMN_GROUP;
                        groupNode.Tag = NodeLoadType.GeneralGroup;

                        parentNode.Nodes.Add(groupNode);
                    }
                    groupNode.Nodes.Add((TreeNode)node.Clone());

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadFieldLinks(TreeNode parentNode, SPClient.FieldLinkCollection fieldLinks, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(fieldLinks);
                ctx.ExecuteQuery();

                int total = fieldLinks.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add fields to parent node
                foreach (SPClient.FieldLink fieldLink in fieldLinks)
                {
                    TreeNode node = AddTreeNode(parentNode, fieldLink.Name, fieldLink, Constants.IMAGE_FIELD_LINK, "Specifies a reference to a field or field definition for a content type.", form.mnContextItem);
                    if (fieldLink.Hidden)
                        node.ForeColor = Color.Gray;

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadContentTypes(TreeNode parentNode, SPClient.ContentTypeCollection contentTypes, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                IEnumerable<ContentType> result = ctx.LoadQuery(contentTypes.IncludeWithDefaultProperties(
                    c => c.SchemaXmlWithResourceTokens));
                ctx.ExecuteQuery();

                int total = result.Count();
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add content types to parent node
                foreach (SPClient.ContentType ct in result)
                {
                    TreeNode node = AddTreeNode(parentNode, string.Format("{0} ({1})", ct.Name, ct.Group), ct, Constants.IMAGE_CONTENT_TYPE, "Represents a SPContentType object.", form.mnContextItem);
                    if (ct.Hidden)
                        node.ForeColor = Color.Gray;

                    // Add loading nodes
                    AddLoadingNode(node, "Fields", "Gets a value that specifies the collection of fields for the content type.", Constants.IMAGE_SITE_COLUMN, NodeLoadType.ContentTypeFields);
                    AddLoadingNode(node, "Field Links", "Gets the column (also known as field) references in the content type.", Constants.IMAGE_FIELD_LINK, NodeLoadType.FieldLinks);
                    AddLoadingNode(node, "Workflow Associations (2010)", "Gets a value that specifies the collection of workflow associations for the content type.", Constants.IMAGE_WORKFLOW_ASSOCIATION, NodeLoadType.ContentTypeWorkflowAssociations);

                    // Add group node
                    TreeNode groupNode = parentNode.Nodes.OfType<TreeNode>().SingleOrDefault(n => n.Text.Equals(ct.Group));
                    if (groupNode == null)
                    {
                        groupNode = new TreeNode(ct.Group);
                        groupNode.ToolTipText = "Content Type Group";
                        groupNode.ImageKey = Constants.IMAGE_CONTENT_TYPE_GROUP;
                        groupNode.SelectedImageKey = Constants.IMAGE_CONTENT_TYPE_GROUP;
                        groupNode.Tag = NodeLoadType.GeneralGroup;

                        parentNode.Nodes.Add(groupNode);
                    }
                    groupNode.Nodes.Add((TreeNode)node.Clone());

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadLists(TreeNode parentNode, SPClient.ListCollection lists, MainForm form)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                IEnumerable<List> result;

                if (Configuration.Current.LoadAllProperties || ctx.Site.IsCurrentUserAdmin())
                {
                    result = ctx.LoadQuery(lists.IncludeWithDefaultProperties(
                        list => list.BrowserFileHandling,
                        list => list.DataSource,
                        list => list.DefaultViewUrl,
                        list => list.EffectiveBasePermissions,
                        list => list.EffectiveBasePermissionsForUI,
                        list => list.HasUniqueRoleAssignments,
                        list => list.DefaultDisplayFormUrl,
                        list => list.DefaultEditFormUrl,
                        list => list.DefaultNewFormUrl,
                        list => list.IsSiteAssetsLibrary,
                        list => list.OnQuickLaunch,
                        list => list.ValidationMessage,
                        list => list.ValidationFormula,
#if CLIENTSDKV161UP
                        list => list.AllowDeletion,
                        list => list.DefaultViewPath,
                        list => list.EnableAssignToEmail,
                        list => list.ExcludeFromOfflineClient,
                        list => list.IsSystemList,
                        list => list.PageRenderType,
                        list => list.ReadSecurity,
                        list => list.WriteSecurity,
#endif
                        list => list.SchemaXml));
                }
                else
                {
                    result = ctx.LoadQuery(lists.IncludeWithDefaultProperties(
                        list => list.BrowserFileHandling,
                        list => list.DataSource,
                        list => list.DefaultViewUrl,
                        list => list.EffectiveBasePermissions,
                        list => list.EffectiveBasePermissionsForUI,
                        list => list.HasUniqueRoleAssignments,
                        list => list.SchemaXml));
                }

                ctx.ExecuteQuery();

                int total = result.Count();
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add lists to parent node
                foreach (SPClient.List list in result)
                {
                    TreeNode node = AddTreeNode(parentNode, string.Format("{0} ({1})", list.Title, list.ItemCount), list, list.GetImageUrlFileName(), "Represents a list on a SharePoint Web site.", form.mnContextItem);
                    if (list.Hidden)
                        node.ForeColor = Color.Gray;

                    // Add properties
                    if (list.BaseTemplate != Constants.PICTURE_LIBRARY_BASE_ID)
                    {
                        // Workaround: InformationRightsManagementSettings, "Picture Library does not support IRM" exception thrown
                        ctx.Load(list.InformationRightsManagementSettings);
                        ctx.ExecuteQuery();

                        AddTreeNode(node, "Information Rights Management Settings", list.InformationRightsManagementSettings, Constants.IMAGE_INFORMATION_RIGHTS_MANAGEMENT_SETTINGS, string.Empty, form.mnContextItem);
                    }

                    // Loading nodes
                    AddLoadingNode(node, "Fields", "Gets a value that specifies the collection of all fields in the list.", Constants.IMAGE_SITE_COLUMN, NodeLoadType.ListFields);
                    AddLoadingNode(node, "Content Types", "Gets the content types that are associated with the list.", Constants.IMAGE_CONTENT_TYPE, NodeLoadType.ListContentTypes);
                    AddLoadingNode(node, "Items", "Returns a collection of items from the list based on the specified query.", Constants.IMAGE_ITEM, NodeLoadType.ListItems);
                    AddLoadingNode(node, "Views", "Gets a value that specifies the collection of all public views on the list and personal views of the current user on the list.", Constants.IMAGE_VIEW, NodeLoadType.ListViews);
                    AddLoadingNode(node, "Role Assignments", "Gets the role assignments for the securable object.", Constants.IMAGE_ROLE_ASSIGNMENT, NodeLoadType.ListRoleAssignments);
                    AddLoadingNode(node, "Workflow Associations (2010)", "Gets a value that specifies the collection of all workflow associations for the list.", Constants.IMAGE_WORKFLOW_ASSOCIATION, NodeLoadType.ListWorkflowAssociations);
                    AddLoadingNode(node, "Workflow Subscriptions (2013)", "Gets a value that specifies the collection of all workflow subscriptions for the list.", Constants.IMAGE_WORKFLOW_ASSOCIATION, NodeLoadType.ListWorkflowSubscriptions);
                    AddLoadingNode(node, "Event Receivers", Constants.IMAGE_EVENT_RECEIVER, NodeLoadType.ListEventReceivers);
                    AddLoadingNode(node, "User Custom Actions", "Gets a value that specifies the collection of all user custom actions for the list.", Constants.IMAGE_USER_CUSTOM_ACTIONS, NodeLoadType.ListUserCustomActions);

                    // Add root folder
                    LoadFolder(node, list.RootFolder, form, true);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, NodeLoadType.WebLists);
            }
        }

        public static void LoadSiteGroups(TreeNode parentNode, SPClient.GroupCollection groups, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                IEnumerable<Group> results = ctx.LoadQuery(groups.IncludeWithDefaultProperties(
                    g => g.CanCurrentUserEditMembership,
                    g => g.CanCurrentUserManageGroup,
                    g => g.CanCurrentUserViewMembership));
                ctx.ExecuteQuery();

                int total = results.Count();
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add groups to parent node
                foreach (SPClient.Group group in results)
                {
                    AddSiteGroupNode(parentNode, form, group);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadAssociatedGroups(TreeNode parentNode, SPClient.Web web, MainForm form)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(web,
                    w => w.AssociatedVisitorGroup,
                    w => w.AssociatedMemberGroup,
                    w => w.AssociatedOwnerGroup);
                ctx.ExecuteQuery();

                AddSiteGroupNode(parentNode, form, web.AssociatedVisitorGroup, "Associated Visitors");
                AddSiteGroupNode(parentNode, form, web.AssociatedMemberGroup, "Associated Members");
                AddSiteGroupNode(parentNode, form, web.AssociatedOwnerGroup, "Associated Owners");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, NodeLoadType.WebAssociatedGroups);
            }
        }

        private static void AddSiteGroupNode(TreeNode parentNode, MainForm form, SPClient.Group group, string prefix = "", bool addLoadingNodeUsers = true)
        {
            string groupName = string.Empty;

            if (group == null || !group.IsPropertyAvailable("Title"))
                return;

            // Set group name
            if (string.IsNullOrEmpty(prefix))
                groupName = group.Title;
            else
                groupName = string.Format("{0}: {1}", prefix, group.Title);

            TreeNode node = AddTreeNode(parentNode, groupName, group, Constants.IMAGE_SITE_GROUP, "Represents a group on a Microsoft SharePoint Foundation Web site.", form.mnContextItem);

            // Add loading node for users in group
            if (addLoadingNodeUsers)
                AddLoadingNode(node, NodeLoadType.GroupUsers);
        }

        public static void LoadUsers(TreeNode parentNode, SPClient.UserCollection users, MainForm form, NodeLoadType loadType, bool addLoadingNodeUserGroups = true)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(users);
                ctx.ExecuteQuery();

                int total = users.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add users to parent node
                foreach (SPClient.User user in users)
                {
                    TreeNode node = AddTreeNode(parentNode, user.Title, user, user.GetTreeNodeIcon(), "", form.mnContextItem);

                    if (user.IsHiddenInUI)
                        node.ForeColor = Color.Gray;

                    // Add loading nodes
                    if (addLoadingNodeUserGroups)
                        AddLoadingNode(node, "Groups", "Gets the collection of groups of which the user is a member.", Constants.IMAGE_SITE_GROUP, NodeLoadType.GroupsForUser);

                    AddLoadingNode(node, "Alerts", "Gets the collection of alerts for this user.", Constants.IMAGE_ALERT, NodeLoadType.UserAlerts);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadSiteCollectionAdmins(TreeNode parentNode, Site site, MainForm form, NodeLoadType loadType)
        {
            try
            {
                UserCollection users = site.RootWeb.SiteUsers;

                ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(users);
                ctx.ExecuteQuery();

                int total = users.Count;
                int current = 0;
                int adminCount = 0;

                // Add users to parent node
                foreach (User user in users)
                {
                    if (user.IsSiteAdmin)
                    {
                        TreeNode node = AddTreeNode(parentNode, user.Title, user, user.GetTreeNodeIcon(), "", form.mnContextItem);

                        if (user.IsHiddenInUI)
                            node.ForeColor = Color.Gray;

                        adminCount++;
                    }

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }

                // Add count to parent node
                UpdateCountChildNodes(parentNode, adminCount);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadWorkflowAssociations(TreeNode parentNode, SPClient.Workflow.WorkflowAssociationCollection workflows, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(workflows);
                ctx.ExecuteQuery();

                int total = workflows.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add workflow associations to parent node
                foreach (SPClient.Workflow.WorkflowAssociation workflow in workflows)
                {
                    TreeNode node = AddTreeNode(parentNode, workflow.Name, workflow, Constants.IMAGE_WORKFLOW_ASSOCIATION, "Represents the association of a workflow template with a specific list or content type, and that contains members that return custom information about that workflow's association with the specific list or content type.", form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadWorkflowTemplates(TreeNode parentNode, SPClient.Workflow.WorkflowTemplateCollection workflows, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(workflows);
                ctx.ExecuteQuery();

                int total = workflows.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add workflow templates to parent node
                foreach (var workflow in workflows)
                {
                    TreeNode node = AddTreeNode(parentNode, workflow.Name, workflow, Constants.IMAGE_WORKFLOW_TEMPLATE, "Represents a workflow template currently deployed on the SharePoint site, and contains members that you can use to get or set information about the template, such as default instantiation data for creating a workflow association from the template.", form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadWorkflowDefinitions(TreeNode parentNode, SPClient.Web web, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(web);
                ctx.ExecuteQuery();

                SPClient.WorkflowServices.WorkflowServicesManager wsm = new SPClient.WorkflowServices.WorkflowServicesManager(ctx, web);
                SPClient.WorkflowServices.WorkflowDeploymentService wds = wsm.GetWorkflowDeploymentService();

                SPClient.WorkflowServices.WorkflowDefinitionCollection wfDefinitions = wds.EnumerateDefinitions(false);
                ctx.Load(wfDefinitions);
                ctx.ExecuteQuery();

                int total = wfDefinitions.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add workflow definitions to parent node
                foreach (SPClient.WorkflowServices.WorkflowDefinition wfDefintion in wfDefinitions)
                {
                    TreeNode node = AddTreeNode(parentNode, wfDefintion.DisplayName, wfDefintion, SPBrowser.Properties.Resources.IMAGE_WORKFLOW_TEMPLATE, "Represents a workflow definition currently deployed on the SharePoint site, and contains members that you can use to get or set information about the definition, such as default instantiation data for creating a workflow subscription from the template.", form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadWorkflowSubscriptions(TreeNode parentNode, SPClient.Web web, MainForm form, NodeLoadType loadType, SPClient.List list = null)
        {
            try
            {
                string description = string.Empty;
                SPClient.ClientContext ctx = GetClientContext(parentNode);

                SPClient.WorkflowServices.WorkflowServicesManager wsm = new SPClient.WorkflowServices.WorkflowServicesManager(ctx, web);
                SPClient.WorkflowServices.WorkflowSubscriptionService wss = wsm.GetWorkflowSubscriptionService();
                SPClient.WorkflowServices.WorkflowSubscriptionCollection wfSubscriptions = null;

                switch (loadType)
                {
                    // TODO: List, CT, List&CT
                    // WorkflowSubscriptionCollection webSubscriptions = wfSubSvc.EnumerateSubscriptionsByEventSource(web.Id);
                    case NodeLoadType.WebWorkflowSubscriptions:
                        description = "Represents a workflow subscription currently deployed on the SharePoint site, and contains members that you can use to get or set information about the definition, such as default instantiation data for creating a workflow subscription from the template.";
                        wfSubscriptions = wss.EnumerateSubscriptions();
                        break;
                    case NodeLoadType.ListWorkflowSubscriptions:
                        description = "Represents a workflow subscription currently deployed on the SharePoint list, and contains members that you can use to get or set information about the definition, such as default instantiation data for creating a workflow subscription from the template.";
                        wfSubscriptions = wss.EnumerateSubscriptionsByList(list.Id);
                        break;
                    default:
                        break;
                }

                ctx.Load(wfSubscriptions);
                ctx.ExecuteQuery();

                int total = wfSubscriptions.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add workflow subscriptions to parent node
                foreach (SPClient.WorkflowServices.WorkflowSubscription wfSubscription in wfSubscriptions)
                {
                    TreeNode node = AddTreeNode(parentNode, wfSubscription.Name, wfSubscription, Constants.IMAGE_WORKFLOW_TEMPLATE, description, form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadWorkflowInstances(TreeNode parentNode, SPClient.Web web, MainForm form, NodeLoadType loadType, SPClient.ListItem item = null)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);

                SPClient.WorkflowServices.WorkflowServicesManager wsm = new SPClient.WorkflowServices.WorkflowServicesManager(ctx, web);
                SPClient.WorkflowServices.WorkflowInstanceService wis = wsm.GetWorkflowInstanceService();
                SPClient.WorkflowServices.WorkflowInstanceCollection wfInstances = null;

                if (loadType == NodeLoadType.WebWorkflowInstances)
                {
                    wfInstances = wis.EnumerateInstancesForSite();
                }
                if (loadType == NodeLoadType.ItemWorkflowInstances)
                {
                    ctx.Load(item.ParentList);
                    ctx.ExecuteQuery();

                    wfInstances = wis.EnumerateInstancesForListItem(item.ParentList.Id, item.Id);
                }

                ctx.Load(wfInstances);
                ctx.ExecuteQuery();

                int total = wfInstances.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add workflow instances to parent node
                foreach (SPClient.WorkflowServices.WorkflowInstance wfInstance in wfInstances)
                {
                    TreeNode node = AddTreeNode(parentNode, string.Format("{0} ({1})", wfInstance.Id, wfInstance.Status), wfInstance, Constants.IMAGE_WORKFLOW_INSTANCE, "Represents a workflow instance.", form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadViews(TreeNode parentNode, SPClient.ViewCollection views, MainForm form)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                IEnumerable<SPClient.View> results = ctx.LoadQuery(views.IncludeWithDefaultProperties(
#if CLIENTSDKV160UP
                    //v => v.VisualizationInfo));
                    ));
#else
                    ));
#endif
                ctx.ExecuteQuery();

                int total = results.Count();
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add views to parent node
                foreach (SPClient.View view in results)
                {
                    TreeNode node = AddTreeNode(parentNode, view.Title, view, Constants.IMAGE_VIEW, "Specifies a list view.", form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, NodeLoadType.ListViews);
            }
        }

        public static void LoadItems(TreeNode parentNode, SPClient.List list, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);

                // Get list data
                SPClient.CamlQuery query = new SPClient.CamlQuery();
                query.ViewXml = "<View Scope='RecursiveAll'><RowLimit>5000</RowLimit></View>";

                SPClient.ListItemCollection items = list.GetItems(query);
                IEnumerable<ListItem> result = ctx.LoadQuery(items.IncludeWithDefaultProperties(
                    item => item.Client_Title,
                    item => item.ContentType,
                    item => item.DisplayName,
                    item => item.EffectiveBasePermissions,
                    item => item.EffectiveBasePermissionsForUI,
                    item => item.HasUniqueRoleAssignments,
                    item => item.File,
#if CLIENTSDKV161UP
                    //item => item.IconOverlay, // TODO: IconOverlay is causing exceptions when loading items "invalid field name b77cdbcf-5dce-4937-85a7-9fc202705c91"
                    item => item.ComplianceInfo,
#endif
                    item => item.Folder));
                ctx.ExecuteQuery();

                int total = result.Count();
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add items to parent node
                foreach (SPClient.ListItem item in result)
                {
                    TreeNode node = AddTreeNode(parentNode, string.Format("{0}, {1}", item.Id, item.DisplayName), item, item.IsRecord(ctx) ? Constants.IMAGE_ITEM_RECORD : Constants.IMAGE_ITEM, "Represents an item or row in a list.", form.mnContextItem);

                    if (item.IsPropertyAvailable("ContentType"))
                        AddTreeNode(node, item.ContentType.Name, item.ContentType, Constants.IMAGE_CONTENT_TYPE, "Gets a value that specifies the content type of the list item.", form.mnContextItem);
                    if (item.IsPropertyAvailable("File"))
                        AddTreeNode(node, item.File.Name, item.File, Constants.IMAGE_FILE, "Gets the file that is represented by the item from a document library.", form.mnContextItem);
                    if (item.IsPropertyAvailable("Folder"))
                        AddTreeNode(node, item.Folder.Name, item.Folder, Constants.IMAGE_FOLDER, "Gets a folder object that is associated with a folder item.", form.mnContextItem);
#if CLIENTSDKV161UP
                    if (item.IsPropertyAvailable("ComplianceInfo"))
                        AddTreeNode(node, "Compliance Info", item.ComplianceInfo, Constants.IMAGE_COMPLIANCE_INFO, string.Empty, form.mnContextItem);
#endif

                    AddLoadingNode(node, "Attachment Files", "Specifies the collection of attachments that are associated with the list item.", Constants.IMAGE_FILE, NodeLoadType.ItemAttachmentFiles);
                    AddLoadingNode(node, "Field Values", string.Empty, Constants.IMAGE_ITEM_VALUE, NodeLoadType.ItemFieldValues);
                    AddLoadingNode(node, "Field Values As Html", "Gets the values for the list item as HTML.", Constants.IMAGE_ITEM_VALUE, NodeLoadType.ItemFieldValuesAsHtml);
                    AddLoadingNode(node, "Field Values As Text", "Gets the list item’s field values as a collection of string values.", Constants.IMAGE_ITEM_VALUE, NodeLoadType.ItemFieldValuesAsText);
                    AddLoadingNode(node, "Field Values For Edit", "Gets the formatted values to be displayed in an edit form.", Constants.IMAGE_ITEM_VALUE, NodeLoadType.ItemFieldValuesForEdit);
                    AddLoadingNode(node, "Role Assignments", "Gets the role assignments for the securable object.", Constants.IMAGE_ROLE_ASSIGNMENT, NodeLoadType.ItemRoleAssignments);
                    AddLoadingNode(node, "Workflow Instances", string.Empty, Constants.IMAGE_WORKFLOW_INSTANCE, NodeLoadType.ItemWorkflowInstances); // TODO: description
#if CLIENTSDKV161UP
                    AddLoadingNode(node, "Properties", string.Empty, Constants.IMAGE_PROPERTY, NodeLoadType.ItemProperties); // TODO: description
#endif

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadAttachmentCollection(TreeNode parentNode, SPClient.AttachmentCollection attachments, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(attachments);
                ctx.ExecuteQuery();

                int total = attachments.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                foreach (SPClient.Attachment attachment in attachments)
                {
                    TreeNode node = AddTreeNode(parentNode, attachment.FileName, attachment, Constants.IMAGE_FILE, string.Empty, form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadDictionaryValues(TreeNode parentNode, Dictionary<string, object> values, MainForm form, NodeLoadType loadType)
        {
            try
            {
                int total = values.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                foreach (var value in values)
                {
                    TreeNode node = AddTreeNode(parentNode, value.Key, value, Constants.IMAGE_ITEM_VALUE, string.Empty, form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadFieldStringValues(TreeNode parentNode, SPClient.FieldStringValues values, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(values);
                ctx.ExecuteQuery();

                int total = values.FieldValues.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                foreach (var fieldValue in values.FieldValues)
                {
                    TreeNode node = AddTreeNode(parentNode, fieldValue.Key, fieldValue, Constants.IMAGE_ITEM_VALUE, string.Empty, form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadNavigation(TreeNode parentNode, SPClient.Navigation navigation, MainForm form)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(navigation);
                ctx.ExecuteQuery();

                // Add Navigation node
                TreeNode node = AddTreeNode(parentNode, "Navigation", navigation, Constants.IMAGE_NAVIGATION, "Gets a value that specifies the navigation structure on the site, including the Quick Launch area and the top navigation bar.", form.mnContextItem);

                AddLoadingNode(node, "Top Navigation Bar", "Gets a value that collects navigation nodes corresponding to links in the top navigation bar of the site.", Constants.IMAGE_NAVIGATION, NodeLoadType.NavigationTopNavigationBar);
                AddLoadingNode(node, "Quick Launch", "Gets a value that collects navigation nodes corresponding to links in the Quick Launch area of the site.", Constants.IMAGE_NAVIGATION, NodeLoadType.NavigationQuickLaunch);
            }
            catch (Exception ex)
            {
                LogUtil.LogException(ex);
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void LoadNavigationNodes(TreeNode parentNode, SPClient.NavigationNodeCollection navigationNodes, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(navigationNodes);
                ctx.ExecuteQuery();

                int total = navigationNodes.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                foreach (SPClient.NavigationNode navNode in navigationNodes)
                {
                    TreeNode node = AddTreeNode(parentNode, navNode.Title, navNode, Constants.IMAGE_NAVIGATION, "Represents the URL to a specific navigation node and provides access to properties and methods for manipulating the ordering of the navigation node in a navigation node collection.", form.mnContextItem);

                    AddLoadingNode(node, NodeLoadType.NavigationNodes);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadFolder(TreeNode parentNode, SPClient.Folder folder, MainForm form, bool isRootFolder = false)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(folder);
                ctx.ExecuteQuery();

                // Add folder node
                TreeNode node = AddTreeNode(parentNode, isRootFolder ? "Root Folder" : folder.Name, folder, Constants.IMAGE_FOLDER, string.Empty, form.mnContextCollection);

                // Add loading nodes
                AddLoadingNode(node, NodeLoadType.Folder);
            }
            catch (ServerUnauthorizedAccessException ex)
            {
                if (isRootFolder)
                {
                    // Error while loading folder
                    TreeNode node = AddTreeNode(parentNode, "Root Folder", null, Constants.IMAGE_FOLDER_ERROR, ex.Message);
                }
                else
                    AddLoadingNode(parentNode, NodeLoadType.Folder);

                LogUtil.LogException(ex);
            }
            catch (Exception ex)
            {
                AddLoadingNode(parentNode, NodeLoadType.Folder);

                LogUtil.LogException(ex);
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void LoadSubFolders(TreeNode parentNode, SPClient.Folder folder, MainForm form)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(folder.Folders);
#if CLIENTSDKV160UP
                ctx.Load(folder, f => f.StorageMetrics);
#endif
                IEnumerable<File> fileResults = ctx.LoadQuery(folder.Files.IncludeWithDefaultProperties(
#if CLIENTSDKV161UP
                    f => f.ListId,
                    f => f.SiteId,
                    f => f.WebId));
#else
                    ));
#endif
                ctx.ExecuteQuery();

                int total = folder.Folders.Count + fileResults.Count();
                int current = 0;

#if CLIENTSDKV160UP
                // Add Folder properties
                AddTreeNode(parentNode, "Storage Metrics", folder.StorageMetrics, Constants.IMAGE_STORAGE_METRICS, string.Empty, form.mnContextItem);
#endif
                // Add loading nodes for Folder properties
                AddLoadingNode(parentNode, "Properties", Constants.IMAGE_PROPERTY, NodeLoadType.FolderProperties);

                // Add folders
                foreach (SPClient.Folder subFolder in folder.Folders)
                {
                    LoadFolder(parentNode, subFolder, form);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }

                // Add files
                foreach (SPClient.File file in fileResults)
                {
                    // Add file node
                    TreeNode node = AddTreeNode(parentNode, file.Name, file, Constants.IMAGE_FILE, "", form.mnContextItem);

                    // Add file loading node
                    if (file.IsAspxPage()) AddLoadingNode(node, "Web Parts", Constants.IMAGE_WEBPART, NodeLoadType.WebParts);
                    AddLoadingNode(node, "File Versions", Constants.IMAGE_FILE_VERSIONS, NodeLoadType.FileVersions);
#if CLIENTSDKV161UP
                    AddLoadingNode(node, "Version Events", Constants.IMAGE_FILE_VERSIONS_EVENT, NodeLoadType.FileVersionEvents);
#endif
#if CLIENTSDKV160UP
                    AddLoadingNode(node, "Properties", string.Empty, Constants.IMAGE_PROPERTY, NodeLoadType.FileProperties); // TODO: description
#endif

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                AddLoadingNode(parentNode, NodeLoadType.Folder);

                LogUtil.LogException(ex);
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void LoadWebParts(TreeNode parentNode, SPClient.File file, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.WebParts.LimitedWebPartManager limitedWebPartManager = file.GetLimitedWebPartManager(SPClient.WebParts.PersonalizationScope.Shared);
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(limitedWebPartManager.WebParts,
                    wps => wps.Include(
                        wp => wp.WebPart.Title,
                        wp => wp.WebPart.Subtitle,
                        wp => wp.WebPart.IsClosed,
                        wp => wp.WebPart.ZoneIndex,
                        wp => wp.WebPart.TitleUrl,
                        wp => wp.WebPart.Hidden
                    ));
                ctx.ExecuteQuery();

                int total = limitedWebPartManager.WebParts.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                foreach (SPClient.WebParts.WebPartDefinition webPartDefinition in limitedWebPartManager.WebParts)
                {
                    TreeNode node = AddTreeNode(parentNode, webPartDefinition.WebPart.Title, webPartDefinition.WebPart, Constants.IMAGE_WEBPART, "Represents a Web Part on a Web Part Page.", form.mnContextItem);
                    if (webPartDefinition.WebPart.Hidden)
                        node.ForeColor = Color.Gray;

                    // Add folder properties
                    AddLoadingNode(node, "Properties", Constants.IMAGE_PROPERTY, NodeLoadType.WebPartProperties);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                LogUtil.LogException(ex);
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadFileVersions(TreeNode parentNode, SPClient.FileVersionCollection versions, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(versions);
                ctx.ExecuteQuery();

                int total = versions.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                foreach (SPClient.FileVersion version in versions)
                {
                    TreeNode node = AddTreeNode(parentNode, version.VersionLabel, version, Constants.IMAGE_FILE_VERSIONS, "Represents a version of a SPFile object.", form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                LogUtil.LogException(ex);
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadEventReceivers(TreeNode parentNode, SPClient.EventReceiverDefinitionCollection eventReceivers, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(eventReceivers);
                ctx.ExecuteQuery();

                int total = eventReceivers.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add event receivers to parent node
                foreach (SPClient.EventReceiverDefinition eventReceiver in eventReceivers)
                {
                    TreeNode node = AddTreeNode(parentNode, string.Format("{0} ({1})", eventReceiver.ReceiverName, eventReceiver.EventType), eventReceiver, Constants.IMAGE_EVENT_RECEIVER, "Abstract base class that defines general properties of an event receiver for list items, lists, websites, and workflows.", form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadProperties(TreeNode parentNode, SPClient.PropertyValues properties, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(properties);
                ctx.ExecuteQuery();

                int total = properties.FieldValues.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add properties to parent node
                foreach (var property in properties.FieldValues)
                {
                    TreeNode node = AddTreeNode(parentNode, string.Format("{0}", property.Key), property, Constants.IMAGE_PROPERTY, string.Empty, form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadRecycleBin(TreeNode parentNode, SPClient.RecycleBinItemCollection recycleBinItems, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(recycleBinItems);
                ctx.ExecuteQuery();

                int total = recycleBinItems.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add recycle bin items to parent node
                foreach (var recycleBinItem in recycleBinItems)
                {
                    TreeNode node = AddTreeNode(parentNode, string.Format("{0}", recycleBinItem.Title), recycleBinItem, Constants.IMAGE_RECYCLE_BIN, "Represents a Recycle Bin item in the Recycle Bin of a site or a site collection.", form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadListTemplates(TreeNode parentNode, SPClient.ListTemplateCollection listTemplates, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(listTemplates);
                ctx.ExecuteQuery();

                int total = listTemplates.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add templates to parent node
                foreach (var template in listTemplates)
                {
                    TreeNode node = AddTreeNode(parentNode, string.Format("{0} ({1})", template.Name, template.InternalName), template, Constants.IMAGE_LIST_TEMPLATES, "Specifies a list template.", form.mnContextItem);

                    if (template.Hidden)
                    {
                        node.ForeColor = Color.Gray;
                    }

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadUserCustomActions(TreeNode parentNode, SPClient.UserCustomActionCollection customActions, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                IEnumerable<UserCustomAction> results = ctx.LoadQuery(customActions.IncludeWithDefaultProperties(
#if CLIENTSDKV161UP
                    ca => ca.ClientSideComponentId,
                    ca => ca.ClientSideComponentProperties,
#endif
                    ca => ca.Description));

                ctx.ExecuteQuery();

                int total = results.Count();
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add custom actions to parent node
                foreach (var customAction in results)
                {
                    TreeNode node = AddTreeNode(parentNode, string.Format("{0} ({1})", customAction.Title, customAction.Name), customAction, Constants.IMAGE_USER_CUSTOM_ACTIONS, "Represents a custom action associated with a SharePoint list, Web site, or subsite.", form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadAppInstances(TreeNode parentNode, SPClient.Web web, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ClientObjectList<AppInstance> apps = AppCatalog.GetAppInstances(ctx, web);
                ctx.Load(apps);
                ctx.ExecuteQuery();

                int total = apps.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add apps to parent node
                foreach (var app in apps)
                {
                    TreeNode node = AddTreeNode(parentNode, string.Format("{0} ({1})", app.Title, app.Status), app, Constants.IMAGE_APP_INSTANCES, "Represents an App object installed to a specific Web site.", form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        /// <summary>
        /// Gets the collection of push notification subscribers for the site, and cannot be NULL.
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="notifications"></param>
        /// <param name="form"></param>
        /// <param name="loadType"></param>
        public static void LoadPushNotificationSubscribers(TreeNode parentNode, SPClient.PushNotificationSubscriberCollection notifications, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(notifications);
                ctx.ExecuteQuery();

                int total = notifications.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add notifications to parent node
                foreach (var notification in notifications)
                {
                    TreeNode node = AddTreeNode(parentNode, string.Format("{0} by {1}", notification.SubscriberType, notification.User), notification, Constants.IMAGE_PUSH_NOTIFICATION_SUBSCRIBER, "Specifies a push notification subscriber.", form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadRoleAssignments(TreeNode parentNode, SPClient.RoleAssignmentCollection roleAssignments, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(roleAssignments);
                ctx.ExecuteQuery();

                int total = roleAssignments.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add role assignments to parent node
                foreach (var roleAssignment in roleAssignments)
                {
                    ctx.Load(roleAssignment.Member);
                    ctx.Load(roleAssignment.RoleDefinitionBindings);
                    ctx.ExecuteQuery();

                    TreeNode node = AddTreeNode(parentNode, string.Format("{0}", roleAssignment.Member.LoginName), roleAssignment, Constants.IMAGE_ROLE_ASSIGNMENT, "Defines the securable object role assignments for a user or group on the Web site, list, or list item.", form.mnContextItem);

                    foreach (var binding in roleAssignment.RoleDefinitionBindings)
                    {
                        LoadRoleDefinition(node, form, binding);
                    }

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadRoleDefinitions(TreeNode parentNode, SPClient.RoleDefinitionCollection roleDefinitions, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(roleDefinitions);
                ctx.ExecuteQuery();

                int total = roleDefinitions.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add role definitions to parent node
                foreach (var roleDefinition in roleDefinitions)
                {
                    LoadRoleDefinition(parentNode, form, roleDefinition);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        private static void LoadRoleDefinition(TreeNode parentNode, MainForm form, SPClient.RoleDefinition roleDefinition)
        {
            try
            {
                TreeNode node = AddTreeNode(parentNode, string.Format("{0}", roleDefinition.Name), roleDefinition, Constants.IMAGE_ROLE_DEFINITIONS, "Defines a single role definition, including a name, description, and set of rights.", form.mnContextItem);

                string[] keys = Enum.GetNames(typeof(SPClient.PermissionKind));
                foreach (string key in keys.OrderBy(k => k))
                {
                    if (roleDefinition.BasePermissions.Has((SPClient.PermissionKind)Enum.Parse(typeof(SPClient.PermissionKind), key)))
                    {
                        SPClient.PermissionKind permission = (SPClient.PermissionKind)Enum.Parse(typeof(SPClient.PermissionKind), key, true);

                        AddTreeNode(node, string.Format("{0} ({1})", permission.ToString(), (int)permission), permission, Constants.IMAGE_ROLE_DEFINITIONS, "Specifies permissions that are used to define user roles.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, NodeLoadType.WebRoleDefinitions);
            }
        }

        public static void LoadWebTemplates(TreeNode parentNode, SPClient.WebTemplateCollection webTemplates, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(webTemplates);
                ctx.ExecuteQuery();

                int total = webTemplates.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add web templates to parent node
                foreach (var template in webTemplates)
                {
                    TreeNode node = AddTreeNode(parentNode, string.Format("{0} ({1})", template.Title, template.Name), template, Constants.IMAGE_WEB_TEMPLATES, "Specifies a site definition or a site template that is used to instantiate a site.", form.mnContextItem);

                    if (template.IsHidden)
                    {
                        node.ForeColor = Color.Gray;
                    }

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadTermStores(TreeNode parentNode, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                TaxonomySession ts = TaxonomySession.GetTaxonomySession(ctx);
                IEnumerable<TermStore> results = ctx.LoadQuery(ts.TermStores.IncludeWithDefaultProperties(
                    t => t.ContentTypePublishingHub));
                ctx.ExecuteQuery();

                int total = results.Count();
                int current = 0;

                foreach (var store in results)
                {
                    TreeNode node = AddTreeNode(parentNode, string.Format("{0}", store.Name), store, Constants.IMAGE_TERM_STORE, "Represents a store that contains metadata within child Group objects, TermSet objects, and Term objects.", form.mnContextItem);

                    // Add loading node
                    AddLoadingNode(node, NodeLoadType.TermGroups);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadTermGroups(TreeNode parentNode, TermGroupCollection termGroups, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                TaxonomySession ts = TaxonomySession.GetTaxonomySession(ctx);
                IEnumerable<TermGroup> results = ctx.LoadQuery(termGroups.IncludeWithDefaultProperties());

                // Load additional properties when current user has appropriate rights
                if (Configuration.Current.LoadAllProperties || ctx.Site.IsCurrentUserAdmin())
                {
                    results = ctx.LoadQuery(termGroups.IncludeWithDefaultProperties(
#if CLIENTSDKV161UP
                        tg => tg.ContributorPrincipalNames,
                        tg => tg.GroupManagerPrincipalNames,
#endif
                        tg => tg.Description));
                }

                ctx.ExecuteQuery();

                int total = results.Count();
                int current = 0;

                foreach (var group in results)
                {
                    TreeNode node = AddTreeNode(parentNode, string.Format("{0}", group.Name), group, Constants.IMAGE_TERM_GROUP, "Represents the top-level container in a TermStore object.", form.mnContextItem);

                    // Add loading node
                    AddLoadingNode(node, NodeLoadType.TermSets);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadTermSets(TreeNode parentNode, TermSetCollection termSets, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                TaxonomySession ts = TaxonomySession.GetTaxonomySession(ctx);
                ctx.Load(termSets);
                ctx.ExecuteQuery();

                int total = termSets.Count;
                int current = 0;

                foreach (var termSet in termSets)
                {
                    TreeNode node = AddTreeNode(parentNode, string.Format("{0}", termSet.Name), termSet, Constants.IMAGE_TERM_SET, "Represents a hierarchical or flat set of Term objects known as a \"TermSet\".", form.mnContextItem);

                    // Add loading node
                    AddLoadingNode(node, NodeLoadType.Terms);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadTerms(TreeNode parentNode, TermCollection terms, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                TaxonomySession ts = TaxonomySession.GetTaxonomySession(ctx);
                ctx.Load(terms);
                ctx.ExecuteQuery();

                int total = terms.Count;
                int current = 0;

                foreach (var term in terms)
                {
                    TreeNode node = AddTreeNode(parentNode, string.Format("{0}", term.Name), term, Constants.IMAGE_TERM, "Represents a Term or a Keyword in a managed metadata hierarchy.", form.mnContextItem);

                    // Add loading node
                    AddLoadingNode(node, NodeLoadType.TermChilds);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadUserProfiles(TreeNode parentNode, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                PeopleManager peopleManager = new PeopleManager(ctx);
                ctx.Load(peopleManager,
                    pm => pm.EditProfileLink,
                    pm => pm.IsMyPeopleListPublic);
                ctx.ExecuteQuery();

                // Add People Manager
                TreeNode node = AddTreeNode(parentNode, "PeopleManager object", peopleManager, Constants.IMAGE_DOTNET_OBJECT, "Provides methods for operations related to people.", form.mnContextItem);

                // Add loading nodes People Manager
                AddLoadingNode(node, "Trending Tags", Constants.IMAGE_TERM, NodeLoadType.UserProfileTrendingTags);

                // Get User Profile object
                ProfileLoader loader = ProfileLoader.GetProfileLoader(ctx);
                UserProfile profile = loader.GetUserProfile();
                ctx.Load(profile,
#if CLIENTSDKV160UP
                    p => p.AccountName,
                    p => p.DisplayName,
                    p => p.IsPeopleListPublic,
                    p => p.IsPrivacySettingOn,
                    p => p.IsSelf,
                    p => p.JobTitle,
                    p => p.MySiteFirstRunExperience,
                    p => p.MySiteHostUrl,
                    p => p.O15FirstRunExperience,
                    p => p.PersonalSiteFirstCreationError,
                    p => p.PersonalSiteFirstCreationTime,
                    p => p.PersonalSiteLastCreationTime,
                    p => p.PersonalSiteNumberOfRetries,
                    p => p.PictureUrl,
                    p => p.PublicUrl,
                    p => p.SipAddress,
                    p => p.FollowPersonalSiteUrl,
#endif
                    p => p.PersonalSiteCapabilities,
                    p => p.PersonalSiteInstantiationState,
                    p => p.PictureImportEnabled,
                    p => p.UrlToCreatePersonalSite,
                    p => p.FollowedContent.FollowedDocumentsUrl,
                    p => p.FollowedContent.FollowedSitesUrl);
                ctx.ExecuteQuery();

                // Add user profile
                TreeNode upNode = AddTreeNode(parentNode, "UserProfile object (current user)", profile, Constants.IMAGE_DOTNET_OBJECT, "Represents a client-side user profile for a person", form.mnContextItem);

                // Add followed content
                TreeNode nodeFollowedContent = AddTreeNode(upNode, "FollowedContent object", profile.FollowedContent, Constants.IMAGE_DOTNET_OBJECT, "Gets a FollowedContent object for the user. SocialFollowingManager is the recommended API to use for Following People and Following Content features.", form.mnContextItem);

                // Add loading nodes
                AddLoadingNode(parentNode, "Current User Profile", Constants.IMAGE_USER_PROFILE, NodeLoadType.UserProfileCurrentUser);
                AddLoadingNode(parentNode, "Other Profiles", Constants.IMAGE_USER_PROFILE, NodeLoadType.UserProfileOtherUsers);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadUserProfileCurrentUser(TreeNode parentNode, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);

                // Load current user in client context
                ctx.Load(ctx.Web.CurrentUser);
                ctx.ExecuteQuery();

                // Load current user profile properties
                LoadUserProfilePersonProperties(parentNode, ctx.Web.CurrentUser.LoginName, form, loadType);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadUserProfileOtherUsers(TreeNode parentNode, MainForm form, NodeLoadType loadType, int position = 0, int batchSize = Constants.SEARCH_BATCH_SIZE)
        {
            DateTime start = DateTime.Now;

            try
            {
                if (batchSize > Constants.SEARCH_ROW_LIMIT)
                {
                    throw new ArgumentOutOfRangeException("batchSize", "The batch size is larger than the maximum allowed row limit.");
                }

                SPClient.ClientContext ctx = GetClientContext(parentNode);

                // Define people search query
                KeywordQuery keywordQuery = new KeywordQuery(ctx);
                keywordQuery.QueryText = "*";
                keywordQuery.SourceId = new Guid(Constants.LOCAL_PEOPLE_RESULTS_CONTENT_SOURCE_ID);
                keywordQuery.StartRow = position;
                keywordQuery.RowLimit = batchSize;
                keywordQuery.TrimDuplicates = false;

                // Execute people search query
                SearchExecutor searchExecutor = new SearchExecutor(ctx);
                ClientResult<ResultTableCollection> results = searchExecutor.ExecuteQuery(keywordQuery);
                ctx.ExecuteQuery();

                int totalCount = results.Value[0].TotalRowsIncludingDuplicates;
                int resultCount = results.Value[0].ResultRows.Count();
                int current = 0;

                // Load user profiles based on search result
                foreach (var resultRow in results.Value[0].ResultRows)
                {
                    // Get account name and load profile
                    string accountName = resultRow["AccountName"].ToString();
                    LoadUserProfilePersonProperties(parentNode, accountName, form, loadType);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = resultCount, CurrentItem = current });

                    position++;
                }

                DateTime end = DateTime.Now;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, totalCount, position);

                // Raise batch complete event
                UserProfilesBatchCompleted(null, new BatchCompletedEventArgs()
                {
                    CurrentItem = position,
                    BatchSize = batchSize,
                    TotalItems = totalCount,
                    Duration = end - start
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadUserProfilePersonProperties(TreeNode parentNode, string accountName, MainForm form, NodeLoadType loadType)
        {
            // http://msdn.microsoft.com/en-us/library/jj163800.aspx

            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                PeopleManager peopleManager = new PeopleManager(ctx);
                PersonProperties personProperties = peopleManager.GetPropertiesFor(accountName);
                ctx.Load(personProperties,
                    p => p.AccountName,
                    p => p.DisplayName,
                    p => p.Email,
                    p => p.IsFollowed,
                    p => p.PersonalUrl,
                    p => p.PictureUrl,
                    p => p.Title,
                    p => p.LatestPost,
                    p => p.UserUrl);
                ctx.ExecuteQuery();

                // Add user profile
                TreeNode node = AddTreeNode(parentNode, string.Format("{0} ({1})", personProperties.DisplayName, personProperties.Email), personProperties, Constants.IMAGE_USER_PROFILE, "Represents user properties.", form.mnContextItem);

                // Check if is current user
                if (!ctx.Web.CurrentUser.IsPropertyAvailable("LoginName"))
                {
                    ctx.Load(ctx.Web.CurrentUser,
                        w => w.LoginName);
                    ctx.ExecuteQuery();
                }
                bool isPersonCurrentUser = ctx.Web.CurrentUser.LoginName.Equals(personProperties.AccountName);

                // Add loading node
                AddLoadingNode(node, "Properties", Constants.IMAGE_USER_PROFILE_PROPERTY, NodeLoadType.UserProfileProperties);
                AddLoadingNode(node, "Peers", Constants.IMAGE_USER_PROFILE_PEERS, NodeLoadType.UserProfilePeers);
                AddLoadingNode(node, "Direct Reports", Constants.IMAGE_USER_PROFILE_DIRECT_REPORTS, NodeLoadType.UserProfileDirectReports);
                AddLoadingNode(node, "Extended Managers", Constants.IMAGE_USER_PROFILE_EXTENDED_MANAGERS, NodeLoadType.UserProfileExtendedManagers);
                AddLoadingNode(node, "Extended Reports", Constants.IMAGE_USER_PROFILE_EXTENDED_REPORTS, NodeLoadType.UserProfileExtendedReports);
                if (isPersonCurrentUser) AddLoadingNode(node, "Followed Tags (only current user)", Constants.IMAGE_TERM, NodeLoadType.UserProfileFollowedTags);
                AddLoadingNode(node, "Followers", Constants.IMAGE_USER_PROFILE, NodeLoadType.UserProfileFollowers);
                if (isPersonCurrentUser) AddLoadingNode(node, "Suggestions (only current user)", Constants.IMAGE_USER_PROFILE, NodeLoadType.UserProfileSuggestions);
                AddLoadingNode(node, "People Followed By User", Constants.IMAGE_USER_PROFILE, NodeLoadType.UserProfilePeopleFollowedByUser);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadUserProfileProperties(TreeNode parentNode, PersonProperties personProperties, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(personProperties,
                    p => p.UserProfileProperties);
                ctx.ExecuteQuery();

                // Add profile properties
                int total = personProperties.UserProfileProperties.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                foreach (var property in personProperties.UserProfileProperties)
                {
                    TreeNode profileNode = AddTreeNode(parentNode, string.Format("{0}: {1}", property.Key, property.Value), property, Constants.IMAGE_USER_PROFILE_PROPERTY, string.Empty, form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadUserProfilePeers(TreeNode parentNode, PersonProperties personProperties, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(personProperties,
                    p => p.Peers);
                ctx.ExecuteQuery();

                // Add profile properties
                int total = personProperties.Peers.Count();
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                foreach (var property in personProperties.Peers)
                {
                    TreeNode profileNode = AddTreeNode(parentNode, string.Format("{0}", property), property, Constants.IMAGE_USER_PROFILE_PEERS, string.Empty, form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadUserProfileDirectReports(TreeNode parentNode, PersonProperties personProperties, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(personProperties,
                    p => p.DirectReports);
                ctx.ExecuteQuery();

                // Add profile properties
                int total = personProperties.DirectReports.Count();
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                foreach (var property in personProperties.DirectReports)
                {
                    TreeNode profileNode = AddTreeNode(parentNode, string.Format("{0}", property), property, Constants.IMAGE_USER_PROFILE_DIRECT_REPORTS, string.Empty, form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadUserProfileExtendedManagers(TreeNode parentNode, PersonProperties personProperties, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(personProperties,
                    p => p.ExtendedManagers);
                ctx.ExecuteQuery();

                // Add profile properties
                int total = personProperties.ExtendedManagers.Count();
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                foreach (var property in personProperties.ExtendedManagers)
                {
                    TreeNode profileNode = AddTreeNode(parentNode, string.Format("{0}", property), property, Constants.IMAGE_USER_PROFILE_EXTENDED_MANAGERS, string.Empty, form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadUserProfileExtendedReports(TreeNode parentNode, PersonProperties personProperties, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(personProperties,
                    p => p.ExtendedReports);
                ctx.ExecuteQuery();

                // Add profile properties
                int total = personProperties.ExtendedReports.Count();
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                foreach (var property in personProperties.ExtendedReports)
                {
                    TreeNode profileNode = AddTreeNode(parentNode, string.Format("{0}", property), property, Constants.IMAGE_USER_PROFILE_EXTENDED_REPORTS, string.Empty, form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadUserProfileFollowedTags(TreeNode parentNode, PersonProperties personProperties, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                PeopleManager peopleManager = new PeopleManager(ctx);
                ClientArrayResult<string> followedTags = peopleManager.GetFollowedTags(1000);
                ctx.ExecuteQuery();

                // Add profile properties
                int total = followedTags.Value.Length;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                foreach (var tag in followedTags.Value)
                {
                    TreeNode profileNode = AddTreeNode(parentNode, string.Format("{0}", tag), tag, Constants.IMAGE_TERM, string.Empty, form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadUserProfileFollowers(TreeNode parentNode, PersonProperties personProperties, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                PeopleManager peopleManager = new PeopleManager(ctx);
                ClientObjectList<PersonProperties> followers = peopleManager.GetFollowersFor(((PersonProperties)parentNode.Parent.Tag).AccountName);
                ctx.Load(followers);
                ctx.ExecuteQuery();

                // Add profile properties
                int total = followers.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                foreach (var follower in followers)
                {
                    TreeNode profileNode = AddTreeNode(parentNode, string.Format("{0}", follower.DisplayName), follower, Constants.IMAGE_USER_PROFILE, "Represents user properties.", form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadUserProfileSuggestions(TreeNode parentNode, PersonProperties personProperties, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                PeopleManager peopleManager = new PeopleManager(ctx);
                ClientObjectList<PersonProperties> suggestions = peopleManager.GetMySuggestions();
                ctx.Load(suggestions);
                ctx.ExecuteQuery();

                // Add profile properties
                int total = suggestions.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                foreach (var suggestion in suggestions)
                {
                    TreeNode profileNode = AddTreeNode(parentNode, string.Format("{0}", suggestion.DisplayName), suggestion, Constants.IMAGE_USER_PROFILE, "Represents user properties.", form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadUserProfilePeopleFollowedByUser(TreeNode parentNode, PersonProperties personProperties, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(ctx.Web.CurrentUser);
                PeopleManager peopleManager = new PeopleManager(ctx);
                ClientObjectList<PersonProperties> followers = peopleManager.GetPeopleFollowedBy(((PersonProperties)parentNode.Parent.Tag).AccountName);
                ctx.Load(followers);
                ctx.ExecuteQuery();

                // Add profile properties
                int total = followers.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                foreach (var follower in followers)
                {
                    TreeNode profileNode = AddTreeNode(parentNode, string.Format("{0}", follower.DisplayName), follower, Constants.IMAGE_USER_PROFILE, "Represents user properties.", form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadUserProfileTrendingTags(TreeNode parentNode, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                HashTagCollection tags = PeopleManager.GetTrendingTags(ctx);
                ctx.Load(tags);
                ctx.ExecuteQuery();

                // Add profile properties
                int total = tags.Count;
                int current = 0;

                foreach (var tag in tags)
                {
                    TreeNode profileNode = AddTreeNode(parentNode, string.Format("{0} ({1})", tag.Name, tag.UseCount), tag, Constants.IMAGE_TERM, string.Empty, form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadSPOWebTemplates(TreeNode parentNode, SPTenantAdmin.SPOTenantWebTemplateCollection webTemplates, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(webTemplates);
                ctx.ExecuteQuery();

                int total = webTemplates.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add templates to parent node
                foreach (var template in webTemplates)
                {
                    TreeNode node = AddTreeNode(parentNode, string.Format("{0} ({1})", template.Title, template.Name), template, Constants.IMAGE_WEB_TEMPLATES, "Specifies a site definition or a site template that is used to instantiate a site.", form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadSiteProperties(TreeNode parentNode, SPTenantAdmin.Tenant tenant, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);

                // Add site collections
                SPTenantAdmin.SPOSitePropertiesEnumerable spp = null;

                int total = 0;
                int current = 0;

                // Iterate site collections
                while (spp == null || spp.Count > 0)
                {
                    spp = tenant.GetSiteProperties(current, true);
                    ctx.Load(spp);
                    ctx.ExecuteQuery();

                    total += spp.Count;

                    foreach (SPTenantAdmin.SiteProperties sp in spp)
                    {
                        TreeNode node = AddTreeNode(parentNode, sp.Url, sp, Constants.IMAGE_SITE_PROPERTIES, "Contains a property bag of information about a site.", form.mnContextItem);

                        // Update progress
                        current++;
                        ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                    }
                }

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadDeletedSiteProperties(TreeNode parentNode, SPTenantAdmin.Tenant tenant, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);

                // Add site collections
                SPTenantAdmin.SPODeletedSitePropertiesEnumerable spp = null;

                int total = 0;
                int current = 0;

                // Iterate site collections
                while (spp == null || spp.Count > 0)
                {
                    spp = tenant.GetDeletedSiteProperties(current);
                    ctx.Load(spp);
                    ctx.ExecuteQuery();

                    total += spp.Count;

                    foreach (SPTenantAdmin.DeletedSiteProperties sp in spp)
                    {
                        TreeNode node = AddTreeNode(parentNode, sp.Url, sp, Constants.IMAGE_DELETED_SITE_PROPERTIES, "Contains information about deleted sites.", form.mnContextItem);

                        // Update progress
                        current++;
                        ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                    }
                }

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadExternalUsers(TreeNode parentNode, SPTenantMngt.Office365Tenant tenant, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);

                int position = 0;
                bool hasMore = true;

                while (hasMore)
                {
                    SPTenantMngt.GetExternalUsersResults results = tenant.GetExternalUsers(position, 50, string.Empty, SPTenantMngt.SortOrder.Ascending);
                    ctx.Load(results,
                        //r => r.ExternalUserCollection.Include(k => k.InvitedBy),
                        r => r.UserCollectionPosition,
                        r => r.TotalUserCount,
                        r => r.ExternalUserCollection);
                    ctx.ExecuteQuery();

                    int total = results.TotalUserCount;
                    int current = 0;

                    // Add count to parent node
                    UpdateCountChildNodes(parentNode, total);

                    // Add external users to parent node
                    foreach (SPTenantMngt.ExternalUser user in results.ExternalUserCollection)
                    {
                        TreeNode node = AddTreeNode(parentNode, string.Format("{0} ({1})", user.DisplayName, user.InvitedAs), user, Constants.IMAGE_EXTERNAL_USERS, "Represents an external user in Office 365.", form.mnContextItem);

                        // Update progress
                        current++;
                        ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });

                        position++;
                    }

                    hasMore = (results.TotalUserCount > position);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadApps(TreeNode parentNode, SPTenantAdmin.Tenant tenant, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);

                var apps = tenant.GetAppInfoByName(string.Empty);
                ctx.Load(apps);
                ctx.ExecuteQuery();

                int total = apps.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add apps to parent node
                foreach (SPTenantAdmin.AppInfo app in apps)
                {
                    TreeNode node = AddTreeNode(parentNode, string.Format("{0}", app.Name), app, Constants.IMAGE_APP_INSTANCES, "", form.mnContextItem);

                    // Add loading nodes
                    AddLoadingNode(node, "App Errors", "Gets errors from the app monitoring framework for the specified app within tenant scope.", Constants.IMAGE_APP_ERROR, NodeLoadType.AppErrors);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadAppErrors(TreeNode parentNode, ClientObjectList<SPTenantAdmin.AppErrorEntry> errors, MainForm form, NodeLoadType loadType)
        {
            // TODO: Refactor!!
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);

                ctx.Load(errors);
                ctx.ExecuteQuery();

                int total = errors.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add errors to parent node
                foreach (SPTenantAdmin.AppErrorEntry error in errors)
                {
                    TreeNode node = AddTreeNode(parentNode, string.Format("{0} - {1}", error.TimeStampInUTC, error.ErrorType), error, Constants.IMAGE_APP_ERROR, "", form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadProjectPolicy(TreeNode parentNode, Web web, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);

                ClientResult<bool> doesProjectHavePolicy = ProjectPolicy.DoesProjectHavePolicy(ctx, web);
                ClientResult<bool> isProjectClosed = ProjectPolicy.IsProjectClosed(ctx, web);
                ClientResult<DateTime> projectCloseDate = ProjectPolicy.GetProjectCloseDate(ctx, web);
                ClientResult<DateTime> projectExpirationDate = ProjectPolicy.GetProjectExpirationDate(ctx, web);

                ctx.ExecuteQuery();

                // Add nodes
                AddTreeNode(parentNode, string.Format("Does Project Have Policy: {0}", doesProjectHavePolicy.Value), doesProjectHavePolicy, Constants.IMAGE_PROJECT_POLICY, "Checks whether the site has a policy applied.", form.mnContextItem);
                AddTreeNode(parentNode, string.Format("Is Project Closed: {0}", isProjectClosed.Value), isProjectClosed, Constants.IMAGE_PROJECT_POLICY, "Checks whether the site is closed.", form.mnContextItem);
                AddTreeNode(parentNode, string.Format("Project Close Date: {0}", projectCloseDate.Value), projectCloseDate, Constants.IMAGE_PROJECT_POLICY, "Returns the close date of the site.", form.mnContextItem);
                AddTreeNode(parentNode, string.Format("Project Expiration Date: {0}", projectExpirationDate.Value), projectExpirationDate, Constants.IMAGE_PROJECT_POLICY, "Returns the expiration date of the site.", form.mnContextItem);

                // Add loading node
                AddLoadingNode(parentNode, "Project Policies", "Returns all policies that are available on the site.", Constants.IMAGE_PROJECT_POLICY, NodeLoadType.ProjectPolicies);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadProjectPolicies(TreeNode parentNode, Web web, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);

                ClientObjectList<ProjectPolicy> policies = ProjectPolicy.GetProjectPolicies(ctx, web);

                ctx.Load(policies);
                ctx.ExecuteQuery();

                int total = policies.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add policies to parent node
                foreach (ProjectPolicy policy in policies)
                {
                    TreeNode node = AddTreeNode(parentNode, policy.Name, policy, Constants.IMAGE_PROJECT_POLICY, "", form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadTenantLogs(TreeNode parentNode, MainForm form, NodeLoadType loadType)
        {
            // TODO: Refactor!!            
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);

                SPTenantAdmin.TenantLog log = new SPTenantAdmin.TenantLog(ctx);
                //ClientObjectList<SPTenantAdmin.TenantLogEntry> errors = log.GetEntries(DateTime.UtcNow.AddMonths(-Constants.ERROR_LOGS_ROW_TIME_FRAME_IN_MONTHS), DateTime.UtcNow, Constants.ERROR_LOGS_ROW_LIMIT);
                ClientObjectList<SPTenantAdmin.TenantLogEntry> errors = log.GetEntries(DateTime.UtcNow.AddDays(-5), DateTime.UtcNow, 50);

                ctx.Load(errors);
                ctx.ExecuteQuery();

                int total = errors.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add errors to parent node
                foreach (SPTenantAdmin.TenantLogEntry error in errors)
                {
                    TreeNode node = AddTreeNode(parentNode, error.TimestampUtc.ToString(), error, Constants.IMAGE_TENANT_ERROR, "", form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadTimeZones(TreeNode parentNode, TimeZoneCollection timeZones, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(timeZones);
                ctx.ExecuteQuery();

                int total = timeZones.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add templates to parent node
                foreach (var timeZone in timeZones)
                {
                    TreeNode node = AddTreeNode(parentNode, string.Format("{0} ({1})", timeZone.Description, timeZone.Id), timeZone, Constants.IMAGE_TIME_ZONES, "Represents the time zone setting that is implemented on a SharePoint Web site.", form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

#if CLIENTSDKV160UP
        public static void LoadAppTiles(TreeNode parentNode, SPClient.AppTileCollection appTiles, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(appTiles);
                ctx.ExecuteQuery();

                int total = appTiles.Count;
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add app titles to parent node
                foreach (var appTile in appTiles)
                {
                    TreeNode node = AddTreeNode(parentNode, string.Format("{0} ({1})", appTile.Title, appTile.AppType), appTile, Constants.IMAGE_APP_TILE, string.Empty, form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }
#endif
#if CLIENTSDKV161UP
        internal static void LoadAlerts(TreeNode parentNode, AlertCollection alerts, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                //ctx.Load(alerts);
                IEnumerable<Alert> result = ctx.LoadQuery(alerts.IncludeWithDefaultProperties(
                    a => a.User));
                ctx.ExecuteQuery();

                int total = result.Count();
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add fields to parent node
                foreach (SPClient.Alert alert in result)
                {
                    TreeNode node = AddTreeNode(parentNode, string.Format("{0} (for {1})", alert.Title, alert.User.LoginName), alert, Constants.IMAGE_ALERT, "", form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }

        internal static void LoadFileVersionEvents(TreeNode parentNode, FileVersionEventCollection versionEvents, MainForm form, NodeLoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(versionEvents);
                ctx.ExecuteQuery();

                int total = versionEvents.Count();
                int current = 0;

                // Add count to parent node
                UpdateCountChildNodes(parentNode, total);

                // Add fields to parent node
                foreach (SPClient.FileVersionEvent versionEvent in versionEvents)
                {
                    TreeNode node = AddTreeNode(parentNode, string.Format("{0} ({1})", versionEvent.Time, versionEvent.EventType), versionEvent, Constants.IMAGE_FILE_VERSIONS_EVENT, "", form.mnContextItem);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);

                AddLoadingNode(parentNode, loadType);
            }
        }
#endif

        public static SPClient.ClientContext GetClientContext(TreeNode node)
        {
            SPClient.ClientContext clientContext = null;

            if (node != null)
            {
                switch (node.Tag.GetClientType())
                {
                    case ClientType.Tenant:
                        string rootSiteUrl = ((SPTenantAdmin.Tenant)node.Tag).RootSiteUrl;

                        // Get current tenant
                        TenantAuthentication tenantAuthn = Globals.Tenants[rootSiteUrl];

                        if (tenantAuthn != null)
                            clientContext = tenantAuthn.ClientContext;
                        break;
                    case ClientType.Site:
                        string siteUrl = ((SPClient.Site)node.Tag).Url;
                        SiteAuthentication siteAuthn = Globals.Sites[siteUrl];

                        if (siteAuthn != null)
                            clientContext = siteAuthn.ClientContext;
                        break;
                    default:
                        break;
                }

                if (clientContext != null)
                    return clientContext;

                return GetClientContext(node.Parent);
            }

            return clientContext;
        }

        /// <summary>
        /// Updates the count child nodes.
        /// </summary>
        /// <remarks>
        /// Sample values which are supported by the Regex pattern:
        /// Node Text (20)
        /// Node Text (47.820)
        /// Node Text (47,820)
        /// Node Text (4.747.820) » not supported at the moment
        /// Node Text (39/38)
        /// Node Text (39/4.038)
        /// Node Text (4,539/4.038)
        /// </remarks>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="total">The total.</param>
        /// <param name="position">The position.</param>
        private static void UpdateCountChildNodes(TreeNode parentNode, int total, int position = -1)
        {
            string pattern = @"\s(\(((?:\d*(\.|\,))?\d+|(?:\d*(\.|\,))?\d+\/(?:\d*(\.|\,))?\d+)\))";
            string text = System.Text.RegularExpressions.Regex.Replace(parentNode.Text, pattern, string.Empty);

            parentNode.Text = string.Format("{0} ({2}{1})",
                text,
                total.ToString("N0"),
                position == -1 ? string.Empty : string.Format("{0}/", position.ToString("N0")));
        }

        private static void AddLoadingNode(TreeNode parentNode, NodeLoadType loadType)
        {
            TreeNode node = parentNode.Nodes.Add(Constants.NODE_LOADING_TEXT);
            node.Tag = loadType;

            parentNode.Collapse();
        }

        private static void AddLoadingNode(TreeNode parentNode, string nodeText, string imageKey, NodeLoadType tag)
        {
            AddLoadingNode(parentNode, nodeText, string.Empty, imageKey, tag);
        }

        private static void AddLoadingNode(TreeNode parentNode, string nodeText, string toolTipText, string imageKey, NodeLoadType tag)
        {
            // Excluded node loading when site collection version is less than SharePoint 2013 (SharePoint 15)
            switch (tag)
            {
                case NodeLoadType.SiteEventReceivers:
                    if (!((SPClient.Site)parentNode.Tag).Context.IsMinimalServerVersion(ServerVersion.SharePoint2013))
                        return;
                    break;
                case NodeLoadType.WebUsers:
                case NodeLoadType.WebEventReceivers:
                case NodeLoadType.WebRecycleBin:
                    if (!((SPClient.Web)parentNode.Tag).Context.IsMinimalServerVersion(ServerVersion.SharePoint2013))
                        return;
                    break;
                case NodeLoadType.ListEventReceivers:
                    if (!((SPClient.List)parentNode.Tag).Context.IsMinimalServerVersion(ServerVersion.SharePoint2013))
                        return;
                    break;
                default:
                    break;
            }

            TreeNode node = parentNode.Nodes.Add(nodeText);
            node.ImageKey = imageKey;
            node.SelectedImageKey = imageKey;
            node.Tag = tag;
            node.ToolTipText = toolTipText;
            node.ContextMenuStrip = ((MainForm)parentNode.TreeView.FindForm()).mnContextCollection;

            node.Nodes.Add(Constants.NODE_LOADING_TEXT);

            node.Collapse();
        }

        /// <summary>
        /// Add a node to the treeview.
        /// </summary>
        /// <param name="parentNode">Parent node for current object.</param>
        /// <param name="title">Title for tree node.</param>
        /// <param name="tag">Current object to load.</param>
        /// <param name="image">Internal image name, located in <see cref="Constants"/>.</param>
        /// <param name="toolTip">Tooltip text for additional contextual information.</param>
        /// <param name="contextMenu">Optional context menu to show when right click tree node.</param>
        /// <returns>Returns the newly added tree node.</returns>
        private static TreeNode AddTreeNode(TreeNode parentNode, string title, object tag, string image, string toolTip = "", ContextMenuStrip contextMenu = null)
        {
            TreeNode siteNode = parentNode.Nodes.Add(title);
            siteNode.Name = title;
            siteNode.Tag = tag;
            siteNode.ImageKey = image;
            siteNode.SelectedImageKey = image;
            siteNode.ToolTipText = toolTip;
            siteNode.ContextMenuStrip = contextMenu;

            if (tag == null)
            {
                // When data is not available make text color gray.
                siteNode.ForeColor = Color.Gray;
            }

            LogUtil.LogMessage(string.Format("Added {1} node '{0}'", title, tag == null ? string.Empty : tag.GetType().ToString()), LogLevel.Verbose, 0, LogCategory.TreeViewLoader);

            return siteNode;
        }

        public static void LoadRawData(TreeNode selectedNode, DataGridView gvRawData)
        {
            LoadRawListData(selectedNode, gvRawData);
            LoadRawItemData(selectedNode, gvRawData);
            LoadRawKeyValueData(selectedNode, gvRawData);
            LoadRawUserProfileData(selectedNode, gvRawData);
            LoadRawUserInfoData(selectedNode, gvRawData);
        }

        private static void LoadRawUserInfoData(TreeNode selectedNode, DataGridView gvRawData)
        {
            try
            {
                if (selectedNode.Tag is NodeLoadType && ((NodeLoadType)selectedNode.Tag) == NodeLoadType.WebUsers)
                {
                    // Reset the grid
                    gvRawData.Columns.Clear();
                    gvRawData.Rows.Clear();

                    // Get list data
                    Web web = (Web)selectedNode.Parent.Tag;
                    UserCollection users = web.SiteUsers;
                    web.Context.Load(users);
                    web.Context.ExecuteQuery();

                    // Add columns
                    gvRawData.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Email", HeaderText = "Email", DataPropertyName = "Email", ReadOnly = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
                    gvRawData.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Id", HeaderText = "Id", DataPropertyName = "Id", ReadOnly = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
                    gvRawData.Columns.Add(new DataGridViewTextBoxColumn() { Name = "IsHiddenInUI", HeaderText = "IsHiddenInUI", DataPropertyName = "IsHiddenInUI", ReadOnly = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
                    gvRawData.Columns.Add(new DataGridViewTextBoxColumn() { Name = "IsSiteAdmin", HeaderText = "IsSiteAdmin", DataPropertyName = "IsSiteAdmin", ReadOnly = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
                    gvRawData.Columns.Add(new DataGridViewTextBoxColumn() { Name = "LoginName", HeaderText = "LoginName", DataPropertyName = "LoginName", ReadOnly = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
                    gvRawData.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Title", HeaderText = "Title", DataPropertyName = "Title", ReadOnly = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });

                    int total = users.Count;
                    int current = 0;

                    // Add rows
                    foreach (SPClient.User user in users)
                    {
                        DataGridViewRow row = gvRawData.Rows[gvRawData.Rows.Add()];
                        row.Cells["Title"].Value = user.Title;
                        row.Cells["LoginName"].Value = user.LoginName;
                        row.Cells["Email"].Value = user.Email;
                        row.Cells["Id"].Value = user.Id;
                        row.Cells["IsHiddenInUI"].Value = user.IsHiddenInUI;
                        row.Cells["IsSiteAdmin"].Value = user.IsSiteAdmin;

                        // Update progress
                        current++;
                        ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private static void LoadRawItemData(TreeNode selectedNode, DataGridView gvRawData)
        {
            const string COLUMN_NAME_KEY = "Field";
            const string COLUMN_NAME_VALUE = "Value";

            try
            {
                SPClient.ListItem item = selectedNode.Tag as SPClient.ListItem;

                // Retrieve and set list item field values
                if (item != null)
                {
                    // Reset the grid
                    gvRawData.Columns.Clear();
                    gvRawData.Rows.Clear();

                    // Add columns
                    DataGridViewColumn columnKey = new DataGridViewTextBoxColumn();
                    columnKey.Name = COLUMN_NAME_KEY;
                    columnKey.HeaderText = COLUMN_NAME_KEY;
                    columnKey.ToolTipText = "This shows the InternalName for the field.";
                    columnKey.ReadOnly = true;
                    columnKey.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    gvRawData.Columns.Add(columnKey);

                    DataGridViewColumn columnValue = new DataGridViewTextBoxColumn();
                    columnValue.Name = COLUMN_NAME_VALUE;
                    columnValue.HeaderText = COLUMN_NAME_VALUE;
                    columnValue.ReadOnly = true;
                    columnValue.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    gvRawData.Columns.Add(columnValue);

                    int total = item.FieldValues.Count;
                    int current = 0;

                    // Load fields for current item
                    SPClient.ClientContext ctx = NodeLoader.GetClientContext(selectedNode);
                    FieldCollection fields = item.ParentList.Fields;
                    ctx.Load(fields);
                    ctx.ExecuteQuery();

                    // Add rows
                    foreach (string key in item.FieldValues.Keys)
                    {
                        DataGridViewRow row = gvRawData.Rows[gvRawData.Rows.Add()];

                        // Get field related to column
                        SPClient.Field field = fields.GetByInternalNameOrTitle(key);
                        field = fields.SingleOrDefault(f => f.InternalName.Equals(key));

                        // Set field
                        row.Cells[COLUMN_NAME_KEY].Value = field.InternalName;
                        row.Cells[COLUMN_NAME_KEY].ToolTipText = string.Format("Title: {4}\nInternalName: {0}\nStaticName: {1}\nTypeAsString: {2}\nType: {3}",
                            field.InternalName,
                            field.StaticName,
                            field.TypeAsString,
                            field.GetType(),
                            field.Title);

                        // Set field value
                        string value = GetFieldValue(item, field);
                        row.Cells[COLUMN_NAME_VALUE].Value = value;
                        row.Cells[COLUMN_NAME_VALUE].ToolTipText = value.Replace(string.Format(" {0} ", DELIMITER), "\n");

                        // Update progress
                        current++;
                        ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                    }

                    // Sort grid
                    gvRawData.Sort(columnKey, System.ComponentModel.ListSortDirection.Ascending);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private static void LoadRawListData(TreeNode selectedNode, DataGridView gvRawData)
        {
            try
            {
                SPClient.List list = selectedNode.Tag as SPClient.List;
                if (list != null)
                {
                    // Reset the grid
                    gvRawData.Columns.Clear();
                    gvRawData.Rows.Clear();

                    // Get list data
                    SPClient.CamlQuery query = new SPClient.CamlQuery();
                    query.ViewXml = string.Format("<View Scope='RecursiveAll'><RowLimit>{0}</RowLimit></View>", Constants.LIST_ROW_LIMIT);

                    SPClient.ClientContext ctx = NodeLoader.GetClientContext(selectedNode);
                    SPClient.ListItemCollection items = list.GetItems(query);
                    ctx.Load(items);
                    ctx.Load(list.Fields);
                    ctx.ExecuteQuery();

                    // Show warning when List.ItemCount is greater than RowLimit
                    if (list.ItemCount > Constants.LIST_ROW_LIMIT)
                    {
                        string warning = string.Format("The list contains more than {0} items, this view is limited to the top {0} items.", Constants.LIST_ROW_LIMIT);
                        MessageBox.Show(warning, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        LogUtil.LogMessage(warning);
                    }

                    // Add columns
                    foreach (SPClient.Field field in list.Fields)
                    {
                        DataGridViewColumn column = new DataGridViewTextBoxColumn();
                        column.Name = field.InternalName;
                        column.HeaderText = field.Title;
                        column.DataPropertyName = field.InternalName;
                        column.ReadOnly = true;
                        column.Tag = field;
                        column.ToolTipText = string.Format("InternalName: {0}\nStaticName: {1}\nTypeAsString: {2}\nType: {3}", field.InternalName, field.StaticName, field.TypeAsString, field.GetType());

                        if (field.Hidden)
                        {
                            DataGridViewCellStyle style = new DataGridViewCellStyle();
                            style.ForeColor = Color.Gray;

                            column.HeaderCell.Style = style;
                        }

                        gvRawData.Columns.Add(column);
                    }

                    int total = items.Count;
                    int current = 0;

                    // Add rows
                    foreach (SPClient.ListItem item in items)
                    {
                        DataGridViewRow row = gvRawData.Rows[gvRawData.Rows.Add()];

                        foreach (DataGridViewColumn column in gvRawData.Columns)
                        {
                            if (item.FieldValues.ContainsKey(column.Name) && item.FieldValues[column.Name] != null)
                            {
                                // Get field related to column
                                SPClient.Field field = (SPClient.Field)column.Tag;

                                // Get field value
                                row.Cells[column.Name].Value = GetFieldValue(item, field);
                            }
                        }

                        // Update progress
                        current++;
                        ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private static void LoadRawKeyValueData(TreeNode selectedNode, DataGridView gvRawData)
        {
            const string COLUMN_NAME_KEY = "Key";
            const string COLUMN_NAME_VALUE = "Value";

            try
            {
                SPClient.PropertyValues values = null;

                if (selectedNode.Tag.GetType() == typeof(NodeLoadType))
                {
                    NodeLoadType nodeType = (NodeLoadType)selectedNode.Tag;

                    switch (nodeType)
                    {
                        case NodeLoadType.WebProperties:
                            values = (selectedNode.Parent.Tag as Web).AllProperties;
                            break;
                        case NodeLoadType.FolderProperties:
                            values = (selectedNode.Parent.Tag as Folder).Properties;
                            break;
                        case NodeLoadType.WebPartProperties:
                            values = (selectedNode.Parent.Tag as SPClient.WebParts.WebPart).Properties;
                            break;
#if CLIENTSDKV160UP
                        case NodeLoadType.FileProperties:
                            values = (selectedNode.Parent.Tag as File).Properties;
                            break;
#endif
#if CLIENTSDKV161UP
                        case NodeLoadType.ItemProperties:
                            values = (selectedNode.Parent.Tag as ListItem).Properties;
                            break;
#endif
                        case NodeLoadType.SiteProperties: // TODO: Implement SiteProperties Raw Data tab view
                        case NodeLoadType.DeletedSiteProperties: // TODO: Implement DeletedSiteProperties Raw Data tab view
                        case NodeLoadType.UserProfileProperties: // Implemented within LoadRawUserProfileData() method
                        default:
                            break;
                    }
                }

                if (values != null)
                {
                    SPClient.ClientContext ctx = NodeLoader.GetClientContext(selectedNode);
                    ctx.Load(values);
                    ctx.ExecuteQuery();

                    // Retrieve and set key value pair data
                    if (values != null)
                    {
                        // Reset the grid
                        gvRawData.Columns.Clear();
                        gvRawData.Rows.Clear();

                        // Add columns
                        DataGridViewColumn columnKey = new DataGridViewTextBoxColumn();
                        columnKey.Name = COLUMN_NAME_KEY;
                        columnKey.HeaderText = COLUMN_NAME_KEY;
                        columnKey.ReadOnly = true;
                        columnKey.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        gvRawData.Columns.Add(columnKey);

                        DataGridViewColumn columnValue = new DataGridViewTextBoxColumn();
                        columnValue.Name = COLUMN_NAME_VALUE;
                        columnValue.HeaderText = COLUMN_NAME_VALUE;
                        columnValue.ReadOnly = true;
                        columnValue.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        gvRawData.Columns.Add(columnValue);

                        int total = values.FieldValues.Count;
                        int current = 0;

                        // Add rows
                        foreach (string key in values.FieldValues.Keys)
                        {
                            DataGridViewRow row = gvRawData.Rows[gvRawData.Rows.Add()];

                            // Set field
                            row.Cells[COLUMN_NAME_KEY].Value = key;

                            // Set field value
                            row.Cells[COLUMN_NAME_VALUE].Value = values[key];

                            // Update progress
                            current++;
                            ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                        }

                        // Sort grid
                        gvRawData.Sort(columnKey, System.ComponentModel.ListSortDirection.Ascending);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private static void LoadRawUserProfileData(TreeNode selectedNode, DataGridView gvRawData)
        {
            const string COLUMN_NAME_KEY = "Field";
            const string COLUMN_NAME_VALUE = "Value";

            try
            {
                PersonProperties properties = selectedNode.Tag as PersonProperties;

                // Retrieve and set list item field values
                if (properties != null)
                {
                    // Reset the grid
                    gvRawData.Columns.Clear();
                    gvRawData.Rows.Clear();

                    // Add columns
                    DataGridViewColumn columnKey = new DataGridViewTextBoxColumn();
                    columnKey.Name = COLUMN_NAME_KEY;
                    columnKey.HeaderText = COLUMN_NAME_KEY;
                    columnKey.ToolTipText = "This shows the Name for the profile property.";
                    columnKey.ReadOnly = true;
                    columnKey.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    gvRawData.Columns.Add(columnKey);

                    DataGridViewColumn columnValue = new DataGridViewTextBoxColumn();
                    columnValue.Name = COLUMN_NAME_VALUE;
                    columnValue.HeaderText = COLUMN_NAME_VALUE;
                    columnValue.ToolTipText = "A user's privacy settings affect which properties can be retrieved. Multiple values are delimited by the pipe (|) sign and a null reference values are passed as empty strings.";
                    columnValue.ReadOnly = true;
                    columnValue.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    gvRawData.Columns.Add(columnValue);

                    // Load fields for current item
                    SPClient.ClientContext ctx = NodeLoader.GetClientContext(selectedNode);
                    ctx.Load(properties,
                        p => p.UserProfileProperties);
                    ctx.ExecuteQuery();

                    int total = properties.UserProfileProperties.Count;
                    int current = 0;

                    // Add rows
                    foreach (var property in properties.UserProfileProperties)
                    {
                        DataGridViewRow row = gvRawData.Rows[gvRawData.Rows.Add()];

                        // Set field
                        row.Cells[COLUMN_NAME_KEY].Value = property.Key;
                        row.Cells[COLUMN_NAME_KEY].ToolTipText = string.Format("Name: {0}", property.Key);

                        // Set field value
                        row.Cells[COLUMN_NAME_VALUE].Value = property.Value;

                        // Update progress
                        current++;
                        ItemLoaded(null, new ItemLoadedEventArgs() { TotalItems = total, CurrentItem = current });
                    }

                    // Sort grid
                    gvRawData.Sort(columnKey, System.ComponentModel.ListSortDirection.Ascending);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        public static void LoadChanges(TreeNode selectedNode, DataGridView gvChanges)
        {
            try
            {
                ChangeQuery changeQuery = new ChangeQuery(false, true);
                ChangeCollection changes = null;

                if (selectedNode.Tag == null)
                    return;

                SPClient.Site site;
                SPClient.Web web;
                SPClient.List list;

                // Determine change query specific to selected node
                switch (selectedNode.Tag.GetClientType())
                {
                    case ClientType.Site:
                        site = selectedNode.Tag as SPClient.Site;
                        changeQuery.Site = true;
                        changes = site.GetChanges(changeQuery);
                        break;
                    case ClientType.Web:
                        site = GetClientContext(selectedNode).Site;
                        changeQuery.Web = true;
                        changes = site.GetChanges(changeQuery);
                        break;
                    case ClientType.List:
                        // TODO: Load RootFolderUrl, causing exceptions
                        web = selectedNode.Parent.Parent.Tag as SPClient.Web;
                        changeQuery.List = true;
                        changes = web.GetChanges(changeQuery);
                        break;
                    case ClientType.ListItem:
                        SPClient.ListItem item = selectedNode.Tag as SPClient.ListItem;
                        changeQuery.Item = true;
                        changes = item.ParentList.GetChanges(changeQuery);
                        break;
                    case ClientType.ContentType:
                        switch (selectedNode.Parent.Parent.Tag.GetClientType())
                        {
                            case ClientType.Site:
                                site = selectedNode.Parent.Parent.Tag as SPClient.Site;
                                changeQuery.ContentType = true;
                                changes = site.GetChanges(changeQuery);
                                break;
                            case ClientType.Web:
                                web = selectedNode.Parent.Parent.Tag as SPClient.Web;
                                changeQuery.ContentType = true;
                                changes = web.GetChanges(changeQuery);
                                break;
                            case ClientType.List:
                                list = selectedNode.Parent.Parent.Tag as SPClient.List;
                                changeQuery.ContentType = true;
                                changes = list.GetChanges(changeQuery);
                                break;
                            default:
                                break;
                        }
                        break;
                    case ClientType.Field:
                    case ClientType.FieldCalculated:
                    case ClientType.FieldChoice:
                    case ClientType.FieldComputed:
                    case ClientType.FieldDateTime:
                    case ClientType.FieldGuid:
                    case ClientType.FieldLookup:
                    case ClientType.FieldMultiChoice:
                    case ClientType.FieldMultiLineText:
                    case ClientType.FieldNumber:
                    case ClientType.FieldRatingScale:
                    case ClientType.FieldText:
                    case ClientType.FieldUrl:
                    case ClientType.FieldUser:
                    case ClientType.TaxonomyField:
                        switch (selectedNode.Parent.Parent.Tag.GetClientType())
                        {
                            case ClientType.Site:
                                site = selectedNode.Parent.Parent.Tag as SPClient.Site;
                                changeQuery.Field = true;
                                changes = site.GetChanges(changeQuery);
                                break;
                            case ClientType.Web:
                                web = selectedNode.Parent.Parent.Tag as SPClient.Web;
                                changeQuery.Field = true;
                                changes = web.GetChanges(changeQuery);
                                break;
                            case ClientType.List:
                                list = selectedNode.Parent.Parent.Tag as SPClient.List;
                                changeQuery.Field = true;
                                changes = list.GetChanges(changeQuery);
                                break;
                            default:
                                break;
                        }
                        break;
                    case ClientType.Group:
                        site = selectedNode.Parent.Parent.Parent.Tag as SPClient.Site;
                        changeQuery.Group = true;
                        changes = site.GetChanges(changeQuery);
                        break;
                    case ClientType.User:
                        site = selectedNode.Parent.Parent.Parent.Tag as SPClient.Site;
                        changeQuery.User = true;
                        changes = site.GetChanges(changeQuery);
                        break;
                    default:
                        break;
                }

                // Reset data source
                gvChanges.DataSource = null;

                // Retrieve changes and convert to specific change type
                if (changes != null)
                {
                    SPClient.ClientContext ctx = NodeLoader.GetClientContext(selectedNode);
                    IEnumerable<Change> results = ctx.LoadQuery(changes.IncludeWithDefaultProperties(
#if CLIENTSDKV161UP
                        c => c.RelativeTime,
#endif
                        c => c.ChangeToken));
                    ctx.ExecuteQuery();

                    switch (selectedNode.Tag.GetClientType())
                    {
                        case ClientType.Site:
                            gvChanges.DataSource = GetChangeList<ChangeSite>(results);
                            break;
                        case ClientType.Web:
                            gvChanges.DataSource = GetChangeList<ChangeWeb>(results).FindAll(c => c.WebId.Equals(((Web)selectedNode.Tag).Id));
                            break;
                        case ClientType.List:
                            gvChanges.DataSource = GetChangeList<ChangeList>(results);
                            break;
                        case ClientType.ListItem:
                            gvChanges.DataSource = GetChangeList<ChangeItem>(results).FindAll(c => c.ItemId.Equals(((ListItem)selectedNode.Tag).Id));
                            break;
                        case ClientType.ContentType:
                            gvChanges.DataSource = GetChangeList<ChangeContentType>(results).FindAll(c => c.ContentTypeId.StringValue.Equals(((ContentType)selectedNode.Tag).Id.StringValue));
                            break;
                        case ClientType.Field:
                        case ClientType.FieldCalculated:
                        case ClientType.FieldChoice:
                        case ClientType.FieldComputed:
                        case ClientType.FieldDateTime:
                        case ClientType.FieldGuid:
                        case ClientType.FieldLookup:
                        case ClientType.FieldMultiChoice:
                        case ClientType.FieldMultiLineText:
                        case ClientType.FieldNumber:
                        case ClientType.FieldRatingScale:
                        case ClientType.FieldText:
                        case ClientType.FieldUrl:
                        case ClientType.FieldUser:
                        case ClientType.TaxonomyField:
                            gvChanges.DataSource = GetChangeList<ChangeField>(results).FindAll(c => c.FieldId.Equals(((Field)selectedNode.Tag).Id));
                            break;
                        case ClientType.Group:
                            gvChanges.DataSource = GetChangeList<ChangeGroup>(results).FindAll(c => c.GroupId.Equals(((Group)selectedNode.Tag).Id));
                            break;
                        case ClientType.User:
                            gvChanges.DataSource = GetChangeList<ChangeUser>(results).FindAll(c => c.UserId.Equals(((User)selectedNode.Tag).Id));
                            break;
                        default:
                            break;
                    }

                    // Sort on time
                    //gvChanges.Sort(gvChanges.Columns["Time"], System.ComponentModel.ListSortDirection.Descending);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private static List<T> GetChangeList<T>(IEnumerable<Change> results)
        {
            List<T> changes = new List<T>();

            foreach (Change change in results)
            {
                changes.Add((T)(object)change);
            }

            return changes;
        }

        private static string GetFieldValue(SPClient.ListItem item, SPClient.Field field)
        {
            string value = string.Empty;
            object fieldValue = item.FieldValues[field.InternalName];

            // Validate item field value is not empty
            if (fieldValue == null)
                return string.Empty;

            // Get item field value based on field type
            switch (field.TypeAsString)
            {
                case "Lookup":
                    FieldLookupValue lookupValue = fieldValue as FieldLookupValue;
                    value = lookupValue.GetDisplayValue();
                    break;
                case "LookupMulti":
                    foreach (FieldLookupValue lookupItemValue in (FieldLookupValue[])fieldValue as FieldLookupValue[])
                    {
                        value += string.Format("{0} {1} ", lookupItemValue.GetDisplayValue(), DELIMITER);
                    }
                    break;
                case "URL":
                    FieldUrlValue urlValue = fieldValue as FieldUrlValue;
                    value = urlValue.GetDisplayValue();
                    break;
                case "User":
                    FieldUserValue userValue = fieldValue as FieldUserValue;
                    value = userValue.GetDisplayValue();
                    break;
                case "TaxonomyFieldType":
                    TaxonomyFieldValue taxonomyValue = fieldValue as TaxonomyFieldValue;
                    value = taxonomyValue.GetDisplayValue();
                    break;
                case "TaxonomyFieldTypeMulti":
                    foreach (TaxonomyFieldValue taxonomyItemValue in (TaxonomyFieldValueCollection)fieldValue as TaxonomyFieldValueCollection)
                    {
                        value += string.Format("{0} {1} ", taxonomyItemValue.GetDisplayValue(), DELIMITER);
                    }
                    break;
                default:
                    value = fieldValue.ToString();
                    break;
            }

            return value;
        }

        public static string LoadSchemaXML(TreeNode treeNode)
        {
            string xml = string.Empty;

            if (treeNode.Tag != null)
            {
                // Check general class types
                switch (treeNode.Tag.GetClientType())
                {
                    case ClientType.List:
                        xml = ((SPClient.List)treeNode.Tag).SchemaXml;
                        break;
                    case ClientType.View:
                        xml = ((SPClient.View)treeNode.Tag).ListViewXml;
                        break;
                    case ClientType.ContentType:
                        xml = ((SPClient.ContentType)treeNode.Tag).SchemaXml;
                        break;
                    case ClientType.WorkflowDefinition:
                        xml = ((SPClient.WorkflowServices.WorkflowDefinition)treeNode.Tag).Xaml;
                        break;
                    case ClientType.Field:
                    case ClientType.FieldCalculated:
                    case ClientType.FieldChoice:
                    case ClientType.FieldComputed:
                    case ClientType.FieldDateTime:
                    case ClientType.FieldGuid:
                    case ClientType.FieldLookup:
                    case ClientType.FieldMultiChoice:
                    case ClientType.FieldMultiLineText:
                    case ClientType.FieldNumber:
                    case ClientType.FieldRatingScale:
                    case ClientType.FieldText:
                    case ClientType.FieldUrl:
                    case ClientType.FieldUser:
                    case ClientType.TaxonomyField:
                        xml = ((SPClient.Field)treeNode.Tag).SchemaXml;
                        break;
                    default:
                        break;
                }
            }

            // Format plain text to XML format
            if (!string.IsNullOrEmpty(xml))
                xml = System.Xml.Linq.XElement.Parse(xml).ToString();

            return xml;
        }

        public static Uri LoadRestQuery(TreeNode treeNode)
        {
            Uri url = null;

            if (treeNode.Tag != null)
            {
                // Check general class types
                switch (treeNode.Tag.GetClientType())
                {
                    case ClientType.Site:
                        url = ((SPClient.Site)treeNode.Tag).GetRestUrl();
                        break;
                    case ClientType.Web:
                        url = ((SPClient.Web)treeNode.Tag).GetRestUrl();
                        break;
                    case ClientType.List:
                        url = ((SPClient.List)treeNode.Tag).GetRestUrl();
                        break;
                    case ClientType.Field:
                    case ClientType.FieldCalculated:
                    case ClientType.FieldChoice:
                    case ClientType.FieldComputed:
                    case ClientType.FieldDateTime:
                    case ClientType.FieldGuid:
                    case ClientType.FieldLookup:
                    case ClientType.FieldMultiChoice:
                    case ClientType.FieldMultiLineText:
                    case ClientType.FieldNumber:
                    case ClientType.FieldRatingScale:
                    case ClientType.FieldText:
                    case ClientType.FieldUrl:
                    case ClientType.FieldUser:
                    case ClientType.TaxonomyField:
                        url = ((SPClient.Field)treeNode.Tag).GetRestUrl(treeNode);
                        break;
                    default:
                        break;
                }
            }

            return url;
        }
    }

    public class ItemLoadedEventArgs : EventArgs
    {
        public int TotalItems { get; set; }
        public int CurrentItem { get; set; }
    }

    public class BatchCompletedEventArgs : EventArgs
    {
        public int CurrentItem { get; set; }
        public int TotalItems { get; set; }
        public int BatchSize { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
