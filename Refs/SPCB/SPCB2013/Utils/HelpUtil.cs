using SPBrowser.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPBrowser.Utils
{
    /// <summary>
    /// Utility class for MSDN and/or SharePoint Help information.
    /// </summary>
    public static class HelpUtil
    {
        private const string MSDN_HELP_LINK_2010 = "http://msdn.microsoft.com/en-us/library/{0}(v=office.14).aspx";
        private const string MSDN_HELP_LINK_2013 = "http://msdn.microsoft.com/en-us/library/{0}(v=office.15).aspx";
        private const string MSDN_HELP_LINK_2016 = "http://msdn.microsoft.com/en-us/library/{0}(v=office.16).aspx";

        private static List<Feature> _features;

        static HelpUtil()
        {
            InitFeaturesSharePoint2013();
        }

        /// <summary>
        /// Gets the help link for current product version.
        /// </summary>
        /// <returns></returns>
        private static string GetHelpLink()
        {
            string url = string.Empty;

#if CLIENTSDKV150
            url = MSDN_HELP_LINK_2013;
#elif CLIENTSDKV160UP
            url = MSDN_HELP_LINK_2016;
#endif
            return url;
        }

        /// <summary>
        /// Gets the "Microsoft.SharePoint.Client" namespace MSDN help link.
        /// </summary>
        /// <returns>Returns the link to MSDN for "Microsoft.SharePoint.Client" namespace.</returns>
        public static Uri GetMSDNHelpLink()
        {
            return new Uri(string.Format(GetHelpLink(), "Microsoft.SharePoint.Client"));
        }

        /// <summary>
        /// Gets the MSDN help link based on the provided node.
        /// </summary>
        /// <param name="node"><see cref="TreeNode"/> which contains the object to show MSDN help for.</param>
        /// <returns>Returns the link to MSDN for "Microsoft.SharePoint.Client" namespace.</returns>
        public static Uri GetMSDNHelpLink(System.Windows.Forms.TreeNode node)
        {
            Uri helpLink = null;

            if (node != null && node.Tag != null && !(node.Tag is SPBrowser.NodeLoadType))
            {
                helpLink = new Uri(string.Format(GetHelpLink().ToString(), node.Tag.GetType().ToString()));
            }

            return helpLink;
        }

        /// <summary>
        /// Gets the SharePoint Client Components SDK download title.
        /// </summary>
        /// <returns></returns>
        public static string GetSDKDownloadTitle()
        {
            string title = string.Empty;

#if CLIENTSDKV150
            title = Constants.DOWNLOAD_SP2013_SDK_TITLE;
#elif CLIENTSDKV160
            title = Constants.DOWNLOAD_SP2016_SDK_TITLE;
#elif CLIENTSDKV161
            title = Constants.DOWNLOAD_SPONLINE_SDK_TITLE;
#endif

            return title;
        }

        /// <summary>
        /// Gets the SDK major version.
        /// </summary>
        /// <returns></returns>
        public static int GetSDKMajorVersion()
        {
            int version = 0;

#if CLIENTSDKV150
            version = 15;
#elif CLIENTSDKV160
            version = 16;
#elif CLIENTSDKV161
            version = 16;
#endif

            return version;
        }

        /// <summary>
        /// Gets the SharePoint Client Components SDK download URL.
        /// </summary>
        /// <returns></returns>
        public static string GetSDKDownloadUrl()
        {
            string url = string.Empty;

#if CLIENTSDKV150
            url = Constants.DOWNLOAD_SP2013_SDK_URL;
#elif CLIENTSDKV160
            url = Constants.DOWNLOAD_SP2016_SDK_URL;
#elif CLIENTSDKV161
            url = Constants.DOWNLOAD_SPONLINE_SDK_URL;
#endif

            return url;
        }

        /// <summary>
        /// Gets a <see cref="Feature"/> object with detailed information.
        /// </summary>
        /// <param name="featureId">The feature identifier in <see cref="Guid"/> format.</param>
        /// <returns>Returns <see cref="Feature"/> object or null when it does not exist.</returns>
        public static Feature GetFeature(Guid featureId)
        {
            return GetFeature(featureId.ToString());
        }

        /// <summary>
        /// Gets a <see cref="Feature"/> object with detailed information.
        /// </summary>
        /// <param name="featureId">The feature identifier in <see cref="Guid"/> format.</param>
        /// <returns>Returns <see cref="Feature"/> object or null when it does not exist.</returns>
        public static Feature GetFeature(string featureId)
        {
            // Try to retrieve from internal feature collection
            Feature feature = _features.FirstOrDefault(f => f.Id.ToString().Equals(featureId, StringComparison.InvariantCultureIgnoreCase));

            // If not found internally, then try retrieving it from the custom feature definition source.
            if (feature == null)
                feature = Globals.CustomFeatureDefinitions.FirstOrDefault(f => f.Id.ToString().Equals(featureId, StringComparison.InvariantCultureIgnoreCase));

            return feature;
        }

        /// <summary>
        /// All out-of-the-box SharePoint 2013 features and IDs.
        /// </summary>
        /// <remarks>
        /// Check-out the <seealso cref="https://msdn.microsoft.com/en-us/library/aa369771(v=vs.85).aspx"/> link for information regarding localization data.
        /// </remarks>
        /// <seealso cref="https://msdn.microsoft.com/en-us/library/aa369771(v=vs.85).aspx"/>
        /// <seealso cref="https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.splocale.lcid.aspx"/>
        private static void InitFeaturesSharePoint2013()
        {
            _features = new List<Feature>();
            _features.Add(new Feature() { Id = new Guid("c6a92dbf-6441-4b8b-882f-8d97cb12c83a"), DisplayName = "Reports", InternalName = "AbuseReportsList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("a0f12ee4-9b60-4ba4-81f6-75724f4ca973"), DisplayName = "Access Requests List", InternalName = "AccessRequests", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("1cc4b32c-299b-41aa-9770-67715ea05f25"), DisplayName = "Access Services 2010 Farm Feature", InternalName = "AccSrvApplication", Hidden = false, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("29ea7495-fca1-4dc6-8ac1-500c247a036e"), DisplayName = "Access Services System Objects", InternalName = "AccSrvMSysAso", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("a4d4ee2c-a6cb-4191-ab0a-21bb5bde92fb"), DisplayName = "Access Services Restricted List Definition", InternalName = "AccSrvRestrictedList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("bcf89eb7-bca1-4468-bdb4-ca27f61a2292"), DisplayName = "Access Services Shell", InternalName = "AccSrvShell", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("744b5fd3-3b09-4da6-9bd1-de18315b045d"), DisplayName = "Access Services Solution Gallery", InternalName = "AccSrvSolutionGallery", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("d5ff2d2c-8571-4c3c-87bc-779111979811"), DisplayName = "Access Services Solution Gallery Feature Stapler", InternalName = "AccSrvSolutionGalleryStapler", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("1a8251a0-47ab-453d-95d4-07d7ca4f8166"), DisplayName = "Access Services User Templates", InternalName = "AccSrvUserTemplate", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("28101b19-b896-44f4-9264-db028f307a62"), DisplayName = "Access Services User Application Log", InternalName = "AccSrvUSysAppLog", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("d2b9ec23-526b-42c5-87b6-852bd83e0364"), DisplayName = "Access App", InternalName = "AccSvcAddAccessApp", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("3d7415e4-61ba-4669-8d78-213d374d9825"), DisplayName = "Access Service Add Access Application Feature Stapler", InternalName = "AccSvcAddAccessAppStapling", Hidden = false, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("5094e988-524b-446c-b2f6-040b5be46297"), DisplayName = "Access Services Farm Feature", InternalName = "AccSvcApplication", Hidden = false, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("7ffd6d57-4b10-4edb-ac26-c2cfbf8173ab"), DisplayName = "Access Services Shell", InternalName = "AccSvcShell", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("d250636f-0a26-4019-8425-a5232d592c09"), DisplayName = "Add Dashboard", InternalName = "AddDashboard", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("fead7313-ae6d-45dd-8260-13b563cb4c71"), DisplayName = "Central Administration Links", InternalName = "AdminLinks", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("b8f36433-367d-49f3-ae11-f7d76b51d251"), DisplayName = "Administrative Reporting Infrastructure", InternalName = "AdminReportCore", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("55312854-855b-4088-b09d-c5efe0fbf9d2"), DisplayName = "Administrative Reporting Core Pushdown Feature", InternalName = "AdminReportCorePushdown", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("00bfea71-d1ce-42de-9c63-a44004ce0104"), DisplayName = "Announcements Lists", InternalName = "AnnouncementsList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("23330bdb-b83e-4e09-8770-8155aa5e87fd"), DisplayName = "", InternalName = "AppLockdown", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("fdc6383e-3f1d-4599-8b7c-c515e99cbf18"), DisplayName = "Allow registration of SharePoint apps", InternalName = "AppRegistration", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("334dfc83-8655-48a1-b79d-68b7f6c63222"), DisplayName = "App Requests List", InternalName = "AppRequestsList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("4bcccd62-dcaf-46dc-a7d4-e38277ef33f4"), DisplayName = "Asset Library", InternalName = "AssetLibrary", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("fa7cefd8-5595-4d68-84fa-fe2d9e693de7"), DisplayName = "Autohosted App Licensing Feature", InternalName = "AutohostedAppLicensing", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("013a0db9-1607-4c42-8f71-08d821d395c2"), DisplayName = "Autohosted App Licensing Feature Stapling", InternalName = "AutohostedAppLicensingStapling", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("b21b090c-c796-4b0f-ac0f-7ef1659c20ae"), DisplayName = "SharePoint Server Standard Site Collection features", InternalName = "BaseSite", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("97a2485f-ef4b-401f-9167-fa4fe177c6f6"), DisplayName = "Base Site Features Stapling", InternalName = "BaseSiteStapling", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("99fe402e-89a0-45aa-9163-85342e865dc8"), DisplayName = "SharePoint Server Standard Site features", InternalName = "BaseWeb", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("4f56f9fa-51a0-420c-b707-63ecbb494db1"), DisplayName = "SharePoint Server Standard Web application features", InternalName = "BaseWebApplication", Hidden = false, IsCustomDefinition = false, Scope = Scope.WebApplication });
            _features.Add(new Feature() { Id = new Guid("00bfea71-1c5e-4a24-b310-ba51c3eb7a57"), DisplayName = "Basic Web Parts", InternalName = "BasicWebParts", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("60c8481d-4b54-4853-ab9f-ed7e1c21d7e4"), DisplayName = "External System Events Activator", InternalName = "BcsEvents", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("3f59333f-4ce1-406d-8a97-9ecb0ff0337f"), DisplayName = "Document Center Enhancements", InternalName = "BDR", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("f979e4dc-1852-4f26-ab92-d1b2a190afc9"), DisplayName = "Dashboards Library", InternalName = "BICenterDashboardsLib", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("3d8210e9-1e89-4f12-98ef-643995339ed4"), DisplayName = "BICenter Data Connections Feature", InternalName = "BICenterDataConnections", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("26676156-91a0-49f7-87aa-37b1d5f0c4d0"), DisplayName = "DataConnections Library for PerformancePoint", InternalName = "BICenterDataconnectionsLib", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("a64c4402-7037-4476-a290-84cfd56ca01d"), DisplayName = "BICenter Data Connections List Instance Feature", InternalName = "BICenterDataConnectionsListInstance", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("3a027b18-36e4-4005-9473-dd73e6756a73"), DisplayName = "BICenter Feature Stapler Feature", InternalName = "BICenterFeatureStapler", Hidden = true, IsCustomDefinition = false, Scope = Scope.WebApplication });
            _features.Add(new Feature() { Id = new Guid("a354e6b3-6015-4744-bdc2-2fc1e4769e65"), DisplayName = "BICenter PPS Content Pages Feature", InternalName = "BICenterPPSContentPages", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("faf31b50-880a-4e4f-a21b-597f6b4d6478"), DisplayName = "BICenter PPS Content Navigation Bar Feature", InternalName = "BICenterPPSNavigationLink", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("f9c216ad-35c7-4538-abb8-8ec631a5dff7"), DisplayName = "BICenter PPS Workspace List Instance Feature", InternalName = "BICenterPPSWorkspaceListInstance", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("3992d4ab-fa9e-4791-9158-5ee32178e88a"), DisplayName = "Business Intelligence center sample data", InternalName = "BICenterSampleData", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("43f41342-1a37-4372-8ca0-b44d881e4434"), DisplayName = "SharePoint Portal Server Business Appications Content Type Definition", InternalName = "BizAppsCTypes", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("5a979115-6b71-45a5-9881-cdc872051a69"), DisplayName = "SPS Biz Apps Field Definition", InternalName = "BizAppsFields", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("065c78be-5231-477e-a972-14177cc5b3c7"), DisplayName = "SharePoint Portal Server Status Indicator List template", InternalName = "BizAppsListTemplates", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("4248e21f-a816-4c88-8cab-79d82201da7b"), DisplayName = "BizApps Site Templates", InternalName = "BizAppsSiteTemplates", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("0d1c50f7-0309-431c-adfb-b777d5473a65"), DisplayName = "$Resources:core,blogContentFeatureTitle;", InternalName = "BlogContent", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("e4639bb7-6e95-4e2f-b562-03b832dd4793"), DisplayName = "Blog Home Page", InternalName = "BlogHomePage", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("faf00902-6bab-4583-bd02-84db191801d8"), DisplayName = "Blog Site Feature", InternalName = "BlogSiteTemplate", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("aeef8777-70c0-429f-8a13-f12db47a6d47"), DisplayName = "Bulk workflow process button", InternalName = "BulkWorkflow", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("d992aeca-3802-483a-ab40-6c9376300b61"), DisplayName = "Bulk Workflow Timer Job", InternalName = "BulkWorkflowTimerJob", Hidden = true, IsCustomDefinition = false, Scope = Scope.WebApplication });
            _features.Add(new Feature() { Id = new Guid("239650e3-ee0b-44a0-a22a-48292402b8d8"), DisplayName = "Phone Call Memo List", InternalName = "CallTrackList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("d32700c7-9ec5-45e6-9c89-ea703efca1df"), DisplayName = "Categories List Feature", InternalName = "CategoriesList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("a568770a-50ba-4052-ab48-37d8029b3f47"), DisplayName = "Circulation List", InternalName = "CirculationList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("1fce0577-1f58-4fc2-a996-6c4bcf59eeed"), DisplayName = "Content Management Interoperability Services (CMIS) Producer", InternalName = "CmisProducer", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("502a2d54-6102-4757-aaa0-a90586106368"), DisplayName = "Site Mailbox", InternalName = "CollaborationMailbox", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("3a11d8ef-641e-4c79-b4d9-be3b17f9607c"), DisplayName = "Site Mailboxes", InternalName = "CollaborationMailboxFarm", Hidden = false, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("2b03956c-9271-4d1c-868a-07df2971ed30"), DisplayName = "Community Portal Feature", InternalName = "CommunityPortal", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("961d6a9c-4388-4cf2-9733-38ee8c89afd4"), DisplayName = "Community Site Feature", InternalName = "CommunitySite", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("00bfea71-7e6d-4186-9ba8-c047ac750105"), DisplayName = "Contacts Lists", InternalName = "ContactsList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("cd1a49b0-c067-4fdd-adfe-69e6f5022c1a"), DisplayName = "Content Deployment Source Feature", InternalName = "ContentDeploymentSource", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("7890e045-6c96-48d8-96e7-6a1d63737d71"), DisplayName = "Content Following", InternalName = "ContentFollowing", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a34e5458-8d20-4c0d-b137-e1390f5824a1"), DisplayName = "Content Following List", InternalName = "ContentFollowingList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("e1580c3c-c510-453b-be15-35feb0ddb1a5"), DisplayName = "Content Following Stapling", InternalName = "ContentFollowingStapling", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("0f121a23-c6bc-400f-87e4-e6bbddf6916d"), DisplayName = "Standard User Interface Items", InternalName = "ContentLightup", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("9a447926-5937-44cb-857a-d3829301c73b"), DisplayName = "Content Type Syndication Hub", InternalName = "ContentTypeHub", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("dd903064-c9d8-4718-b4e7-8ab9bd039fff"), DisplayName = "Content type publishing", InternalName = "ContentTypePublish", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("fead7313-4b9e-4632-80a2-ff00a2d83297"), DisplayName = "Standard Content Type Settings Links", InternalName = "ContentTypeSettings", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("34339dc9-dec4-4256-b44a-b30ff2991a64"), DisplayName = "Content type syndication", InternalName = "ContentTypeSyndication", Hidden = true, IsCustomDefinition = false, Scope = Scope.WebApplication });
            _features.Add(new Feature() { Id = new Guid("0ac11793-9c2f-4cac-8f22-33f93fac18f2"), DisplayName = "App Catalog", InternalName = "CorporateCatalog", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("f8bea737-255e-4758-ab82-e34bb46f5828"), DisplayName = "App Catalog Settings", InternalName = "CorporateCuratedGallerySettings", Hidden = true, IsCustomDefinition = false, Scope = Scope.WebApplication });
            _features.Add(new Feature() { Id = new Guid("a5aedf1a-12e5-46b4-8348-544386d5312d"), DisplayName = "Cross-Farm Site Permissions", InternalName = "CrossFarmSitePermissions", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("151d22d9-95a8-4904-a0a3-22e4db85d1e0"), DisplayName = "Cross-Site Collection Publishing", InternalName = "CrossSiteCollectionPublishing", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("695b6570-a48b-4a8e-8ea5-26ea7fc1d162"), DisplayName = "Standard Content Type Definitions", InternalName = "CTypes", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("00bfea71-de22-43b2-a848-c05709900100"), DisplayName = "Custom Lists", InternalName = "CustomList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("00bfea71-dbd7-4f72-b8cb-da7ac0440130"), DisplayName = "Data Connections Feature", InternalName = "DataConnectionLibrary", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("cdfa39c6-6413-4508-bccf-bf30368472b3"), DisplayName = "Data Connection Library", InternalName = "DataConnectionLibraryStapling", Hidden = false, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("00bfea71-f381-423d-b9d1-da7a54c50110"), DisplayName = "Data Source Libraries", InternalName = "DataSourceLibrary", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("ca2543e6-29a1-40c1-bba9-bd8510a4c17b"), DisplayName = "Content Deployment", InternalName = "DeploymentLinks", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("e374875e-06b6-11e0-b0fa-57f5dfd72085"), DisplayName = "Developer Feature", InternalName = "Developer", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("00bfea71-6a49-43fa-b535-d15c05500108"), DisplayName = "Discussion Lists", InternalName = "DiscussionsList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("1ec2c859-e9cb-4d79-9b2b-ea8df09ede22"), DisplayName = "DM Content Type Setting Links", InternalName = "DMContentTypeSettings", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("b50e3104-6812-424f-a011-cc90e6327318"), DisplayName = "Document ID Service", InternalName = "DocId", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("184c82e7-7eb1-4384-8e8c-62720ef397a0"), DisplayName = "Academic Library Site", InternalName = "docmarketplace", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("5690f1a0-22b6-4262-b1c2-74f505bc0670"), DisplayName = "Academic Library Site Safe Controls.", InternalName = "docmarketplacesafecontrols", Hidden = true, IsCustomDefinition = false, Scope = Scope.WebApplication });
            _features.Add(new Feature() { Id = new Guid("1dfd85c5-feff-489f-a71f-9322f8b13902"), DisplayName = "Academic Library Site Sample Data", InternalName = "docmarketplacesampledata", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("00bfea71-e717-4e80-aa17-d0c71b360101"), DisplayName = "Document Libraries", InternalName = "DocumentLibrary", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("3a4ce811-6fe0-4e97-a6ae-675470282cf2"), DisplayName = "Document Sets metadata synchronization", InternalName = "DocumentManagement", Hidden = false, IsCustomDefinition = false, Scope = Scope.WebApplication });
            _features.Add(new Feature() { Id = new Guid("7ad5272a-2694-4349-953e-ea5ef290e97c"), DisplayName = "Content Organizer", InternalName = "DocumentRouting", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("0c8a9a47-22a9-4798-82f1-00e62a96006e"), DisplayName = "Document Routing Resources", InternalName = "DocumentRoutingResources", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bae86a2-776d-499d-9db8-fa4cdc7884f8"), DisplayName = "Document Sets", InternalName = "DocumentSet", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a140a1ac-e757-465d-94d4-2ca25ab2c662"), DisplayName = "Office.com Entry Points from SharePoint", InternalName = "DownloadFromOfficeDotCom", Hidden = false, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("142ae5f3-6796-45c5-b31d-2e62e8868b53"), DisplayName = "Group Approval", InternalName = "EawfSite", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("1ba1b299-716c-4ee1-9c23-e8bbab3c812a"), DisplayName = "Group Approval", InternalName = "EawfWeb", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("e8c02a2a-9010-4f98-af88-6668d59f91a7"), DisplayName = "eDiscovery Case Feature", InternalName = "EDiscoveryCaseResources", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("250042b9-1aad-4b56-a8a6-69d9fe1c8c2c"), DisplayName = "eDiscovery Portal", InternalName = "EDiscoveryConsole", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("03509cfb-8b2f-4f46-a4c9-8316d1e62a4b"), DisplayName = "Class Central Administration Links", InternalName = "EduAdminLinks", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("c1b78fe6-9110-42e8-87cb-5bd1c8ab278a"), DisplayName = "Class Administration Pages", InternalName = "EduAdminPages", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("bf76fc2c-e6c9-11df-b52f-cb00e0d72085"), DisplayName = "Class Community Web Types", InternalName = "EduCommunity", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("739ec067-2b57-463e-a986-354be77bb828"), DisplayName = "Class Community Site Actions", InternalName = "EduCommunityCustomSiteActions", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("2e030413-c4ff-41a4-8ee0-f6688950b34a"), DisplayName = "Community Lists", InternalName = "EduCommunitySite", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a16e895c-e61a-11df-8f6e-103edfd72085"), DisplayName = "Class Web Types", InternalName = "EduCourseCommunity", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("824a259f-2cce-4006-96cd-20c806ee9cfd"), DisplayName = "Class Lists", InternalName = "EduCourseCommunitySite", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("5025492c-dae2-4c00-8f34-cd08f7c7c294"), DisplayName = "Class and My Site Content", InternalName = "EduDashboard", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("cb869762-c694-439e-8d05-cf5ca066f271"), DisplayName = "Class Web Application Configurations", InternalName = "EduFarmWebApplication", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("41bfb21c-0447-4c97-bc62-0b07bec262a1"), DisplayName = "Bulk Request and Response List", InternalName = "EduInstitutionAdmin", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("978513c0-1e6c-4efb-b12e-7698963bfd05"), DisplayName = "Class Administration Lists", InternalName = "EduInstitutionSiteCollection", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("bd012a1f-c69b-4a13-b6a4-f8bc3e59760e"), DisplayName = "Class Membership Page", InternalName = "EduMembershipUI", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("abf1a85c-e91a-11df-bf2e-f7acdfd72085"), DisplayName = "Class My Site Web Types", InternalName = "EduMySiteCommunity", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("932f5bb1-e815-4c14-8917-c2bae32f70fe"), DisplayName = "Class My Site Host Content", InternalName = "EduMySiteHost", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("8d75610e-5ff9-4cd1-aefc-8b926f2af771"), DisplayName = "Class Search Display Template", InternalName = "EduSearchDisplayTemplates", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("08585e12-4762-4cc9-842a-a8d7b074bdb7"), DisplayName = "Class Shared Content", InternalName = "EduShared", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a46935c3-545f-4c15-a2fd-3a19b62d8a02"), DisplayName = "Study Group Web Content", InternalName = "EduStudyGroupCommunity", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("7f52c29e-736d-11e0-80b8-9edd4724019b"), DisplayName = "Class User Cache", InternalName = "EduUserCache", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("7de489aa-2e4a-46ff-88f0-d1b5a9d43709"), DisplayName = "Class Web Application", InternalName = "EduWebApplication", Hidden = true, IsCustomDefinition = false, Scope = Scope.WebApplication });
            _features.Add(new Feature() { Id = new Guid("d44a1358-e800-47e8-8180-adf2d0f77543"), DisplayName = "E-mail Integration with Content Organizer", InternalName = "EMailRouting", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("397942ec-14bf-490e-a983-95b87d0d29d1"), DisplayName = "Email Templates feature", InternalName = "EmailTemplates", Hidden = true, IsCustomDefinition = false, Scope = Scope.WebApplication });
            _features.Add(new Feature() { Id = new Guid("ae3a1339-61f5-4f8f-81a7-abd2da956a7d"), DisplayName = "Enable sideloading of Apps for Office and SharePoint", InternalName = "EnableAppSideLoading", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("81ebc0d6-8fb2-4e3f-b2f8-062640037398"), DisplayName = "Enhanced Html Editing", InternalName = "EnhancedHtmlEditing", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("068bc832-4951-11dc-8314-0800200c9a66"), DisplayName = "Enhanced Theming", InternalName = "EnhancedTheming", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("76d688ad-c16e-4cec-9b71-7b7f0d79b9cd"), DisplayName = "Enterprise Wiki", InternalName = "EnterpriseWiki", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("a942a218-fa43-4d11-9d85-c01e3e3a37cb"), DisplayName = "Enterprise Wiki Layouts", InternalName = "EnterpriseWikiLayouts", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("00bfea71-ec85-4903-972d-ebe475780106"), DisplayName = "Events Lists", InternalName = "EventsList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("e4e6a041-bc5b-45cb-beab-885a27079f74"), DisplayName = "Excel Services Application View Farm Feature", InternalName = "ExcelServer", Hidden = false, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("b3da33d0-5e51-4694-99ce-705a3ac80dc5"), DisplayName = "Deprecated Office Web Apps", InternalName = "ExcelServerEdit", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3cb475e7-4e87-45eb-a1f3-db96ad7cf313"), DisplayName = "Excel Services Application View Site Feature", InternalName = "ExcelServerSite", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("4c42ab64-55af-4c7c-986a-ac216a6e0c0e"), DisplayName = "Excel Services Application Web Part Site Feature", InternalName = "ExcelServerWebPart", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("c6ac73de-1936-47a4-bdff-19a6fc3ba490"), DisplayName = "Excel Services Application Web Part Farm Feature", InternalName = "ExcelServerWebPartStapler", Hidden = false, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("5f68444a-0131-4bb0-b013-454d925681a2"), DisplayName = "Farm Level Exchange Tasks Sync", InternalName = "ExchangeSync", Hidden = false, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("7cd95467-1777-4b6b-903e-89e253edc1f7"), DisplayName = "Site Subscription Level Exchange Tasks Sync", InternalName = "ExchangeSyncSiteSubscription", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("c85e5759-f323-4efb-b548-443d2216efb5"), DisplayName = "Disposition Approval Workflow", InternalName = "ExpirationWorkflow", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("00bfea71-9549-43f8-b978-e47e54a10600"), DisplayName = "External Lists", InternalName = "ExternalList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("5b10d113-2d0d-43bd-a2fd-f8bc879f5abd"), DisplayName = "External System Events", InternalName = "ExternalSubscription", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("58160a6b-4396-4d6e-867c-65381fb5fbc9"), DisplayName = "Resources List", InternalName = "FacilityList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("38969baa-3590-4635-81a4-2049d982adfa"), DisplayName = "FAST Search Central Admin Help Collection", InternalName = "FastCentralAdminHelpCollection", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("6e8f2b8d-d765-4e69-84ea-5702574c11d6"), DisplayName = "FAST Search End User Help Collection", InternalName = "FastEndUserHelpCollection", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("d2d98dc8-c7e9-46ec-80a5-b38f039c16be"), DisplayName = "ESS_SHORT_NAME_RTM Master Job Provisioning", InternalName = "FastFarmFeatureActivation", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("08386d3d-7cc0-486b-a730-3b4cfe1b5509"), DisplayName = "Manage Resources", InternalName = "FCGroupsList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("0125140f-7123-4657-b70a-db9aa1f209e5"), DisplayName = "Feature Pushdown Links", InternalName = "FeaturePushdown", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("ca7bd552-10b1-4563-85b9-5ed1d39c962a"), DisplayName = "Standard Column Definitions", InternalName = "Fields", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a7a2793e-67cd-4dc1-9fd0-43f61581207a"), DisplayName = "Following Content", InternalName = "FollowingContent", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("00bfea71-513d-4ca0-96c2-6a47775c0119"), DisplayName = "Gantt Chart Tasks Lists", InternalName = "GanttTasksList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("6e8a2add-ed09-4592-978e-8fa71e6f117c"), DisplayName = "Group Work Provisioning", InternalName = "GBWProvision", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("3d25bd73-7cd4-4425-b8fb-8899977f73de"), DisplayName = "GroupBoardWebParts", InternalName = "GBWWebParts", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("4aec7207-0d02-4f4f-aa07-b370199cd0c7"), DisplayName = "Getting Started", InternalName = "GettingStarted", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("4ddc5942-98b0-4d70-9f7f-17acfec010e5"), DisplayName = "Getting started with your app catalog site", InternalName = "GettingStartedWithAppCatalogSite", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("319d8f70-eb3a-4b44-9c79-2087a87799d6"), DisplayName = "Global Web Parts", InternalName = "GlobalWebParts", Hidden = false, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("00bfea71-3a1d-41d3-a0ee-651d11570120"), DisplayName = "Grid Lists", InternalName = "GridList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("9c03e124-eef7-4dc6-b5eb-86ccd207cb87"), DisplayName = "Group Work Lists", InternalName = "GroupWork", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("071de60d-4b02-4076-b001-b456e93146fe"), DisplayName = "Help", InternalName = "HelpLibrary", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("f9ce21f8-f437-4f7e-8bc6-946378c850f0"), DisplayName = "Hierarchy Tasks Lists", InternalName = "HierarchyTasksList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("9e56487c-795a-4077-9425-54a1ecb84282"), DisplayName = "Hold", InternalName = "Hold", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("9ad4c2d4-443b-4a94-8534-49a23f20ba3c"), DisplayName = "Holidays List", InternalName = "HolidaysList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("a4c654e4-a8da-4db3-897c-a386048f7157"), DisplayName = "Html Design", InternalName = "HtmlDesign", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("7877bbf6-30f5-4f58-99d9-a0cc787c1300"), DisplayName = "Apps that require accessible internet facing endpoints", InternalName = "IfeDependentApps", Hidden = false, IsCustomDefinition = false, Scope = Scope.WebApplication });
            _features.Add(new Feature() { Id = new Guid("1c6a572c-1b58-49ab-b5db-75caf50692e6"), DisplayName = "Microsoft IME Dictionary List", InternalName = "IMEDicList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("da2e115b-07e4-49d9-bb2c-35e93bb9fca9"), DisplayName = "In Place Records Management", InternalName = "InPlaceRecords", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a10b6aa4-135d-4598-88d1-8d4ff5691d13"), DisplayName = "Admin Links for InfoPath Forms Services.", InternalName = "ipfsAdminLinks", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("750b8e49-5213-4816-9fa2-082900c0201a"), DisplayName = "Admin Links for InfoPath Forms Services.", InternalName = "IPFSAdminWeb", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("c88c4ff1-dbf5-4649-ad9f-c6c426ebcbf5"), DisplayName = "InfoPath Forms Services support", InternalName = "IPFSSiteFeatures", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("15845762-4ec4-4606-8993-1c0512a98680"), DisplayName = "InfoPath Forms Services Tenant Administration", InternalName = "IPFSTenantFormsConfig", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("3c577815-7658-4d4f-a347-cfbb370700a7"), DisplayName = "InfoPath Forms Services Web Service Proxy Administration", InternalName = "IPFSTenantWebProxyConfig", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("a0e5a010-1329-49d4-9e09-f280cdbed37d"), DisplayName = "InfoPath Forms Services support", InternalName = "IPFSWebFeatures", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("00bfea71-5932-4f9c-ad71-1557e5751100"), DisplayName = "Issues Lists", InternalName = "IssuesList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("fde5d850-671e-4143-950a-87b473922dc7"), DisplayName = "Three-state workflow", InternalName = "IssueTrackingWorkflow", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("39d18bbf-6e0f-4321-8f16-4e3b51212393"), DisplayName = "Item Form Recommendations", InternalName = "ItemFormRecommendations", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("6e53dd27-98f2-4ae5-85a0-e9a8ef4aa6df"), DisplayName = "Document Libraries", InternalName = "LegacyDocumentLibrary", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("c845ed8d-9ce5-448c-bd3e-ea71350ce45b"), DisplayName = "SharePoint 2007 Workflows", InternalName = "LegacyWorkflows", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("00bfea71-2062-426c-90bf-714c59600103"), DisplayName = "Links Lists", InternalName = "LinksList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("fc33ba3b-7919-4d7e-b791-c6aeccf8f851"), DisplayName = "List Content Targeting", InternalName = "ListTargeting", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("14aafd3a-fcb9-4bb7-9ad7-d8e36b663bbd"), DisplayName = "SharePoint Portal Server Local Site Directory Capture Control", InternalName = "LocalSiteDirectoryControl", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("8f15b342-80b1-4508-8641-0751e2b55ca6"), DisplayName = "Local Site Directory MetaData Capture Feature", InternalName = "LocalSiteDirectoryMetaData", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("e978b1a6-8de7-49d0-8600-09a250354e14"), DisplayName = "Site Settings Link to Local Site Directory Settings page.", InternalName = "LocalSiteDirectorySettingsLink", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("063c26fa-3ccc-4180-8a84-b6f98e991df3"), DisplayName = "Library and Folder Based Retention", InternalName = "LocationBasedPolicy", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("8c6f9096-388d-4eed-96ff-698b3ec46fc4"), DisplayName = "Maintenance Log Library", InternalName = "MaintenanceLogs", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("c59dbaa9-fa01-495d-aaa3-3c02cc2ee8ff"), DisplayName = "Manage Profile Service Application", InternalName = "ManageUserProfileServiceApplication", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("8a663fe0-9d9c-45c7-8297-66365ad50427"), DisplayName = "SharePoint Portal Server Master Site Directory Capture Control", InternalName = "MasterSiteDirectoryControl", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("d95c97f3-e528-4da2-ae9f-32b3535fbb59"), DisplayName = "Mobile Browser View", InternalName = "MBrowserRedirect", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("2dd8788b-0e6b-4893-b4c0-73523ac261b1"), DisplayName = "Mobile Browser View feature stapling", InternalName = "MBrowserRedirectStapling", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("87294c72-f260-42f3-a41b-981a2ffce37a"), DisplayName = "Minimal Download Strategy", InternalName = "MDSFeature", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("5b79b49a-2da6-4161-95bd-7375c1995ef9"), DisplayName = "Media Web Part", InternalName = "MediaWebPart", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("947afd14-0ea1-46c6-be97-dea1bf6f5bae"), DisplayName = "Site Membership", InternalName = "MembershipList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("7201d6a4-a5d3-49a1-8c19-19c4bac6e668"), DisplayName = "Metadata Navigation and Filtering", InternalName = "MetaDataNav", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("5a020a4f-c449-4a65-b07d-f2cc2d8778dd"), DisplayName = "Excel Services Application Mobile Excel Web Access Feature", InternalName = "MobileEwaFarm", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("5a020a4f-c449-4a65-b07d-f2cc2d8778dd"), DisplayName = "$Resources&#58;xlsrv;", InternalName = "MobileEwaFarm", Hidden = false, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("e995e28b-9ba8-4668-9933-cf5c146d7a9f"), DisplayName = "Excel Services Application Mobile Excel Web Access Feature", InternalName = "MobileExcelWebAccess", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("e995e28b-9ba8-4668-9933-cf5c146d7a9f"), DisplayName = "Excel Mobile Viewer Feature", InternalName = "MobileExcelWebAccess", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("893627d9-b5ef-482d-a3bf-2a605175ac36"), DisplayName = "$Resources&#58;pptservercore;", InternalName = "MobilePowerPointViewer", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("8dfaf93d-e23c-4471-9347-07368668ddaf"), DisplayName = "$Resources&#58;waccore;", InternalName = "MobileWordViewer", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("f41cc668-37e5-4743-b4a8-74d1db3fd8a4"), DisplayName = "Mobility Shortcut URL", InternalName = "MobilityRedirect", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("345ff4f9-f706-41e1-92bc-3f0ec2d9f6ea"), DisplayName = "App Monitor", InternalName = "MonitoredApps", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("1b811cfe-8c78-4982-aad7-e5c112e397d1"), DisplayName = "App Monitor user interface", InternalName = "MonitoredAppsUI", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("875d1044-c0cf-4244-8865-d2a0039c2a49"), DisplayName = "Chart Web Part", InternalName = "MossChart", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("39dd29fb-b6f5-4697-b526-4d38de4893e5"), DisplayName = "Meetings Workspaces Web Parts", InternalName = "MpsWebParts", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("1eb6a0c1-5f08-4672-b96f-16845c2448c6"), DisplayName = "Recent Documents", InternalName = "MruDocsWebPart", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("69cc9662-d373-47fc-9449-f18d11ff732c"), DisplayName = "My Site", InternalName = "MySite", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("863da2ac-3873-4930-8498-752886210911"), DisplayName = "My Site Blogs", InternalName = "MySiteBlog", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("0faf7d1b-95b1-4053-b4e2-19fd5c9bbc88"), DisplayName = "My Site Cleanup Feature", InternalName = "MySiteCleanup", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("e9c0ff81-d821-4771-8b4c-246aa7e5e9eb"), DisplayName = "My Site Document Library Feature", InternalName = "MySiteDocumentLibrary", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("49571cd1-b6a1-43a3-bf75-955acc79c8d8"), DisplayName = "My Site Host", InternalName = "MySiteHost", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("5ede0a86-c772-4f1d-a120-72e734b3400c"), DisplayName = "Shared Picture Library for Organizations logos", InternalName = "MySiteHostPictureLibrary", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("65b53aaf-4754-46d7-bb5b-7ed4cf5564e1"), DisplayName = "My Site Instantiation Queue timer jobs", InternalName = "MySiteInstantiationQueues", Hidden = true, IsCustomDefinition = false, Scope = Scope.WebApplication });
            _features.Add(new Feature() { Id = new Guid("6928b0e5-5707-46a1-ae16-d6e52522d52b"), DisplayName = "My Site Layouts Feature", InternalName = "MySiteLayouts", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("fb01ca75-b306-4fc2-ab27-b4814bf823d1"), DisplayName = "My Site Master Feature", InternalName = "MySiteMaster", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("ea23650b-0340-4708-b465-441a41c37af7"), DisplayName = "MySite MicroBlogging List", InternalName = "MySiteMicroBlog", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("dfa42479-9531-4baf-8873-fc65b22c9bd4"), DisplayName = "MySite MicroBlogging", InternalName = "MySiteMicroBlogCtrl", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("6adff05c-d581-4c05-a6b9-920f15ec6fd9"), DisplayName = "My Site Navigation", InternalName = "MySiteNavigation", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("f661430e-c155-438e-a7c6-c68648f1b119"), DisplayName = "My Site Personal Site Configuration", InternalName = "MySitePersonalSite", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("034947cc-c424-47cd-a8d1-6014f0e36925"), DisplayName = "My Site Layouts Feature", InternalName = "MySiteQuickLaunch", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("b2741073-a92b-4836-b1d8-d5e9d73679bb"), DisplayName = "MySite Social Deployment Scenario", InternalName = "MySiteSocialDeployment", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("0ee1129f-a2f3-41a9-9e9c-c7ee619a8c33"), DisplayName = "MySite Storage Deployment Scenario", InternalName = "MySiteStorageDeployment", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("41baa678-ad62-41ef-87e6-62c8917fc0ad"), DisplayName = "My Site Unified Navigation Feature", InternalName = "MySiteUnifiedNavigation", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("eaa41f18-8e4a-4894-baee-60a87f026e42"), DisplayName = "My Site Single Quick Launch Feature", InternalName = "MySiteUnifiedQuickLaunch", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("89d1184c-8191-4303-a430-7a24291531c9"), DisplayName = "My Tasks Dashboard", InternalName = "MyTasksDashboard", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("04a98ac6-82d5-4e01-80ea-c0b7d9699d94"), DisplayName = "My Tasks Dashboard Custom Redirect", InternalName = "MyTasksDashboardCustomRedirect", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("4cc8aab8-5af0-45d7-a170-169ea583866e"), DisplayName = "My Tasks Dashboard Stapling", InternalName = "MyTasksDashboardStapling", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("89e0306d-453b-4ec5-8d68-42067cdbf98e"), DisplayName = "Portal Navigation", InternalName = "Navigation", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("541f5f57-c847-4e16-b59a-b31e90e6f9ea"), DisplayName = "Portal Navigation Properties", InternalName = "NavigationProperties", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("00bfea71-f600-43f6-a895-40c0de7b0117"), DisplayName = "No-code Workflow Libraries", InternalName = "NoCodeWorkflowLibrary", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("683df0c0-20b7-4852-87a3-378945158fab"), DisplayName = "BDC Profile Pages Feature", InternalName = "ObaProfilePages", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("90c6c1e5-3719-4c52-9f36-34a97df596f7"), DisplayName = "BDC Profile Pages Tenant Stapling Feature", InternalName = "ObaProfilePagesTenantStapling", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("d250636f-0a26-4019-8425-a5232d592c01"), DisplayName = "Offline Synchronization for External Lists", InternalName = "ObaSimpleSolution", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("f9cb1a2a-d285-465a-a160-7e3e95af1fdd"), DisplayName = "Offline Synchronization for External Lists", InternalName = "ObaStaple", Hidden = false, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("61e874cd-3ac3-4531-8628-28c3acb78279"), DisplayName = "Apps for Office Catalog", InternalName = "OfficeExtensionCatalog", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("0c504a5c-bcea-4376-b05e-cbca5ced7b4f"), DisplayName = "Deprecated Office Web Apps", InternalName = "OfficeWebApps", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("c9c9515d-e4e2-4001-9050-74f980f93160"), DisplayName = "Microsoft Office Server workflows", InternalName = "OffWFCommon", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3d433d02-cf49-4975-81b4-aede31e16edf"), DisplayName = "Deprecated Office Web Apps", InternalName = "OnenoteServerViewing", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("8a4b8de2-6fd8-41e9-923c-c7c3c00f8295"), DisplayName = "Open Documents in Client Applications by Default", InternalName = "OpenInClient", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("9b0293a7-8942-46b0-8b78-49d29a9edd53"), DisplayName = "Organizations Claim Hierarchy Provider", InternalName = "OrganizationsClaimHierarchyProvider", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("bc29e863-ae07-4674-bd83-2c6d0aa5623f"), DisplayName = "SharePoint Server Site Search", InternalName = "OSearchBasicFeature", Hidden = false, IsCustomDefinition = false, Scope = Scope.WebApplication });
            _features.Add(new Feature() { Id = new Guid("c922c106-7d0a-4377-a668-7f13d52cb80f"), DisplayName = "Search Central Admin Links", InternalName = "OSearchCentralAdminLinks", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("4750c984-7721-4feb-be61-c660c6190d43"), DisplayName = "SharePoint Server Enterprise Search", InternalName = "OSearchEnhancedFeature", Hidden = false, IsCustomDefinition = false, Scope = Scope.WebApplication });
            _features.Add(new Feature() { Id = new Guid("e792e296-5d7f-47c7-9dfa-52eae2104c3b"), DisplayName = "$Resources:HealthReportsFeatureTitle;", InternalName = "OSearchHealthReports", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("09fe98f3-3324-4747-97e5-916a28a0c6c0"), DisplayName = "Health Reports Pushdown Feature", InternalName = "OSearchHealthReportsPushdown", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("edf48246-e4ee-4638-9eed-ef3d0aee7597"), DisplayName = "Search Admin Portal Links and Navbar", InternalName = "OSearchPortalAdminLinks", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("068f8656-bea6-4d60-a5fa-7f077f8f5c20"), DisplayName = "Shared Services Administration Links", InternalName = "OsrvLinks", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("10bdac29-a21a-47d9-9dff-90c7cae1301e"), DisplayName = "Shared Services Navigation", InternalName = "OssNavigation", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("03b0a3dc-93dd-4c68-943e-7ec56e65ed4d"), DisplayName = "$Resources:SearchEndUserHelp_Feature_Title;", InternalName = "OSSSearchEndUserHelpFeature", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("7acfcb9d-8e8f-4979-af7e-8aed7e95245e"), DisplayName = "Search Center URL", InternalName = "OSSSearchSearchCenterUrlFeature", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("7ac8cc56-d28e-41f5-ad04-d95109eb987a"), DisplayName = "Site collection level Search Center Url Feature", InternalName = "OSSSearchSearchCenterUrlSiteFeature", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("14173c38-5e2d-4887-8134-60f9df889bad"), DisplayName = "Document to Page Converters", InternalName = "PageConverters", Hidden = true, IsCustomDefinition = false, Scope = Scope.WebApplication });
            _features.Add(new Feature() { Id = new Guid("ed5e77f7-c7b1-4961-a659-0de93080fa36"), DisplayName = "Personalization Site", InternalName = "PersonalizationSite", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("41e1d4bf-b1a2-47f7-ab80-d5d6cbba3092"), DisplayName = "Push Notifications", InternalName = "PhonePNSubscriber", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("00bfea71-52d4-45b3-b544-b1c71b620109"), DisplayName = "Picture Libraries", InternalName = "PictureLibrary", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("5f3b0127-2f1d-4cfd-8dd2-85ad1fb00bfc"), DisplayName = "Portal Layouts Feature", InternalName = "PortalLayouts", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("4f31948e-8dc7-4e67-a4b7-070941848658"), DisplayName = "PowerPivot Administrative Feature", InternalName = "PowerPivotCA", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("f8c51e81-0b46-4535-a3d5-244f63e1cab9"), DisplayName = "PowerPivot Integration Feature", InternalName = "PowerPivotFarm", Hidden = false, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("1a33a234-b4a4-4fc6-96c2-8bdb56388bd5"), DisplayName = "PowerPivot Feature Integration for Site Collections", InternalName = "PowerPivotSite", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("30a6403b-b04f-42cc-805a-bc4d77826253"), DisplayName = "$Resources&#58;pptservercore;", InternalName = "PowerPointBroadcastServer", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("738250ba-9327-4dc0-813a-a76928ba1842"), DisplayName = "$Resources&#58;pptservercore;", InternalName = "PowerPointEditServer ", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("5709298b-1876-4686-b257-f101a923f58d"), DisplayName = "$Resources&#58;pptservercore;", InternalName = "PowerPointServer", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("bf8b58f5-ebae-4a70-9848-622beaaf2043"), DisplayName = "Power View Integration Feature", InternalName = "PowerView", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3b5dc9dd-896c-4d6b-8c73-8f854b3a652b"), DisplayName = "Power View Integration Stapling Feature", InternalName = "PowerViewStapling", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("5d220570-df17-405e-b42d-994237d60ebf"), DisplayName = "PerformancePoint Data Source Library Template", InternalName = "PPSDatasourceLib", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("05891451-f0c4-4d4e-81b1-0dabd840bad4"), DisplayName = "PerformancePoint Datasource Content Type definition", InternalName = "PPSMonDatasourceCtype", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("ae31cd14-a866-4834-891a-97c9d37662a2"), DisplayName = "PerformancePoint Ribbon", InternalName = "PPSRibbon", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a1cb5b7f-e5e9-421b-915f-bf519b0760ef"), DisplayName = "PerformancePoint Services Site Collection Features", InternalName = "PPSSiteCollectionMaster", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("0b07a7f4-8bb8-4ec0-a31b-115732b9584d"), DisplayName = "PerformancePoint Services Site Features", InternalName = "PPSSiteMaster", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("8472208f-5a01-4683-8119-3cea50bea072"), DisplayName = "PPS Site Stapling", InternalName = "PPSSiteStapling", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("ee9dbf20-1758-401e-a169-7db0a6bbccb2"), DisplayName = "PerformancePoint Monitoring", InternalName = "PPSWebParts", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("f45834c7-54f6-48db-b7e4-a35fa470fc9b"), DisplayName = "PerformancePoint Content Type Definition", InternalName = "PPSWorkspaceCtype", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("481333e1-a246-4d89-afab-d18c6fe344ce"), DisplayName = "PerformancePoint Content List", InternalName = "PPSWorkspaceList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("9e99f7d7-08e9-455c-b3aa-fc71b9210027"), DisplayName = "SharePoint Server Enterprise Premium Search Verticals", InternalName = "PremiumSearchVerticals", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("8581a8a7-cf16-4770-ac54-260265ddb0b2"), DisplayName = "SharePoint Server Enterprise Site Collection features", InternalName = "PremiumSite", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a573867a-37ca-49dc-86b0-7d033a7ed2c8"), DisplayName = "Premium Site Features Stapling", InternalName = "PremiumSiteStapling", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("0806d127-06e6-447a-980e-2e90b03101b8"), DisplayName = "SharePoint Server Enterprise Site features", InternalName = "PremiumWeb", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("0ea1c3b6-6ac0-44aa-9f3f-05e8dbe6d70b"), DisplayName = "SharePoint Server Enterprise Web application features", InternalName = "PremiumWebApplication", Hidden = false, IsCustomDefinition = false, Scope = Scope.WebApplication });
            _features.Add(new Feature() { Id = new Guid("bfc789aa-87ba-4d79-afc7-0c7e45dae01a"), DisplayName = "Preservation List", InternalName = "Preservation", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("dd926489-fc66-47a6-ba00-ce0e959c9b41"), DisplayName = "Product Catalog List Template", InternalName = "ProductCatalogListTemplate", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("409d2feb-3afb-4642-9462-f7f426a0f3e9"), DisplayName = "Product Catalog Site", InternalName = "ProductCatalogResources", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("af847aa9-beb6-41d4-8306-78e41af9ce25"), DisplayName = "Profile Synchronization Feature", InternalName = "ProfileSynch", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("2fcd5f8a-26b7-4a6a-9755-918566dba90a"), DisplayName = "Site Policy", InternalName = "ProjectBasedPolicy", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("4446ee9b-227c-4f1a-897d-d78ecdd6a824"), DisplayName = "MySite Recommendations", InternalName = "ProjectDiscovery", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("e2f2bb18-891d-4812-97df-c265afdba297"), DisplayName = "Project Functionality", InternalName = "ProjectFunctionality", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("192efa95-e50c-475e-87ab-361cede5dd7f"), DisplayName = "Promoted Links", InternalName = "PromotedLinksList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("22a9ef51-737b-4ff2-9346-694633fe4416"), DisplayName = "Publishing", InternalName = "Publishing", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("d3f51be2-38a8-4e44-ba84-940d35be1566"), DisplayName = "Page Layouts and Master Pages Pack", InternalName = "PublishingLayouts", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("57cc6207-aebf-426e-9ece-45946ea82e4a"), DisplayName = "Mobile and Alternate Device Targeting", InternalName = "PublishingMobile", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a392da98-270b-4e85-9769-04c0fde267aa"), DisplayName = "Publishing Prerequisites", InternalName = "PublishingPrerequisites", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("aebc918d-b20f-4a11-a1db-9ed84d79c87e"), DisplayName = "Publishing Resources", InternalName = "PublishingResources", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("f6924d36-2fa8-4f0b-b16d-06b7250180fa"), DisplayName = "SharePoint Server Publishing Infrastructure", InternalName = "PublishingSite", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("001f4bd7-746d-403b-aa09-a6cc43de7942"), DisplayName = "Publishing Features Stapling", InternalName = "PublishingStapling", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("20477d83-8bdb-414e-964b-080637f7d99b"), DisplayName = "Publishing Timer Jobs", InternalName = "PublishingTimerJobs", Hidden = true, IsCustomDefinition = false, Scope = Scope.WebApplication });
            _features.Add(new Feature() { Id = new Guid("94c94ca6-b32f-4da9-a9e3-1f3d343d7ecb"), DisplayName = "SharePoint Server Publishing", InternalName = "PublishingWeb", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("d9742165-b024-4713-8653-851573b9dfbd"), DisplayName = "Query Based In-Place Hold", InternalName = "QueryBasedPreservation", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("915c240e-a6cc-49b8-8b2c-0bff8b553ed3"), DisplayName = "Ratings", InternalName = "Ratings", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("5bccb9a4-b903-4fd1-8620-b795fa33c9ba"), DisplayName = "Record Resources", InternalName = "RecordResources", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("e0a45587-1069-46bd-bf05-8c8db8620b08"), DisplayName = "Records Center Configuration", InternalName = "RecordsCenter", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("6d127338-5e7d-4391-8f62-a11e43b1d404"), DisplayName = "Records Management", InternalName = "RecordsManagement", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("b5ef96cb-d714-41da-b66c-ce3517034c21"), DisplayName = "Records Management Tenant Administration", InternalName = "RecordsManagementTenantAdmin", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("8c54e5d3-4635-4dff-a533-19fe999435dc"), DisplayName = "Records Management Tenant Administration Stapling", InternalName = "RecordsManagementTenantAdminStapling", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("306936fd-9806-4478-80d1-7e397bfa6474"), DisplayName = "SharePoint Portal Server Redirect Page Content Type Binding Feature", InternalName = "RedirectPageContentTypeBinding", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("e8734bb6-be8e-48a1-b036-5a40ff0b8a81"), DisplayName = "Related Links scope settings page", InternalName = "RelatedLinksScopeSettingsLink", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("b9455243-e547-41f0-80c1-d5f6ce6a19e5"), DisplayName = "Reports and Data Search", InternalName = "ReportAndDataSearch", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("c5d947d6-b0a2-4e07-9929-8e54f5a9fff9"), DisplayName = "Report Center Sample Data", InternalName = "ReportCenterSampleData", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("7094bd89-2cfe-490a-8c7e-fbace37b4a34"), DisplayName = "Reporting", InternalName = "Reporting", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("2510d73f-7109-4ccc-8a1c-314894deeb3a"), DisplayName = "SharePoint Portal Server Report Library", InternalName = "ReportListTemplate", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("e0a9f213-54f5-4a5a-81d5-f5f3dbe48977"), DisplayName = "SharePoint Portal Server Reports And Data Content Type Definition", InternalName = "ReportsAndDataCTypes", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("365356ee-6c88-4cf1-92b8-fa94a8b8c118"), DisplayName = "SPS Reports and Data Field Definition", InternalName = "ReportsAndDataFields", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("b435069a-e096-46e0-ae30-899daca4b304"), DisplayName = "Reports and Data Search Support", InternalName = "ReportsAndDataListTemplates", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("e8389ec7-70fd-4179-a1c4-6fcb4342d7a0"), DisplayName = "Report Server Integration Feature", InternalName = "ReportServer", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("5f2e3537-91b5-4341-86ff-90c6a2f99aae"), DisplayName = "Report Server Central Administration Feature", InternalName = "ReportServerCentralAdmin", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("c769801e-2387-47ef-a810-2d292d4cb05d"), DisplayName = "Report Server File Sync", InternalName = "ReportServerItemSync", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("6bcbccc3-ff47-47d3-9468-572bf2ab9657"), DisplayName = "Report Server Integration Stapling Feature", InternalName = "ReportServerStapling", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("a44d2aa3-affc-4d58-8db4-f4a3af053188"), DisplayName = "Publishing Approval Workflow", InternalName = "ReviewPublishingSPD", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0401"), DisplayName = "Publishing Workflow - SharePoint 2013  (ar-sa)", InternalName = "ReviewPublishingSPD1025", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0402"), DisplayName = "Publishing Workflow - SharePoint 2013  (bg-bg)", InternalName = "ReviewPublishingSPD1026", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0403"), DisplayName = "Publishing Workflow - SharePoint 2013  (ca-es)", InternalName = "ReviewPublishingSPD1027", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0404"), DisplayName = "Publishing Workflow - SharePoint 2013  (zh-tw)", InternalName = "ReviewPublishingSPD1028", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0405"), DisplayName = "Publishing Workflow - SharePoint 2013  (cs-cz)", InternalName = "ReviewPublishingSPD1029", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0406"), DisplayName = "Publishing Workflow - SharePoint 2013  (da-dk)", InternalName = "ReviewPublishingSPD1030", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0407"), DisplayName = "Publishing Workflow - SharePoint 2013  (de-de)", InternalName = "ReviewPublishingSPD1031", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0408"), DisplayName = "Publishing Workflow - SharePoint 2013  (el-gr)", InternalName = "ReviewPublishingSPD1032", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0409"), DisplayName = "Publishing Workflow - SharePoint 2013  (en-US)", InternalName = "ReviewPublishingSPD1033", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead040b"), DisplayName = "Publishing Workflow - SharePoint 2013  (fi-fi)", InternalName = "ReviewPublishingSPD1035", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead040b"), DisplayName = "Publishing Workflow - SharePoint 2013  (fi-fi)", InternalName = "ReviewPublishingSPD1035", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead040c"), DisplayName = "Publishing Workflow - SharePoint 2013  (fr-fr)", InternalName = "ReviewPublishingSPD1036", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead040d"), DisplayName = "Publishing Workflow - SharePoint 2013  (he-IL)", InternalName = "ReviewPublishingSPD1037", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead040e"), DisplayName = "Publishing Workflow - SharePoint 2013  (hu-HU)", InternalName = "ReviewPublishingSPD1038", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0410"), DisplayName = "Publishing Workflow - SharePoint 2013  (it-it)", InternalName = "ReviewPublishingSPD1040", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0411"), DisplayName = "Publishing Workflow - SharePoint 2013  (ja-jp)", InternalName = "ReviewPublishingSPD1041", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0412"), DisplayName = "Publishing Workflow - SharePoint 2013  (ko-KR)", InternalName = "ReviewPublishingSPD1042", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0413"), DisplayName = "Publishing Workflow - SharePoint 2013  (nl-nl)", InternalName = "ReviewPublishingSPD1043", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0414"), DisplayName = "Publishing Workflow - SharePoint 2013  (nb-no)", InternalName = "ReviewPublishingSPD1044", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0415"), DisplayName = "Publishing Workflow - SharePoint 2013  (pl-pl)", InternalName = "ReviewPublishingSPD1045", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0416"), DisplayName = "Publishing Workflow - SharePoint 2013  (pt-br)", InternalName = "ReviewPublishingSPD1046", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0418"), DisplayName = "Publishing Workflow - SharePoint 2013  (ro-ro)", InternalName = "ReviewPublishingSPD1048", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0419"), DisplayName = "Publishing Workflow - SharePoint 2013  (ru-RU)", InternalName = "ReviewPublishingSPD1049", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead041a"), DisplayName = "Publishing Workflow - SharePoint 2013  (hr-hr)", InternalName = "ReviewPublishingSPD1050", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead041b"), DisplayName = "Publishing Workflow - SharePoint 2013  (sk-sk)", InternalName = "ReviewPublishingSPD1051", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead041d"), DisplayName = "Publishing Workflow - SharePoint 2013  (sv-se)", InternalName = "ReviewPublishingSPD1053", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead041e"), DisplayName = "Publishing Workflow - SharePoint 2013  (th-TH)", InternalName = "ReviewPublishingSPD1054", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead041f"), DisplayName = "Publishing Workflow - SharePoint 2013  (tr-tr)", InternalName = "ReviewPublishingSPD1055", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0422"), DisplayName = "Publishing Workflow - SharePoint 2013  (uk-ua)", InternalName = "ReviewPublishingSPD1058", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0424"), DisplayName = "Publishing Workflow - SharePoint 2013  (sl-si)", InternalName = "ReviewPublishingSPD1060", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0425"), DisplayName = "Publishing Workflow - SharePoint 2013  (et-ee)", InternalName = "ReviewPublishingSPD1061", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0426"), DisplayName = "Publishing Workflow - SharePoint 2013  (lv-lv)", InternalName = "ReviewPublishingSPD1062", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0427"), DisplayName = "Publishing Workflow - SharePoint 2013  (lt-lt)", InternalName = "ReviewPublishingSPD1063", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead042a"), DisplayName = "Publishing Workflow - SharePoint 2013  (vi-vn)", InternalName = "ReviewPublishingSPD1066", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead042d"), DisplayName = "Publishing Workflow - SharePoint 2013  (eu-es)", InternalName = "ReviewPublishingSPD1069", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0439"), DisplayName = "Publishing Workflow - SharePoint 2013  (hi-in)", InternalName = "ReviewPublishingSPD1081", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead043f"), DisplayName = "Publishing Workflow - SharePoint 2013  (??-??)", InternalName = "ReviewPublishingSPD1087", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0456"), DisplayName = "Publishing Workflow - SharePoint 2013  (gl-es)", InternalName = "ReviewPublishingSPD1110", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0804"), DisplayName = "Publishing Workflow - SharePoint 2013  (zh-CN)", InternalName = "ReviewPublishingSPD2052", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0816"), DisplayName = "Publishing Workflow - SharePoint 2013  (pt-PT)", InternalName = "ReviewPublishingSPD2070", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead081a"), DisplayName = "Publishing Workflow - SharePoint 2013  (sr-latn-cs)", InternalName = "ReviewPublishingSPD2074", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0c0a"), DisplayName = "Publishing Workflow - SharePoint 2013  (es-ES)", InternalName = "ReviewPublishingSPD3082", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("02464c6a-9d07-4f30-ba04-e9035cf54392"), DisplayName = "Routing Workflows", InternalName = "ReviewWorkflows", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("b5934f65-a844-4e67-82e5-92f66aafe912"), DisplayName = "Routing Workflows - SharePoint 2010", InternalName = "ReviewWorkflowsSPD", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b60401"), DisplayName = "Routing Workflows - SharePoint 2013  (ar-sa)", InternalName = "ReviewWorkflowsSPD1025", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b60404"), DisplayName = "Routing Workflows - SharePoint 2013  (zh-tw)", InternalName = "ReviewWorkflowsSPD1028", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b60405"), DisplayName = "Routing Workflows - SharePoint 2013  (cs-cz)", InternalName = "ReviewWorkflowsSPD1029", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b60406"), DisplayName = "Routing Workflows - SharePoint 2013  (da-dk)", InternalName = "ReviewWorkflowsSPD1030", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b60407"), DisplayName = "Routing Workflows - SharePoint 2013  (de-de)", InternalName = "ReviewWorkflowsSPD1031", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b60408"), DisplayName = "Routing Workflows - SharePoint 2013  (el-gr)", InternalName = "ReviewWorkflowsSPD1032", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b60409"), DisplayName = "Routing Workflows - SharePoint 2013  (en-US)", InternalName = "ReviewWorkflowsSPD1033", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b6040b"), DisplayName = "Routing Workflows - SharePoint 2013  (fi-fi)", InternalName = "ReviewWorkflowsSPD1035", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b6040c"), DisplayName = "Routing Workflows - SharePoint 2013  (fr-fr)", InternalName = "ReviewWorkflowsSPD1036", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b6040d"), DisplayName = "Routing Workflows - SharePoint 2013  (he-IL)", InternalName = "ReviewWorkflowsSPD1037", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b6040e"), DisplayName = "Routing Workflows - SharePoint 2013  (hu-HU)", InternalName = "ReviewWorkflowsSPD1038", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b60410"), DisplayName = "Routing Workflows - SharePoint 2013  (it-it)", InternalName = "ReviewWorkflowsSPD1040", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b60411"), DisplayName = "Routing Workflows - SharePoint 2013  (ja-jp)", InternalName = "ReviewWorkflowsSPD1041", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b60412"), DisplayName = "Routing Workflows - SharePoint 2013  (ko-KR)", InternalName = "ReviewWorkflowsSPD1042", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b60413"), DisplayName = "Routing Workflows - SharePoint 2013  (nl-nl)", InternalName = "ReviewWorkflowsSPD1043", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b60414"), DisplayName = "Routing Workflows - SharePoint 2013  (nb-no)", InternalName = "ReviewWorkflowsSPD1044", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b60415"), DisplayName = "Routing Workflows - SharePoint 2013  (pl-pl)", InternalName = "ReviewWorkflowsSPD1045", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b60419"), DisplayName = "Routing Workflows - SharePoint 2013  (ru-RU)", InternalName = "ReviewWorkflowsSPD1049", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b6041b"), DisplayName = "Routing Workflows - SharePoint 2013  (sk-sk)", InternalName = "ReviewWorkflowsSPD1051", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b6041d"), DisplayName = "Routing Workflows - SharePoint 2013  (sv-se)", InternalName = "ReviewWorkflowsSPD1053", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b6041e"), DisplayName = "Routing Workflows - SharePoint 2013  (th-TH)", InternalName = "ReviewWorkflowsSPD1054", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b6041f"), DisplayName = "Routing Workflows - SharePoint 2013  (tr-tr)", InternalName = "ReviewWorkflowsSPD1055", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b60424"), DisplayName = "Routing Workflows - SharePoint 2013  (sl-si)", InternalName = "ReviewWorkflowsSPD1060", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b60426"), DisplayName = "Routing Workflows - SharePoint 2013  (lv-lv)", InternalName = "ReviewWorkflowsSPD1062", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b60427"), DisplayName = "Routing Workflows - SharePoint 2013  (lt-lt)", InternalName = "ReviewWorkflowsSPD1063", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b60439"), DisplayName = "Routing Workflows - SharePoint 2013  (hi-in)", InternalName = "ReviewWorkflowsSPD1081", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b60804"), DisplayName = "Routing Workflows - SharePoint 2013  (zh-CN)", InternalName = "ReviewWorkflowsSPD2052", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b60816"), DisplayName = "Routing Workflows - SharePoint 2013  (pt-PT)", InternalName = "ReviewWorkflowsSPD2070", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b60c0a"), DisplayName = "Routing Workflows - SharePoint 2013  (es-ES)", InternalName = "ReviewWorkflowsSPD3082", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("588b23d5-8e23-4b1b-9fe3-2f2f62965f2d"), DisplayName = "Rollup Page Layouts", InternalName = "RollupPageLayouts", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("dffaae84-60ee-413a-9600-1cf431cf0560"), DisplayName = "Rollup Pages", InternalName = "RollupPages", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("636287a7-7f62-4a6e-9fcc-081f4672cbf8"), DisplayName = "Schedule and Reservations List", InternalName = "ScheduleList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("c65861fa-b025-4634-ab26-22a23e49808f"), DisplayName = "Microsoft Search Administration Web Parts", InternalName = "SearchAdminWebParts", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("1dbf6063-d809-45ea-9203-d3ba4a64f86d"), DisplayName = "Search And Process", InternalName = "SearchAndProcess", Hidden = true, IsCustomDefinition = false, Scope = Scope.WebApplication });
            _features.Add(new Feature() { Id = new Guid("6077b605-67b9-4937-aeb6-1d41e8f5af3b"), DisplayName = "Search Server Center Files", InternalName = "SearchCenterFiles", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("073232a0-1868-4323-a144-50de99c70efc"), DisplayName = "Search Server Center Lite Files", InternalName = "SearchCenterLiteFiles", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("fbbd1168-3b17-4f29-acb4-ef2d34c54cfb"), DisplayName = "Search Server Center Files", InternalName = "SearchCenterLiteUpgrade", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("372b999f-0807-4427-82dc-7756ae73cb74"), DisplayName = "Search Server Center Files", InternalName = "SearchCenterUpgrade", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("48a243cb-7b16-4b5a-b1b5-07b809b43f47"), DisplayName = "Search Config Data Content Types", InternalName = "SearchConfigContentType", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("41dfb393-9eb6-4fe4-af77-28e4afce8cdc"), DisplayName = "Search Config Data Site Columns", InternalName = "SearchConfigFields", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("acb15743-f07b-4c83-8af3-ffcfdf354965"), DisplayName = "Search Config List Instance Feature", InternalName = "SearchConfigList", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("e47705ec-268d-4c41-aa4e-0d8727985ebc"), DisplayName = "Search Config Template Feature", InternalName = "SearchConfigListTemplate", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("9fb35ca8-824b-49e6-a6c5-cba4366444ab"), DisplayName = "$Resources:SearchConfigTenantStapler_Feature_Title;", InternalName = "SearchConfigTenantStapler", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("592ccb4a-9304-49ab-aab1-66638198bb58"), DisplayName = "Search-Driven Content", InternalName = "SearchDrivenContent", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("17415b1d-5339-42f9-a10b-3fef756b84d1"), DisplayName = "Search Engine Optimization Feature", InternalName = "SearchEngineOptimization", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("5eac763d-fbf5-4d6f-a76b-eded7dd7b0a5"), DisplayName = "Search extensions", InternalName = "SearchExtensions", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("9c0834e1-ba47-4d49-812b-7d4fb6fea211"), DisplayName = "Search Server Web Parts and Templates", InternalName = "SearchMaster", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("e09cefae-2ada-4a1d-aee6-8a8398215905"), DisplayName = "$Resources:SearchServerWizard_Feature_Title;", InternalName = "SearchServerWizardFeature", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("67ae7d04-6731-42dd-abe1-ba2a5eaa3b48"), DisplayName = "Search Server Web Parts and Support Files", InternalName = "SearchTaxonomyRefinementWebParts", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("8c34f59f-8dfb-4a39-9a08-7497237e3dc4"), DisplayName = "Search Server Web Parts and Support Files", InternalName = "SearchTaxonomyRefinementWebPartsHtml", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("8b2c6bcb-c47f-4f17-8127-f8eae47a44dd"), DisplayName = "Display Templates for Search and Content Web Parts", InternalName = "SearchTemplatesandResources", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("eaf6a128-0482-4f71-9a2f-b1c650680e77"), DisplayName = "Search Server Web Parts and Support Files", InternalName = "SearchWebParts", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("922ed989-6eb4-4f5e-a32e-27f31f93abfa"), DisplayName = "Search Server Web Parts and Support Files Stapler", InternalName = "SearchWebPartsStapler", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("f324259d-393d-4305-aa48-36e8d9a7a0d6"), DisplayName = "Shared Services Infrastructure", InternalName = "SharedServices", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("10f73b29-5779-46b3-85a8-4817a6e9a6c2"), DisplayName = "Share with Everyone", InternalName = "ShareWithEveryone", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("87866a72-efcf-4993-b5b0-769776b5283f"), DisplayName = "Share with Everyone feature stapling", InternalName = "ShareWithEveryoneStapling", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("6c09612b-46af-4b2f-8dfc-59185c962a29"), DisplayName = "Collect Signatures Workflow", InternalName = "SignaturesWorkflow", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("c4773de6-ba70-4583-b751-2a7b1dc67e3a"), DisplayName = "Collect Signatures Workflow - SharePoint 2010", InternalName = "SignaturesWorkflowSPD", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b40190401"), DisplayName = "Collect Signatures Workflow - SharePoint 2013  (ar-sa)", InternalName = "SignaturesWorkflowSPD1025", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b40190404"), DisplayName = "Collect Signatures Workflow - SharePoint 2013  (zh-tw)", InternalName = "SignaturesWorkflowSPD1028", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b40190405"), DisplayName = "Collect Signatures Workflow - SharePoint 2013  (cs-cz)", InternalName = "SignaturesWorkflowSPD1029", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b40190406"), DisplayName = "Collect Signatures Workflow - SharePoint 2013  (da-dk)", InternalName = "SignaturesWorkflowSPD1030", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b40190407"), DisplayName = "Collect Signatures Workflow - SharePoint 2013  (de-de)", InternalName = "SignaturesWorkflowSPD1031", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b40190408"), DisplayName = "Collect Signatures Workflow - SharePoint 2013  (el-gr)", InternalName = "SignaturesWorkflowSPD1032", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b40190409"), DisplayName = "Collect Signatures Workflow - SharePoint 2013  (en-US)", InternalName = "SignaturesWorkflowSPD1033", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b4019040b"), DisplayName = "Collect Signatures Workflow - SharePoint 2013  (fi-fi)", InternalName = "SignaturesWorkflowSPD1035", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b4019040c"), DisplayName = "Collect Signatures Workflow - SharePoint 2013  (fr-fr)", InternalName = "SignaturesWorkflowSPD1036", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b4019040d"), DisplayName = "Collect Signatures Workflow - SharePoint 2013  (he-IL)", InternalName = "SignaturesWorkflowSPD1037", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b4019040e"), DisplayName = "Collect Signatures Workflow - SharePoint 2013  (hu-HU)", InternalName = "SignaturesWorkflowSPD1038", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b40190410"), DisplayName = "Collect Signatures Workflow - SharePoint 2013  (it-it)", InternalName = "SignaturesWorkflowSPD1040", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b40190411"), DisplayName = "Collect Signatures Workflow - SharePoint 2013  (ja-jp)", InternalName = "SignaturesWorkflowSPD1041", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b40190412"), DisplayName = "Collect Signatures Workflow - SharePoint 2013  (ko-KR)", InternalName = "SignaturesWorkflowSPD1042", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b40190413"), DisplayName = "Collect Signatures Workflow - SharePoint 2013  (nl-nl)", InternalName = "SignaturesWorkflowSPD1043", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b40190414"), DisplayName = "Collect Signatures Workflow - SharePoint 2013  (nb-no)", InternalName = "SignaturesWorkflowSPD1044", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b40190415"), DisplayName = "Collect Signatures Workflow - SharePoint 2013  (pl-pl)", InternalName = "SignaturesWorkflowSPD1045", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b40190419"), DisplayName = "Collect Signatures Workflow - SharePoint 2013  (ru-RU)", InternalName = "SignaturesWorkflowSPD1049", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b4019041b"), DisplayName = "Collect Signatures Workflow - SharePoint 2013  (sk-sk)", InternalName = "SignaturesWorkflowSPD1051", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b4019041d"), DisplayName = "Collect Signatures Workflow - SharePoint 2013  (sv-se)", InternalName = "SignaturesWorkflowSPD1053", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b4019041e"), DisplayName = "Collect Signatures Workflow - SharePoint 2013  (th-TH)", InternalName = "SignaturesWorkflowSPD1054", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b4019041f"), DisplayName = "Collect Signatures Workflow - SharePoint 2013  (tr-tr)", InternalName = "SignaturesWorkflowSPD1055", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b40190424"), DisplayName = "Collect Signatures Workflow - SharePoint 2013  (sl-si)", InternalName = "SignaturesWorkflowSPD1060", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b40190426"), DisplayName = "Collect Signatures Workflow - SharePoint 2013  (lv-lv)", InternalName = "SignaturesWorkflowSPD1062", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b40190427"), DisplayName = "Collect Signatures Workflow - SharePoint 2013  (lt-lt)", InternalName = "SignaturesWorkflowSPD1063", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b40190439"), DisplayName = "Collect Signatures Workflow - SharePoint 2013  (hi-in)", InternalName = "SignaturesWorkflowSPD1081", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b40190804"), DisplayName = "Collect Signatures Workflow - SharePoint 2013  (zh-CN)", InternalName = "SignaturesWorkflowSPD2052", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b40190816"), DisplayName = "Collect Signatures Workflow - SharePoint 2013  (pt-PT)", InternalName = "SignaturesWorkflowSPD2070", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b40190c0a"), DisplayName = "Collect Signatures Workflow - SharePoint 2013  (es-ES)", InternalName = "SignaturesWorkflowSPD3082", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("98d11606-9a9b-4f44-b4c2-72d72f867da9"), DisplayName = "Site Assets", InternalName = "SiteAssets", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("15a572c6-e545-4d32-897a-bab6f5846e18"), DisplayName = "Site Feed", InternalName = "SiteFeed", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("5153156a-63af-4fac-b557-91bd8c315432"), DisplayName = "Site Feed Feature Controller", InternalName = "SiteFeedController", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("6301cbb8-9396-45d1-811a-757567d35e91"), DisplayName = "Site Feed Feature Stapling", InternalName = "SiteFeedStapling", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("57ff23fc-ec05-4dd8-b7ed-d93faa7c795d"), DisplayName = "Custom Site Collection Help", InternalName = "SiteHelp", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("f151bb39-7c3b-414f-bb36-6bf18872052f"), DisplayName = "Site Notebook", InternalName = "SiteNotebook", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("b21c5a20-095f-4de2-8935-5efde5110ab3"), DisplayName = "Site Services Addins", InternalName = "SiteServicesAddins", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("fead7313-4b9e-4632-80a2-98a2a2d83297"), DisplayName = "Standard Site Settings Links", InternalName = "SiteSettings", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("a311bf68-c990-4da3-89b3-88989a3d7721"), DisplayName = "Sites List creation feature", InternalName = "SitesList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("001f4bd7-746d-403b-aa09-a6cc43de7999"), DisplayName = "Site status bar", InternalName = "SiteStatusBar", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("b63ef52c-1e99-455f-8511-6a706567740f"), DisplayName = "Site Upgrade Links", InternalName = "SiteUpgrade", Hidden = true, IsCustomDefinition = false, Scope = Scope.WebApplication });
            _features.Add(new Feature() { Id = new Guid("937f97e9-d7b4-473d-af17-b03951b2c66b"), DisplayName = "Sku Upgrade Links", InternalName = "SkuUpgradeLinks", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("0be49fe9-9bc9-409d-abf9-702753bd878d"), DisplayName = "Slide Library", InternalName = "SlideLibrary", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("65d96c6b-649a-4169-bf1d-b96505c60375"), DisplayName = "Slide Library Activation", InternalName = "SlideLibraryActivation", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("48c33d5d-acff-4400-a684-351c2beda865"), DisplayName = "Small Business Website", InternalName = "SmallBusinessWebsite", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("fa8379c9-791a-4fb0-812e-d0cfcac809c8"), DisplayName = "Social Data Storage", InternalName = "SocialDataStore", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("756d8a58-4e24-4288-b981-65dc93f9c4e5"), DisplayName = "Social Tags and Note Board Ribbon Controls", InternalName = "SocialRibbonControl", Hidden = false, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("4326e7fc-f35a-4b0f-927c-36264b0a4cf0"), DisplayName = "Community Site Infrastructure", InternalName = "SocialSite", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("abf42bbb-cd9b-4313-803b-6f4a7bd4898f"), DisplayName = "Upload App Analytics Job", InternalName = "SPAppAnalyticsUploaderJob", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("612d671e-f53d-4701-96da-c3a4ee00fdc5"), DisplayName = "Spell Checking", InternalName = "SpellChecking", Hidden = false, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("d97ded76-7647-4b1e-b868-2af51872e1b3"), DisplayName = "Blog Notifications Feature", InternalName = "SPSBlog", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("6d503bb6-027e-44ea-b54c-a53eac3dfed8"), DisplayName = "$Resources:spscore,SPSBlogStaplingFeature_Title;", InternalName = "SPSBlogStapling", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("713a65a1-2bc7-4e62-9446-1d0b56a8bf7f"), DisplayName = "Portal DiscoPage Feature", InternalName = "SPSDisco", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("2ac1da39-c101-475c-8601-122bc36e3d67"), DisplayName = "Microsoft SharePoint Foundation Search feature", InternalName = "SPSearchFeature", Hidden = true, IsCustomDefinition = false, Scope = Scope.WebApplication });
            _features.Add(new Feature() { Id = new Guid("c43a587e-195b-4d29-aba8-ebb22b48eb1a"), DisplayName = "User Profile Administration Links", InternalName = "SRPProfileAdmin", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("35f680d4-b0de-4818-8373-ee0fca092526"), DisplayName = "Secure Store Service Admin", InternalName = "SSSvcAdmin", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("ee21b29b-b0d0-42c6-baff-c97fd91786e6"), DisplayName = "Office Workflows", InternalName = "StapledWorkflows", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("00bfea71-eb8a-40b1-80c7-506be7590102"), DisplayName = "Surveys Lists", InternalName = "SurveysList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("ff13819a-a9ac-46fb-8163-9d53357ef98d"), DisplayName = "TaskListNewsFeed", InternalName = "TaskListNewsFeed", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("00bfea71-a83e-497e-9ba0-7a5c597d0107"), DisplayName = "Tasks Lists", InternalName = "TasksList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("415780bf-f710-4e2c-b7b0-b463c7992ef0"), DisplayName = "Taxonomy feature stapler", InternalName = "TaxonomyFeatureStapler", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("73ef14b1-13a9-416b-a9b5-ececa2b0604c"), DisplayName = "Register taxonomy site wide field added event receiver", InternalName = "TaxonomyFieldAdded", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("7d12c4c3-2321-42e8-8fb6-5295a849ed08"), DisplayName = "Taxonomy Tenant Administration", InternalName = "TaxonomyTenantAdmin", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("8fb893d6-93ee-4763-a046-54f9e640368d"), DisplayName = "Taxonomy Tenant Administration Stapler", InternalName = "TaxonomyTenantAdminStapler", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("48ac883d-e32e-4fd6-8499-3408add91b53"), DisplayName = "Create the taxonomy timer jobs", InternalName = "TaxonomyTimerJobs", Hidden = true, IsCustomDefinition = false, Scope = Scope.WebApplication });
            _features.Add(new Feature() { Id = new Guid("00bfea71-4ea5-48d4-a4ad-7ea5c011abe5"), DisplayName = "Team Collaboration Lists", InternalName = "TeamCollab", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("ff48f7e6-2fa1-428d-9a15-ab154762043d"), DisplayName = "Connect to Office Ribbon Controls", InternalName = "TemplateDiscovery", Hidden = false, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("0a0b2e8f-e48e-4367-923b-33bb86c1b398"), DisplayName = "Tenant Business Data Connectivity Administration", InternalName = "TenantAdminBDC", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("b5d169c9-12db-4084-b68d-eef9273bd898"), DisplayName = "Tenant Business Data Connectivity Administration Stapling", InternalName = "TenantAdminBDCStapling", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("99f380b4-e1aa-4db0-92a4-32b15e35b317"), DisplayName = "Tenant Administration Content Deployment Configuration", InternalName = "TenantAdminDeploymentLinks", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("98311581-29c5-40e8-9347-bd5732f0cb3e"), DisplayName = "Tenant Administration Links", InternalName = "TenantAdminLinks", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("b738400a-f08a-443d-96fa-a852d0356bba"), DisplayName = "$Resources:obacore,tenantadminbdcFeatureTitle;", InternalName = "TenantAdminSecureStore", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("6361e2a8-3bc4-4ca4-abbb-3dfbb727acd7"), DisplayName = "Secure Store Service Stapling Feature", InternalName = "TenantAdminSecureStoreStapling", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("32ff5455-8967-469a-b486-f8eaf0d902f9"), DisplayName = "Tenant User Profile Application", InternalName = "TenantProfileAdmin", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("3d4ea296-0b35-4a08-b2bf-f0a8cabd1d7f"), DisplayName = "Tenant User Profile Application Stapling", InternalName = "TenantProfileAdminStapling", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("983521d7-9c04-4db0-abdc-f7078fc0b040"), DisplayName = "Tenant Search Administration", InternalName = "TenantSearchAdmin", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("08ee8de1-8135-4ef9-87cb-a4944f542ba3"), DisplayName = "$Resources:TenantAdmin_SearchAdminFeatureStapling_Title;", InternalName = "TenantSearchAdminStapling", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("d5191a77-fa2d-4801-9baf-9f4205c9e9d2"), DisplayName = "Time Card List", InternalName = "TimeCardList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("742d4c0e-303b-41d7-8015-aad1dfd54cbd"), DisplayName = "Topic Page Layouts", InternalName = "TopicPageLayouts", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("5ebe1445-5910-4c6e-ac27-da2e93b60f48"), DisplayName = "Topic Pages", InternalName = "TopicPages", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("4e7276bc-e7ab-4951-9c4b-a74d44205c32"), DisplayName = "Translation", InternalName = "Translation", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("d085b8dc-9205-48a4-96ea-b40782abba02"), DisplayName = "Translation Timer Jobs", InternalName = "TranslationTimerJobs", Hidden = true, IsCustomDefinition = false, Scope = Scope.WebApplication });
            _features.Add(new Feature() { Id = new Guid("c6561405-ea03-40a9-a57f-f25472942a22"), DisplayName = "Translation Management Workflow", InternalName = "TranslationWorkflow", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("82e2ea42-39e2-4b27-8631-ed54c1cfc491"), DisplayName = "Translation Management Library", InternalName = "TransMgmtFunc", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("29d85c25-170c-4df9-a641-12db0b9d4130"), DisplayName = "Translation Management Library", InternalName = "TransMgmtLib", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("5709886f-13cc-4ffc-bfdc-ec8ab7f77191"), DisplayName = "SharePoint Server to Server Authentication", InternalName = "UPAClaimProvider", Hidden = false, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("2fa4db13-4109-4a1d-b47c-c7991d4cc934"), DisplayName = "$Resources:UpgradeOnlyFile_Feature_Title;", InternalName = "UpgradeOnlyFile", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("f0deabbb-b0f6-46ba-8e16-ff3b44461aeb"), DisplayName = "Shared Service Provider User Migrator", InternalName = "UserMigrator", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("0867298a-70e0-425f-85df-7f8bd9e06f15"), DisplayName = "User Profile User Settings Provider", InternalName = "UserProfileUserSettingsProvider", Hidden = false, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("f63b7696-9afc-4e51-9dfd-3111015e9a60"), DisplayName = "V2V Published Links Upgrade", InternalName = "V2VPublishedLinks", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("2fbbe552-72ac-11dc-8314-0800200c9a66"), DisplayName = "V2V Publishing Layouts Upgrade", InternalName = "V2VPublishingLayouts", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("6e1e5426-2ebd-4871-8027-c5ca86371ead"), DisplayName = "Video and Rich Media", InternalName = "VideoAndRichMedia", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("7c637b23-06c4-472d-9a9a-7c175762c5c4"), DisplayName = "Limited-access user permission lockdown mode", InternalName = "ViewFormPagesLockDown", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("7e0aabee-b92b-4368-8742-21ab16453d01"), DisplayName = "Visio Process Repository", InternalName = "VisioProcessRepository", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("12e4f16b-8b04-42d2-90f2-aef1cc0b65d9"), DisplayName = "Visio Process Repository 2013 Content Types", InternalName = "VisioProcessRepositoryContentTypes", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("b1f70691-6170-4cae-bc2e-4f7011a74faa"), DisplayName = "Visio Process Repository 2013 Content Types", InternalName = "VisioProcessRepositoryContentTypesUs", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("7e0aabee-b92b-4368-8742-21ab16453d00"), DisplayName = "Visio Process Repository", InternalName = "VisioProcessRepositoryFeatureStapling", Hidden = false, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("7e0aabee-b92b-4368-8742-21ab16453d02"), DisplayName = "Visio Process Repository", InternalName = "VisioProcessRepositoryUs", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("5fe8e789-d1b7-44b3-b634-419c531cfdca"), DisplayName = "Visio Web Access", InternalName = "VisioServer", Hidden = false, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("9fec40ea-a949-407d-be09-6cba26470a0c"), DisplayName = "Visio Web Access", InternalName = "VisioWebAccess", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("af6d9aec-7c38-4dda-997f-cc1ddbb87c92"), DisplayName = "Web Analytics Customize Reports functionality", InternalName = "WACustomReports", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("c04234f4-13b8-4462-9108-b4f5159beae6"), DisplayName = "Advanced Web Analytics", InternalName = "WAMaster", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("2acf27a5-f703-4277-9f5d-24d70110b18b"), DisplayName = "Advanced Web Analytics Reports", InternalName = "WAReports", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("8e947bf0-fe40-4dff-be3d-a8b88112ade6"), DisplayName = "Web Analytics Web Part", InternalName = "WAWhatsPopularWebPart", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("00bfea71-c796-4402-9f2f-0eb9a6e71b18"), DisplayName = "Wiki Page Library", InternalName = "WebPageLibrary", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("2ed1c45e-a73b-4779-ae81-1524e4de467a"), DisplayName = "Web Part Adder default groups", InternalName = "WebPartAdderGroups", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("d7670c9c-1c29-4f44-8691-584001968a74"), DisplayName = "What's New List", InternalName = "WhatsNewList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("9c2ef9dc-f733-432e-be1c-2e79957ea27b"), DisplayName = "$Resources:core,GbwFeatureWhereaboutsTitle;", InternalName = "WhereaboutsList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("00bfea71-d8fe-4fec-8dad-01c19a6e4053"), DisplayName = "Wiki Page Home Page", InternalName = "WikiPageHomePage", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("8c6a6980-c3d9-440e-944c-77f93bc65a7e"), DisplayName = "WikiWelcome", InternalName = "WikiWelcome", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("1663ee19-e6ab-4d47-be1b-adeb27cfd9d2"), DisplayName = "Deprecated Office Web Apps", InternalName = "WordServerViewing", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("ec918931-c874-4033-bd09-4f36b2e31fef"), DisplayName = "Workflows can use app permissions", InternalName = "WorkflowAppOnlyPolicyManager", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("00bfea71-4ea5-48d4-a4ad-305cf7030140"), DisplayName = "Workflow History Lists", InternalName = "WorkflowHistoryList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("00bfea71-2d77-4a75-9fca-76516689e21a"), DisplayName = "WorkflowProcessList Feature", InternalName = "workflowProcessList", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("0af5989a-3aea-4519-8ab0-85d91abe39ff"), DisplayName = "Workflows", InternalName = "Workflows", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("8b82e40f-2001-4f0e-9ce3-0b27d1866dff"), DisplayName = "workflow service stapler", InternalName = "WorkflowServiceStapler", Hidden = true, IsCustomDefinition = false, Scope = Scope.Farm });
            _features.Add(new Feature() { Id = new Guid("2c63df2b-ceab-42c6-aeff-b3968162d4b1"), DisplayName = "workflow service store", InternalName = "WorkflowServiceStore", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("57311b7a-9afd-4ff0-866e-9393ad6647b1"), DisplayName = "Workflow Task Content Type", InternalName = "WorkflowTask", Hidden = false, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("00bfea71-1e1d-4562-b56a-f05371bb0115"), DisplayName = "XML Form Libraries", InternalName = "XmlFormLibrary", Hidden = true, IsCustomDefinition = false, Scope = Scope.Web });
            _features.Add(new Feature() { Id = new Guid("77fc9e13-e99a-4bd3-9438-a3f69670ed97"), DisplayName = "Search Engine Sitemap", InternalName = "XmlSitemap", Hidden = false, IsCustomDefinition = false, Scope = Scope.Site });
            _features.Add(new Feature() { Id = new Guid("3f6680ba-94db-4c92-a5b6-7d5c66f467a7"), DisplayName = "Enterprise Wiki", InternalName = "EnterpriseWikiSecondPhase", Hidden = true, IsCustomDefinition = false, Scope = Scope.Site });
        }
    }
}
