using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPBrowser
{
    public static class SPHelp
    {
        private const string MSDN_HELP_LINK = "http://msdn.microsoft.com/en-us/library/{0}(v=office.14).aspx";

        private static List<SPFeature> _features;

        static SPHelp()
        {
            InitFeatures();
        }

        /// <summary>
        /// Gets the "Microsoft.SharePoint.Client" namespace MSDN help link.
        /// </summary>
        /// <returns>Returns the link to MSDN for "Microsoft.SharePoint.Client" namespace.</returns>
        public static Uri GetMSDNHelpLink()
        {
            return new Uri(string.Format(MSDN_HELP_LINK, "Microsoft.SharePoint.Client"));
        }

        /// <summary>
        /// Gets the MSDN help link based on the provided node.
        /// </summary>
        /// <param name="node"><see cref="TreeNode"/> which contains the object to show MSDN help for.</param>
        /// <returns>Returns the link to MSDN for "Microsoft.SharePoint.Client" namespace.</returns>
        public static Uri GetMSDNHelpLink(System.Windows.Forms.TreeNode node)
        {
            Uri helpLink = null;

            if (node != null && node.Tag != null && !(node.Tag is SPBrowser.LoadType))
                helpLink = new Uri(string.Format(MSDN_HELP_LINK, node.Tag.GetType().ToString()));

            return helpLink;
        }

        public static string GetFileName(string path)
        {
            string filename = System.IO.Path.GetFileName(path);

            if (filename.Contains('?'))
                filename = filename.Remove(System.IO.Path.GetFileName(path).IndexOf('?'));

            return filename;
        }

        public static SPFeature GetFeature(Guid featureId)
        {
            return GetFeature(featureId.ToString());
        }

        public static SPFeature GetFeature(string featureId)
        { 
            // Try to retrieve from internal feature collection
            SPFeature feature = _features.SingleOrDefault(f => f.Id.ToString().Equals(featureId, StringComparison.InvariantCultureIgnoreCase));

            // If not found internally, then try retrieving it from the custom feature definition source.
            if (feature == null)
                feature = Globals.CustomFeatureDefinitions.SingleOrDefault(f => f.Id.ToString().Equals(featureId, StringComparison.InvariantCultureIgnoreCase));

            return feature;
        }

        private static void InitFeatures()
        {
            _features = new List<SPFeature>();
            _features.Add(new SPFeature() { Id = new Guid("001f4bd7-746d-403b-aa09-a6cc43de7942"), DisplayName = "Publishing Features Stapling", InternalName = "PublishingStapling", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("00bfea71-1c5e-4a24-b310-ba51c3eb7a57"), DisplayName = "Basic Web Parts", InternalName = "BasicWebParts", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("00bfea71-1e1d-4562-b56a-f05371bb0115"), DisplayName = "XML Form Libraries", InternalName = "XmlFormLibrary", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("00bfea71-2062-426c-90bf-714c59600103"), DisplayName = "Links Lists", InternalName = "LinksList", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("00bfea71-2d77-4a75-9fca-76516689e21a"), DisplayName = "WorkflowProcessList Feature", InternalName = "workflowProcessList", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("00bfea71-3a1d-41d3-a0ee-651d11570120"), DisplayName = "Grid Lists", InternalName = "GridList", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("00bfea71-4ea5-48d4-a4ad-305cf7030140"), DisplayName = "Workflow History Lists", InternalName = "WorkflowHistoryList", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("00bfea71-4ea5-48d4-a4ad-7ea5c011abe5"), DisplayName = "Team Collaboration Lists", InternalName = "TeamCollab", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("00bfea71-513d-4ca0-96c2-6a47775c0119"), DisplayName = "Gantt Chart Tasks Lists", InternalName = "GanttTasksList", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("00bfea71-52d4-45b3-b544-b1c71b620109"), DisplayName = "Picture Libraries", InternalName = "PictureLibrary", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("00bfea71-5932-4f9c-ad71-1557e5751100"), DisplayName = "Issues Lists", InternalName = "IssuesList", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("00bfea71-6a49-43fa-b535-d15c05500108"), DisplayName = "Discussion Lists", InternalName = "DiscussionsList", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("00bfea71-7e6d-4186-9ba8-c047ac750105"), DisplayName = "Contacts Lists", InternalName = "ContactsList", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("00bfea71-9549-43f8-b978-e47e54a10600"), DisplayName = "External Lists", InternalName = "ExternalList", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("00bfea71-a83e-497e-9ba0-7a5c597d0107"), DisplayName = "Tasks Lists", InternalName = "TasksList", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("00bfea71-c796-4402-9f2f-0eb9a6e71b18"), DisplayName = "Wiki Page Library", InternalName = "WebPageLibrary", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("00bfea71-d1ce-42de-9c63-a44004ce0104"), DisplayName = "Announcements Lists", InternalName = "AnnouncementsList", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("00bfea71-d8fe-4fec-8dad-01c19a6e4053"), DisplayName = "Wiki Page Home Page", InternalName = "WikiPageHomePage", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("00bfea71-dbd7-4f72-b8cb-da7ac0440130"), DisplayName = "Data Connections Feature", InternalName = "DataConnectionLibrary", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("00bfea71-de22-43b2-a848-c05709900100"), DisplayName = "Custom Lists", InternalName = "CustomList", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("00bfea71-e717-4e80-aa17-d0c71b360101"), DisplayName = "Document Libraries", InternalName = "DocumentLibrary", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("00bfea71-eb8a-40b1-80c7-506be7590102"), DisplayName = "Surveys Lists", InternalName = "SurveysList", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("00bfea71-ec85-4903-972d-ebe475780106"), DisplayName = "Events Lists", InternalName = "EventsList", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("00bfea71-f381-423d-b9d1-da7a54c50110"), DisplayName = "Data Source Libraries", InternalName = "DataSourceLibrary", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("00bfea71-f600-43f6-a895-40c0de7b0117"), DisplayName = "No-code Workflow Libraries", InternalName = "NoCodeWorkflowLibrary", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("0125140f-7123-4657-b70a-db9aa1f209e5"), DisplayName = "Feature Pushdown Links", InternalName = "FeaturePushdown", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("02464c6a-9d07-4f30-ba04-e9035cf54392"), DisplayName = "Routing Workflows", InternalName = "ReviewWorkflows", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("034947cc-c424-47cd-a8d1-6014f0e36925"), DisplayName = "My Site Layouts Feature", InternalName = "MySiteQuickLaunch", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("03b0a3dc-93dd-4c68-943e-7ec56e65ed4d"), DisplayName = "$Resources:SearchEndUserHelp_Feature_Title;", InternalName = "OSSSearchEndUserHelpFeature", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("0561d315-d5db-4736-929e-26da142812c5"), DisplayName = "Nintex Workflow 2010", InternalName = "NintexWorkflow", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("05891451-f0c4-4d4e-81b1-0dabd840bad4"), DisplayName = "PerformancePoint Datasource Content Type definition", InternalName = "PPSMonDatasourceCtype", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("063c26fa-3ccc-4180-8a84-b6f98e991df3"), DisplayName = "Library and Folder Based Retention", InternalName = "LocationBasedPolicy", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("065c78be-5231-477e-a972-14177cc5b3c7"), DisplayName = "SharePoint Portal Server Status Indicator List template", InternalName = "BizAppsListTemplates", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("068bc832-4951-11dc-8314-0800200c9a66"), DisplayName = "Enhanced Theming", InternalName = "EnhancedTheming", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("068f8656-bea6-4d60-a5fa-7f077f8f5c20"), DisplayName = "Shared Services Administration Links", InternalName = "OsrvLinks", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("071de60d-4b02-4076-b001-b456e93146fe"), DisplayName = "Help", InternalName = "HelpLibrary", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("0806d127-06e6-447a-980e-2e90b03101b8"), DisplayName = "SharePoint Server Enterprise Site features", InternalName = "PremiumWeb", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("08386d3d-7cc0-486b-a730-3b4cfe1b5509"), DisplayName = "Manage Resources", InternalName = "FCGroupsList", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("095f7b90-f808-40a8-8e41-1483906e8fae"), DisplayName = "Excel Services Application Edit Farm Feature", InternalName = "ExcelServerEditStapler", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("09fe98f3-3324-4747-97e5-916a28a0c6c0"), DisplayName = "Health Reports Pushdown Feature", InternalName = "OSearchHealthReportsPushdown", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("0a0b2e8f-e48e-4367-923b-33bb86c1b398"), DisplayName = "Tenant Business Data Connectivity Administration", InternalName = "TenantAdminBDC", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("0af5989a-3aea-4519-8ab0-85d91abe39ff"), DisplayName = "Workflows", InternalName = "Workflows", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("0b07a7f4-8bb8-4ec0-a31b-115732b9584d"), DisplayName = "PerformancePoint Services Site Features", InternalName = "PPSSiteMaster", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("0be49fe9-9bc9-409d-abf9-702753bd878d"), DisplayName = "Slide Library", InternalName = "SlideLibrary", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("0c504a5c-bcea-4376-b05e-cbca5ced7b4f"), DisplayName = "Office Web Apps", InternalName = "OfficeWebApps", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("0c8a9a47-22a9-4798-82f1-00e62a96006e"), DisplayName = "Document Routing Resources", InternalName = "DocumentRoutingResources", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("0ea1c3b6-6ac0-44aa-9f3f-05e8dbe6d70b"), DisplayName = "SharePoint Server Enterprise Web application features", InternalName = "PremiumWebApplication", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("0f121a23-c6bc-400f-87e4-e6bbddf6916d"), DisplayName = "Standard User Interface Items", InternalName = "ContentLightup", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("0faf7d1b-95b1-4053-b4e2-19fd5c9bbc88"), DisplayName = "My Site Cleanup Feature", InternalName = "MySiteCleanup", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("10bdac29-a21a-47d9-9dff-90c7cae1301e"), DisplayName = "Shared Services Navigation", InternalName = "OssNavigation", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("14173c38-5e2d-4887-8134-60f9df889bad"), DisplayName = "Document to Page Converters", InternalName = "PageConverters", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("14aafd3a-fcb9-4bb7-9ad7-d8e36b663bbd"), DisplayName = "SharePoint Portal Server Local Site Directory Capture Control", InternalName = "LocalSiteDirectoryControl", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("15845762-4ec4-4606-8993-1c0512a98680"), DisplayName = "InfoPath Forms Services Tenant Administration", InternalName = "IPFSTenantFormsConfig", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("1663ee19-e6ab-4d47-be1b-adeb27cfd9d2"), DisplayName = "Word Web App", InternalName = "WordViewer", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0409"), DisplayName = "Publishing Workflow - SharePoint 2010 (en-US)", InternalName = "ReviewPublishingSPD1033", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("19f5f68e-1b92-4a02-b04d-61810ead0413"), DisplayName = "Publishing Workflow - SharePoint 2010 (nl-nl)", InternalName = "ReviewPublishingSPD1043", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("1a8251a0-47ab-453d-95d4-07d7ca4f8166"), DisplayName = "Access Services User Templates", InternalName = "AccSrvUserTemplate", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("1c6a572c-1b58-49ab-b5db-75caf50692e6"), DisplayName = "Microsoft IME Dictionary List", InternalName = "IMEDicList", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("1cc4b32c-299b-41aa-9770-67715ea05f25"), DisplayName = "Access Services Farm Feature", InternalName = "AccSrvApplication", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("1dbf6063-d809-45ea-9203-d3ba4a64f86d"), DisplayName = "Search And Process", InternalName = "SearchAndProcess", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("1ec2c859-e9cb-4d79-9b2b-ea8df09ede22"), DisplayName = "DM Content Type Setting Links", InternalName = "DMContentTypeSettings", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("20477d83-8bdb-414e-964b-080637f7d99b"), DisplayName = "Publishing Timer Jobs", InternalName = "PublishingTimerJobs", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("22a9ef51-737b-4ff2-9346-694633fe4416"), DisplayName = "Publishing", InternalName = "Publishing", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("239650e3-ee0b-44a0-a22a-48292402b8d8"), DisplayName = "Phone Call Memo List", InternalName = "CallTrackList", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("2510d73f-7109-4ccc-8a1c-314894deeb3a"), DisplayName = "SharePoint Portal Server Report Library", InternalName = "ReportListTemplate", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("26676156-91a0-49f7-87aa-37b1d5f0c4d0"), DisplayName = "DataConnections Library for PerformancePoint", InternalName = "BICenterDataconnectionsLib", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("28101b19-b896-44f4-9264-db028f307a62"), DisplayName = "Access Services User Application Log", InternalName = "AccSrvUSysAppLog", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("29d85c25-170c-4df9-a641-12db0b9d4130"), DisplayName = "Translation Management Library", InternalName = "TransMgmtLib", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("29e9a673-31a4-46a3-b0d2-d8e1db1dbd92"), DisplayName = "NintexLiveAdminLinks", InternalName = "NintexLiveAdminLinks", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("29ea7495-fca1-4dc6-8ac1-500c247a036e"), DisplayName = "Access Services System Objects", InternalName = "AccSrvMSysAso", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("2ac1da39-c101-475c-8601-122bc36e3d67"), DisplayName = "Microsoft SharePoint Foundation Search feature", InternalName = "SPSearchFeature", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("2acf27a5-f703-4277-9f5d-24d70110b18b"), DisplayName = "Advanced Web Analytics Reports", InternalName = "WAReports", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("2ed1c45e-a73b-4779-ae81-1524e4de467a"), DisplayName = "Web Part Adder default groups", InternalName = "WebPartAdderGroups", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("2fa4db13-4109-4a1d-b47c-c7991d4cc934"), DisplayName = "$Resources:UpgradeOnlyFile_Feature_Title;", InternalName = "UpgradeOnlyFile", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("2fb9d5df-2fb5-403d-b155-535c256be1dc"), DisplayName = "Nintex Workflow 2010 Enterprise Reporting", InternalName = "NintexWorkflowEnterpriseWeb", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("2fbbe552-72ac-11dc-8314-0800200c9a66"), DisplayName = "V2V Publishing Layouts Upgrade", InternalName = "V2VPublishingLayouts", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("306936fd-9806-4478-80d1-7e397bfa6474"), DisplayName = "SharePoint Portal Server Redirect Page Content Type Binding Feature", InternalName = "RedirectPageContentTypeBinding", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("30a6403b-b04f-42cc-805a-bc4d77826253"), DisplayName = "PowerPoint Broadcast", InternalName = "PowerPointBroadcastServer", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("319d8f70-eb3a-4b44-9c79-2087a87799d6"), DisplayName = "Global Web Parts", InternalName = "GlobalWebParts", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("32ff5455-8967-469a-b486-f8eaf0d902f9"), DisplayName = "Tenant User Profile Application", InternalName = "TenantProfileAdmin", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("34339dc9-dec4-4256-b44a-b30ff2991a64"), DisplayName = "Content type syndication", InternalName = "ContentTypeSyndication", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("35f680d4-b0de-4818-8373-ee0fca092526"), DisplayName = "Secure Store Service Admin", InternalName = "SSSvcAdmin", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("38969baa-3590-4635-81a4-2049d982adfa"), DisplayName = "FAST Search Central Admin Help Collection", InternalName = "FastCentralAdminHelpCollection", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("3992d4ab-fa9e-4791-9158-5ee32178e88a"), DisplayName = "Business Intelligence center sample data", InternalName = "BICenterSampleData", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("39dd29fb-b6f5-4697-b526-4d38de4893e5"), DisplayName = "Meetings Workspaces Web Parts", InternalName = "MpsWebParts", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("3a4ce811-6fe0-4e97-a6ae-675470282cf2"), DisplayName = "Document Sets metadata synchronization", InternalName = "DocumentManagement", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("3bae86a2-776d-499d-9db8-fa4cdc7884f8"), DisplayName = "Document Sets", InternalName = "DocumentSet", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b60409"), DisplayName = "Routing Workflows - SharePoint 2010 (en-US)", InternalName = "ReviewWorkflowsSPD1033", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("3bc0c1e1-b7d5-4e82-afd7-9f7e59b60413"), DisplayName = "Routing Workflows - SharePoint 2010 (nl-nl)", InternalName = "ReviewWorkflowsSPD1043", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("3c577815-7658-4d4f-a347-cfbb370700a7"), DisplayName = "InfoPath Forms Services Web Service Proxy Administration", InternalName = "IPFSTenantWebProxyConfig", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("3cb475e7-4e87-45eb-a1f3-db96ad7cf313"), DisplayName = "Excel Services Application View Site Feature", InternalName = "ExcelServerSite", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("3ce24023-95a1-4778-85b0-8e9b2bcacc80"), DisplayName = "Web Analytics Central Admin Customize Reports", InternalName = "WACentralAdminCustomReports", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("3d25bd73-7cd4-4425-b8fb-8899977f73de"), DisplayName = "GroupBoardWebParts", InternalName = "GBWWebParts", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("3d433d02-cf49-4975-81b4-aede31e16edf"), DisplayName = "OneNote Web App", InternalName = "OneNote", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("3d4ea296-0b35-4a08-b2bf-f0a8cabd1d7f"), DisplayName = "Tenant User Profile Application Stapling", InternalName = "TenantProfileAdminStapling", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("3f59333f-4ce1-406d-8a97-9ecb0ff0337f"), DisplayName = "Document Center Enhancements", InternalName = "BDR", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("412c7903-14f8-480a-8e98-3c5817906f70"), DisplayName = "PowerPoint Broadcast Visibility", InternalName = "PowerPointBroadcastVisibility", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("415780bf-f710-4e2c-b7b0-b463c7992ef0"), DisplayName = "Taxonomy feature stapler", InternalName = "TaxonomyFeatureStapler", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("4248e21f-a816-4c88-8cab-79d82201da7b"), DisplayName = "BizApps Site Templates", InternalName = "BizAppsSiteTemplates", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("43f41342-1a37-4372-8ca0-b44d881e4434"), DisplayName = "SharePoint Portal Server Business Appications Content Type Definition", InternalName = "BizAppsCTypes", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("4750c984-7721-4feb-be61-c660c6190d43"), DisplayName = "SharePoint Server Enterprise Search", InternalName = "OSearchEnhancedFeature", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("481333e1-a246-4d89-afab-d18c6fe344ce"), DisplayName = "PerformancePoint Content List", InternalName = "PPSWorkspaceList", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("485f5158-4b8a-453f-9eeb-7b33f5112adf"), DisplayName = "\"Nintex Live (Central Admin)\"", InternalName = "NintexWorkflowLiveAdminWeb", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("48ac883d-e32e-4fd6-8499-3408add91b53"), DisplayName = "Create the taxonomy timer jobs", InternalName = "TaxonomyTimerJobs", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("49571cd1-b6a1-43a3-bf75-955acc79c8d8"), DisplayName = "My Site Host", InternalName = "MySiteHost", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("4bcccd62-dcaf-46dc-a7d4-e38277ef33f4"), DisplayName = "Asset Library", InternalName = "AssetLibrary", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("4c42ab64-55af-4c7c-986a-ac216a6e0c0e"), DisplayName = "Excel Services Application Web Part Site Feature", InternalName = "ExcelServerWebPart", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("4f56f9fa-51a0-420c-b707-63ecbb494db1"), DisplayName = "SharePoint Server Standard Web application features", InternalName = "BaseWebApplication", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("53164b55-e60f-4bed-b582-a87da32b92f1"), DisplayName = "Nintex Workflow 2010 Reporting Web Parts", InternalName = "NintexWorkflowEnterpriseWebParts", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("541f5f57-c847-4e16-b59a-b31e90e6f9ea"), DisplayName = "Portal Navigation Properties", InternalName = "NavigationProperties", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("54668547-c03f-4bb5-aaab-d9568ebaf9c9"), DisplayName = "Nintex Workflow - Nintex Live Catalog", InternalName = "NintexWorkflowLiveSite", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("55312854-855b-4088-b09d-c5efe0fbf9d2"), DisplayName = "Administrative Reporting Core Pushdown Feature", InternalName = "AdminReportCorePushdown", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("5709298b-1876-4686-b257-f101a923f58d"), DisplayName = "PowerPoint Viewing", InternalName = "PowerPointServer", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("57ff23fc-ec05-4dd8-b7ed-d93faa7c795d"), DisplayName = "Custom Site Collection Help", InternalName = "SiteHelp", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("58160a6b-4396-4d6e-867c-65381fb5fbc9"), DisplayName = "Resources List", InternalName = "FacilityList", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("5a020a4f-c449-4a65-b07d-f2cc2d8778dd"), DisplayName = "Excel Mobile Viewer Feature", InternalName = "MobileEwaFarm", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("5a979115-6b71-45a5-9881-cdc872051a69"), DisplayName = "SPS Biz Apps Field Definition", InternalName = "BizAppsFields", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("5bccb9a4-b903-4fd1-8620-b795fa33c9ba"), DisplayName = "Record Resources", InternalName = "RecordResources", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("5d220570-df17-405e-b42d-994237d60ebf"), DisplayName = "PerformancePoint Data Source Library Template", InternalName = "PPSDatasourceLib", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("5eac763d-fbf5-4d6f-a76b-eded7dd7b0a5"), DisplayName = "Search extensions", InternalName = "SearchExtensions", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("5ede0a86-c772-4f1d-a120-72e734b3400c"), DisplayName = "Shared Picture Library for Organizations logos", InternalName = "MySiteHostPictureLibrary", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("5f2e3537-91b5-4341-86ff-90c6a2f99aae"), DisplayName = "Report Server Central Administration Feature", InternalName = "ReportServerCentralAdmin", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("5f3b0127-2f1d-4cfd-8dd2-85ad1fb00bfc"), DisplayName = "Portal Layouts Feature", InternalName = "PortalLayouts", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("5fe8e789-d1b7-44b3-b634-419c531cfdca"), DisplayName = "Visio Web Access", InternalName = "VisioServer", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("612d671e-f53d-4701-96da-c3a4ee00fdc5"), DisplayName = "Spell Checking", InternalName = "SpellChecking", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("6361e2a8-3bc4-4ca4-abbb-3dfbb727acd7"), DisplayName = "Secure Store Service Stapling Feature", InternalName = "TenantAdminSecureStoreStapling", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("636287a7-7f62-4a6e-9fcc-081f4672cbf8"), DisplayName = "Schedule and Reservations List", InternalName = "ScheduleList", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("65d96c6b-649a-4169-bf1d-b96505c60375"), DisplayName = "Slide Library Activation", InternalName = "SlideLibraryActivation", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("683df0c0-20b7-4852-87a3-378945158fab"), DisplayName = "BDC Profile Pages Feature", InternalName = "ObaProfilePages", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("6928b0e5-5707-46a1-ae16-d6e52522d52b"), DisplayName = "My Site Layouts Feature", InternalName = "MySiteLayouts", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("695b6570-a48b-4a8e-8ea5-26ea7fc1d162"), DisplayName = "Standard Content Type Definitions", InternalName = "CTypes", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("69cc9662-d373-47fc-9449-f18d11ff732c"), DisplayName = "My Site", InternalName = "MySite", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("6adff05c-d581-4c05-a6b9-920f15ec6fd9"), DisplayName = "My Site Navigation", InternalName = "MySiteNavigation", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("6bcbccc3-ff47-47d3-9468-572bf2ab9657"), DisplayName = "Report Server Integration Stapling Feature", InternalName = "ReportServerStapling", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("6c09612b-46af-4b2f-8dfc-59185c962a29"), DisplayName = "Collect Signatures Workflow", InternalName = "SignaturesWorkflow", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("6d127338-5e7d-4391-8f62-a11e43b1d404"), DisplayName = "Records Management", InternalName = "RecordsManagement", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("6e53dd27-98f2-4ae5-85a0-e9a8ef4aa6df"), DisplayName = "Document Libraries", InternalName = "LegacyDocumentLibrary", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("6e8a2add-ed09-4592-978e-8fa71e6f117c"), DisplayName = "Group Work Provisioning", InternalName = "GBWProvision", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("6e8f2b8d-d765-4e69-84ea-5702574c11d6"), DisplayName = "FAST Search End User Help Collection", InternalName = "FastEndUserHelpCollection", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("7094bd89-2cfe-490a-8c7e-fbace37b4a34"), DisplayName = "Reporting", InternalName = "Reporting", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("713a65a1-2bc7-4e62-9446-1d0b56a8bf7f"), DisplayName = "Portal DiscoPage Feature", InternalName = "SPSDisco", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("7201d6a4-a5d3-49a1-8c19-19c4bac6e668"), DisplayName = "Metadata Navigation and Filtering", InternalName = "MetaDataNav", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("738250ba-9327-4dc0-813a-a76928ba1842"), DisplayName = "PowerPoint Editing", InternalName = "PowerPointEditServer", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("73ef14b1-13a9-416b-a9b5-ececa2b0604c"), DisplayName = "Register taxonomy site wide field added event receiver", InternalName = "TaxonomyFieldAdded", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("744b5fd3-3b09-4da6-9bd1-de18315b045d"), DisplayName = "Access Services Solution Gallery", InternalName = "AccSrvSolutionGallery", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("750b8e49-5213-4816-9fa2-082900c0201a"), DisplayName = "Admin Links for InfoPath Forms Services.", InternalName = "IPFSAdminWeb", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("756d8a58-4e24-4288-b981-65dc93f9c4e5"), DisplayName = "Social Tags and Note Board Ribbon Controls", InternalName = "SocialRibbonControl", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("76d688ad-c16e-4cec-9b71-7b7f0d79b9cd"), DisplayName = "Enterprise Wiki", InternalName = "EnterpriseWiki", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("786eaa5b-85d7-4ea0-8998-0b62c8befd94"), DisplayName = "Web Analytics Central Admin Reports", InternalName = "WACentralAdminReports", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("7ac8cc56-d28e-41f5-ad04-d95109eb987a"), DisplayName = "Site collection level Search Center Url Feature", InternalName = "OSSSearchSearchCenterUrlSiteFeature", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("7acfcb9d-8e8f-4979-af7e-8aed7e95245e"), DisplayName = "Search Center URL", InternalName = "OSSSearchSearchCenterUrlFeature", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("7ad5272a-2694-4349-953e-ea5ef290e97c"), DisplayName = "Content Organizer", InternalName = "DocumentRouting", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("7c637b23-06c4-472d-9a9a-7c175762c5c4"), DisplayName = "Restrict Limited Access Permissions", InternalName = "ViewFormPagesLockDown", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("7c939ea0-196e-4759-ad06-8bc2a64ed4e5"), DisplayName = "Excel Services Application Deactivate Site Programmability Feature", InternalName = "ExcelServerDeactivateProgrammability", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("7d12c4c3-2321-42e8-8fb6-5295a849ed08"), DisplayName = "Taxonomy Tenant Administration", InternalName = "TaxonomyTenantAdmin", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("7e0aabee-b92b-4368-8742-21ab16453d00"), DisplayName = "Visio Process Repository", InternalName = "VisioProcessRepositoryFeatureStapling", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("7e0aabee-b92b-4368-8742-21ab16453d01"), DisplayName = "Visio Process Repository", InternalName = "VisioProcessRepository", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("7e0aabee-b92b-4368-8742-21ab16453d02"), DisplayName = "Visio Process Repository", InternalName = "VisioProcessRepositoryUs", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("80bf3218-7353-11df-af9f-058bdfd72085"), DisplayName = "Nintex Workflow 2010 InfoPath Forms", InternalName = "NintexWorkflowInfoPath", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("81ebc0d6-8fb2-4e3f-b2f8-062640037398"), DisplayName = "Enhanced Html Editing", InternalName = "EnhancedHtmlEditing", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("82e2ea42-39e2-4b27-8631-ed54c1cfc491"), DisplayName = "Translation Management Library", InternalName = "TransMgmtFunc", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("8472208f-5a01-4683-8119-3cea50bea072"), DisplayName = "PPS Site Stapling", InternalName = "PPSSiteStapling", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("8581a8a7-cf16-4770-ac54-260265ddb0b2"), DisplayName = "SharePoint Server Enterprise Site Collection features", InternalName = "PremiumSite", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("863da2ac-3873-4930-8498-752886210911"), DisplayName = "My Site Blogs", InternalName = "MySiteBlog", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("86c83d16-605d-41b4-bfdd-c75947899ac7"), DisplayName = "Nintex Workflow Content Type Upgrade", InternalName = "NintexWorkflowContentTypeUpgrade", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("875d1044-c0cf-4244-8865-d2a0039c2a49"), DisplayName = "Chart Web Part", InternalName = "MossChart", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("893627d9-b5ef-482d-a3bf-2a605175ac36"), DisplayName = "PowerPoint Mobile Viewer", InternalName = "MobilePowerPointViewer", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("89e0306d-453b-4ec5-8d68-42067cdbf98e"), DisplayName = "Portal Navigation", InternalName = "Navigation", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("8a4b8de2-6fd8-41e9-923c-c7c3c00f8295"), DisplayName = "Open Documents in Client Applications by Default", InternalName = "OpenInClient", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("8a663fe0-9d9c-45c7-8297-66365ad50427"), DisplayName = "SharePoint Portal Server Master Site Directory Capture Control", InternalName = "MasterSiteDirectoryControl", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("8c6a6980-c3d9-440e-944c-77f93bc65a7e"), DisplayName = "WikiWelcome", InternalName = "WikiWelcome", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("8dfaf93d-e23c-4471-9347-07368668ddaf"), DisplayName = "Word Mobile Viewer", InternalName = "MobileWordViewer", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("8e947bf0-fe40-4dff-be3d-a8b88112ade6"), DisplayName = "Web Analytics Web Part", InternalName = "WAWhatsPopularWebPart", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("8f15b342-80b1-4508-8641-0751e2b55ca6"), DisplayName = "Local Site Directory MetaData Capture Feature", InternalName = "LocalSiteDirectoryMetaData", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("8fb893d6-93ee-4763-a046-54f9e640368d"), DisplayName = "Taxonomy Tenant Administration Stapler", InternalName = "TaxonomyTenantAdminStapler", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("90c6c1e5-3719-4c52-9f36-34a97df596f7"), DisplayName = "BDC Profile Pages Tenant Stapling Feature", InternalName = "ObaProfilePagesTenantStapling", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("915c240e-a6cc-49b8-8b2c-0bff8b553ed3"), DisplayName = "Ratings", InternalName = "Ratings", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("937f97e9-d7b4-473d-af17-b03951b2c66b"), DisplayName = "Sku Upgrade Links", InternalName = "SkuUpgradeLinks", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("94c94ca6-b32f-4da9-a9e3-1f3d343d7ecb"), DisplayName = "SharePoint Server Publishing", InternalName = "PublishingWeb", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("97a2485f-ef4b-401f-9167-fa4fe177c6f6"), DisplayName = "Base Site Features Stapling", InternalName = "BaseSiteStapling", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("98311581-29c5-40e8-9347-bd5732f0cb3e"), DisplayName = "Tenant Administration Links", InternalName = "TenantAdminLinks", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("99f380b4-e1aa-4db0-92a4-32b15e35b317"), DisplayName = "Tenant Administration Content Deployment Configuration", InternalName = "TenantAdminDeploymentLinks", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("99fe402e-89a0-45aa-9163-85342e865dc8"), DisplayName = "SharePoint Server Standard Site features", InternalName = "BaseWeb", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("9a447926-5937-44cb-857a-d3829301c73b"), DisplayName = "Content Type Syndication Hub", InternalName = "ContentTypeHub", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("9ad4c2d4-443b-4a94-8534-49a23f20ba3c"), DisplayName = "Holidays List", InternalName = "HolidaysList", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("9b0293a7-8942-46b0-8b78-49d29a9edd53"), DisplayName = "Organizations Claim Hierarchy Provider", InternalName = "OrganizationsClaimHierarchyProvider", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("9bf095db-11a4-4568-b92e-e23db80a8777"), DisplayName = "Create Web Analytics Workflow Timer Job", InternalName = "WAWebApp", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("9bf7bf98-5660-498a-9399-bc656a61ed5d"), DisplayName = "Nintex Workflow 2010", InternalName = "NintexWorkflowWeb", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("9c03e124-eef7-4dc6-b5eb-86ccd207cb87"), DisplayName = "Group Work Lists", InternalName = "GroupWork", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("9c2ef9dc-f733-432e-be1c-2e79957ea27b"), DisplayName = "$Resources:core,GbwFeatureWhereaboutsTitle;", InternalName = "WhereaboutsList", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("9d46d0d4-af7b-4f2e-8f84-9466ab25766c"), DisplayName = "Web Analytics Feature Stapler", InternalName = "WAFeatureStapler", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("9e56487c-795a-4077-9425-54a1ecb84282"), DisplayName = "Hold and eDiscovery", InternalName = "Hold", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("9fec40ea-a949-407d-be09-6cba26470a0c"), DisplayName = "Visio Web Access", InternalName = "VisioWebAccess", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("a0e5a010-1329-49d4-9e09-f280cdbed37d"), DisplayName = "InfoPath Forms Services support", InternalName = "IPFSWebFeatures", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("a10b6aa4-135d-4598-88d1-8d4ff5691d13"), DisplayName = "Admin Links for InfoPath Forms Services.", InternalName = "ipfsAdminLinks", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("a140a1ac-e757-465d-94d4-2ca25ab2c662"), DisplayName = "Office.com Entry Points from SharePoint", InternalName = "DownloadFromOfficeDotCom", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("a1cb5b7f-e5e9-421b-915f-bf519b0760ef"), DisplayName = "PerformancePoint Services Site Collection Features", InternalName = "PPSSiteCollectionMaster", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("a311bf68-c990-4da3-89b3-88989a3d7721"), DisplayName = "Sites List creation feature", InternalName = "SitesList", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("a392da98-270b-4e85-9769-04c0fde267aa"), DisplayName = "Publishing Prerequisites", InternalName = "PublishingPrerequisites", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b40190409"), DisplayName = "Collect Signatures Workflow - SharePoint 2010 (en-US)", InternalName = "SignaturesWorkflowSPD1033", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("a42f749f-8633-48b7-9b22-403b40190413"), DisplayName = "Collect Signatures Workflow - SharePoint 2010 (nl-nl)", InternalName = "SignaturesWorkflowSPD1043", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("a44d2aa3-affc-4d58-8db4-f4a3af053188"), DisplayName = "Publishing Approval Workflow", InternalName = "ReviewPublishingSPD", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("a4d4ee2c-a6cb-4191-ab0a-21bb5bde92fb"), DisplayName = "Access Services Restricted List Definition", InternalName = "AccSrvRestrictedList", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("a568770a-50ba-4052-ab48-37d8029b3f47"), DisplayName = "Circulation List", InternalName = "CirculationList", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("a573867a-37ca-49dc-86b0-7d033a7ed2c8"), DisplayName = "Premium Site Features Stapling", InternalName = "PremiumSiteStapling", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("a942a218-fa43-4d11-9d85-c01e3e3a37cb"), DisplayName = "Enterprise Wiki Layouts", InternalName = "EnterpriseWikiLayouts", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("aa61ef91-ee2b-42d5-9911-7c6557ad90c2"), DisplayName = "Nintex Workflow Admin", InternalName = "NintexWorkflowEnterpriseAdmin", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("aebc918d-b20f-4a11-a1db-9ed84d79c87e"), DisplayName = "Publishing Resources", InternalName = "PublishingResources", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("aeef8777-70c0-429f-8a13-f12db47a6d47"), DisplayName = "Bulk workflow process button", InternalName = "BulkWorkflow", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("af6d9aec-7c38-4dda-997f-cc1ddbb87c92"), DisplayName = "Web Analytics Customize Reports functionality", InternalName = "WACustomReports", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("af847aa9-beb6-41d4-8306-78e41af9ce25"), DisplayName = "Profile Synchronization Feature", InternalName = "ProfileSynch", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("b21b090c-c796-4b0f-ac0f-7ef1659c20ae"), DisplayName = "SharePoint Server Standard Site Collection features", InternalName = "BaseSite", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("b3da33d0-5e51-4694-99ce-705a3ac80dc5"), DisplayName = "Excel Services Application Edit Site Feature", InternalName = "ExcelServerEdit", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("b50e3104-6812-424f-a011-cc90e6327318"), DisplayName = "Document ID Service", InternalName = "DocId", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("b5934f65-a844-4e67-82e5-92f66aafe912"), DisplayName = "Routing Workflows - SharePoint 2010", InternalName = "ReviewWorkflowsSPD", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("b5d169c9-12db-4084-b68d-eef9273bd898"), DisplayName = "Tenant Business Data Connectivity Administration Stapling", InternalName = "TenantAdminBDCStapling", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("b738400a-f08a-443d-96fa-a852d0356bba"), DisplayName = "$Resources:obacore,tenantadminbdcFeatureTitle;", InternalName = "TenantAdminSecureStore", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("b8f36433-367d-49f3-ae11-f7d76b51d251"), DisplayName = "Administrative Reporting Infrastructure", InternalName = "AdminReportCore", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("bc29e863-ae07-4674-bd83-2c6d0aa5623f"), DisplayName = "SharePoint Server Site Search", InternalName = "OSearchBasicFeature", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("bcf89eb7-bca1-4468-bdb4-ca27f61a2292"), DisplayName = "Access Services Shell", InternalName = "AccSrvShell", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("c04234f4-13b8-4462-9108-b4f5159beae6"), DisplayName = "Advanced Web Analytics", InternalName = "WAMaster", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("c0c2628d-0f59-4873-9cba-100dad2313cb"), DisplayName = "Web Analytics Enterprise Feature Stapler", InternalName = "WAEnterpriseFeatureStapler", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("c43a587e-195b-4d29-aba8-ebb22b48eb1a"), DisplayName = "User Profile Administration Links", InternalName = "SRPProfileAdmin", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("c4773de6-ba70-4583-b751-2a7b1dc67e3a"), DisplayName = "Collect Signatures Workflow - SharePoint 2010", InternalName = "SignaturesWorkflowSPD", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("c59dbaa9-fa01-495d-aaa3-3c02cc2ee8ff"), DisplayName = "Manage Profile Service Application", InternalName = "ManageUserProfileServiceApplication", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("c5d947d6-b0a2-4e07-9929-8e54f5a9fff9"), DisplayName = "Report Center Sample Data", InternalName = "ReportCenterSampleData", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("c6561405-ea03-40a9-a57f-f25472942a22"), DisplayName = "Translation Management Workflow", InternalName = "TranslationWorkflow", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("c65861fa-b025-4634-ab26-22a23e49808f"), DisplayName = "Microsoft Search Administration Web Parts", InternalName = "SearchAdminWebParts", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("c6ac73de-1936-47a4-bdff-19a6fc3ba490"), DisplayName = "Excel Services Application Web Part Farm Feature", InternalName = "ExcelServerWebPartStapler", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("c769801e-2387-47ef-a810-2d292d4cb05d"), DisplayName = "Report Server File Sync", InternalName = "ReportServerItemSync", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("c845ed8d-9ce5-448c-bd3e-ea71350ce45b"), DisplayName = "SharePoint 2007 Workflows", InternalName = "LegacyWorkflows", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("c85e5759-f323-4efb-b548-443d2216efb5"), DisplayName = "Disposition Approval Workflow", InternalName = "ExpirationWorkflow", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("c88c4ff1-dbf5-4649-ad9f-c6c426ebcbf5"), DisplayName = "InfoPath Forms Services support", InternalName = "IPFSSiteFeatures", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("c922c106-7d0a-4377-a668-7f13d52cb80f"), DisplayName = "Search Central Admin Links", InternalName = "OSearchCentralAdminLinks", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("c9c9515d-e4e2-4001-9050-74f980f93160"), DisplayName = "Microsoft Office Server workflows", InternalName = "OffWFCommon", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("ca2543e6-29a1-40c1-bba9-bd8510a4c17b"), DisplayName = "Content Deployment", InternalName = "DeploymentLinks", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("ca7bd552-10b1-4563-85b9-5ed1d39c962a"), DisplayName = "Standard Column Definitions", InternalName = "Fields", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("cdfa39c6-6413-4508-bccf-bf30368472b3"), DisplayName = "Data Connection Library", InternalName = "DataConnectionLibraryStapling", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("cfc6eb4f-b5a9-4c11-b214-00dd22c7e7b5"), DisplayName = "PowerPoint Broadcast Cleanup", InternalName = "PowerPointBroadcastJanitorJob", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("d250636f-0a26-4019-8425-a5232d592c01"), DisplayName = "Offline Synchronization for External Lists", InternalName = "ObaSimpleSolution", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("d250636f-0a26-4019-8425-a5232d592c09"), DisplayName = "Add Dashboard", InternalName = "AddDashboard", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("d2d98dc8-c7e9-46ec-80a5-b38f039c16be"), DisplayName = "FAST Search Server 2010 for SharePoint Master Job Provisioning", InternalName = "FastFarmFeatureActivation", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("d3f51be2-38a8-4e44-ba84-940d35be1566"), DisplayName = "Page Layouts and Master Pages Pack", InternalName = "PublishingLayouts", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("d44a1358-e800-47e8-8180-adf2d0f77543"), DisplayName = "E-mail Integration with Content Organizer", InternalName = "EMailRouting", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("d5191a77-fa2d-4801-9baf-9f4205c9e9d2"), DisplayName = "Time Card List", InternalName = "TimeCardList", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("d5ff2d2c-8571-4c3c-87bc-779111979811"), DisplayName = "Access Services Solution Gallery Feature Stapler", InternalName = "AccSrvSolutionGalleryStapler", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("d7670c9c-1c29-4f44-8691-584001968a74"), DisplayName = "What's New List", InternalName = "WhatsNewList", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("d992aeca-3802-483a-ab40-6c9376300b61"), DisplayName = "Bulk Workflow Timer Job", InternalName = "BulkWorkflowTimerJob", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("da2e115b-07e4-49d9-bb2c-35e93bb9fca9"), DisplayName = "In Place Records Management", InternalName = "InPlaceRecords", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("dd903064-c9d8-4718-b4e7-8ab9bd039fff"), DisplayName = "Content type publishing", InternalName = "ContentTypePublish", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("e09cefae-2ada-4a1d-aee6-8a8398215905"), DisplayName = "$Resources:SearchServerWizard_Feature_Title;", InternalName = "SearchServerWizardFeature", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("e0a45587-1069-46bd-bf05-8c8db8620b08"), DisplayName = "Records Center Configuration", InternalName = "RecordsCenter", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("e15ed6d2-4af1-4361-89d3-2acf8cd485de"), DisplayName = "", InternalName = "ExcelServerWebApplication", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("e4e6a041-bc5b-45cb-beab-885a27079f74"), DisplayName = "Excel Services Application View Farm Feature", InternalName = "ExcelServer", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("e792e296-5d7f-47c7-9dfa-52eae2104c3b"), DisplayName = "$Resources:HealthReportsFeatureTitle;", InternalName = "OSearchHealthReports", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("e8389ec7-70fd-4179-a1c4-6fcb4342d7a0"), DisplayName = "Report Server Integration Feature", InternalName = "ReportServer", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("e8734bb6-be8e-48a1-b036-5a40ff0b8a81"), DisplayName = "Related Links scope settings page", InternalName = "RelatedLinksScopeSettingsLink", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("e978b1a6-8de7-49d0-8600-09a250354e14"), DisplayName = "Site Settings Link to Local Site Directory Settings page.", InternalName = "LocalSiteDirectorySettingsLink", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("e995e28b-9ba8-4668-9933-cf5c146d7a9f"), DisplayName = "Excel Mobile Viewer Feature", InternalName = "MobileExcelWebAccess", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("eaf6a128-0482-4f71-9a2f-b1c650680e77"), DisplayName = "Search Server Web Parts", InternalName = "SearchWebParts", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("eb657559-be37-4b91-a369-1c201183c779"), DisplayName = "Nintex Workflow 2010 Web Parts", InternalName = "NintexWorkflowWebParts", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("ed5e77f7-c7b1-4961-a659-0de93080fa36"), DisplayName = "Personalization Site", InternalName = "PersonalizationSite", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("edf48246-e4ee-4638-9eed-ef3d0aee7597"), DisplayName = "Search Admin Portal Links and Navbar", InternalName = "OSearchPortalAdminLinks", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("ee21b29b-b0d0-42c6-baff-c97fd91786e6"), DisplayName = "Office Workflows", InternalName = "StapledWorkflows", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("ee9dbf20-1758-401e-a169-7db0a6bbccb2"), DisplayName = "PerformancePoint Monitoring", InternalName = "PPSWebParts", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("f0deabbb-b0f6-46ba-8e16-ff3b44461aeb"), DisplayName = "Shared Service Provider User Migrator", InternalName = "UserMigrator", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("f324259d-393d-4305-aa48-36e8d9a7a0d6"), DisplayName = "Shared Services Infrastructure", InternalName = "SharedServices", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("f41cc668-37e5-4743-b4a8-74d1db3fd8a4"), DisplayName = "Mobility Shortcut URL", InternalName = "MobilityRedirect", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("f45834c7-54f6-48db-b7e4-a35fa470fc9b"), DisplayName = "PerformancePoint Content Type Definition", InternalName = "PPSWorkspaceCtype", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("f63b7696-9afc-4e51-9dfd-3111015e9a60"), DisplayName = "V2V Published Links Upgrade", InternalName = "V2VPublishedLinks", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("f661430e-c155-438e-a7c6-c68648f1b119"), DisplayName = "My Site Personal Site Configuration", InternalName = "MySitePersonalSite", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("f6924d36-2fa8-4f0b-b16d-06b7250180fa"), DisplayName = "SharePoint Server Publishing Infrastructure", InternalName = "PublishingSite", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("f7937973-0cf9-4f2d-a549-be2d3c25b772"), DisplayName = "Nintex Workflow Admin", InternalName = "NintexWorkflowAdmin", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("f979e4dc-1852-4f26-ab92-d1b2a190afc9"), DisplayName = "Dashboards Library", InternalName = "BICenterDashboardsLib", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("f9cb1a2a-d285-465a-a160-7e3e95af1fdd"), DisplayName = "Offline Synchronization for External Lists", InternalName = "ObaStaple", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("fb67f269-fd1d-4f9a-af0b-50f5755e19d7"), DisplayName = "Web Companions Stapling", InternalName = "OfficeWebAppsStapling", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("fc33ba3b-7919-4d7e-b791-c6aeccf8f851"), DisplayName = "List Content Targeting", InternalName = "ListTargeting", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("fde5d850-671e-4143-950a-87b473922dc7"), DisplayName = "Three-state workflow", InternalName = "IssueTrackingWorkflow", Hidden = false });
            _features.Add(new SPFeature() { Id = new Guid("fead7313-4b9e-4632-80a2-98a2a2d83297"), DisplayName = "Standard Site Settings Links", InternalName = "SiteSettings", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("fead7313-4b9e-4632-80a2-ff00a2d83297"), DisplayName = "Standard Content Type Settings Links", InternalName = "ContentTypeSettings", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("fead7313-ae6d-45dd-8260-13b563cb4c71"), DisplayName = "Central Administration Links", InternalName = "AdminLinks", Hidden = true });
            _features.Add(new SPFeature() { Id = new Guid("ff48f7e6-2fa1-428d-9a15-ab154762043d"), DisplayName = "\"Connect to Office\" Ribbon Controls", InternalName = "TemplateDiscovery", Hidden = false });
        }
    }
}
