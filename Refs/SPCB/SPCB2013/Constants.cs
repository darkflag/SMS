using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPBrowser
{
    public class Constants
    {
        /// <summary>
        /// Node text for temporarely loading node.
        /// </summary>
        public const string NODE_LOADING_TEXT = "Loading...";

        /// <summary>
        /// File name for storing the history configuration.
        /// </summary>
        public const string CONFIG_HISTORY_FILENAME = "History.xml";

        /// <summary>
        /// File name for storing the history configuration.
        /// </summary>
        public const string CONFIG_HISTORY_TENANTS_FILENAME = "HistoryTenants.xml";

        /// <summary>
        /// File name for storing the custom feature definitions.
        /// </summary>
        public const string CUSTOM_FEATURES_FILENAME = "FeatureDefinitions.xml";

        /// <summary>
        /// Codeplex Project URL.
        /// </summary>
        public const string CODEPLEX_PROJECT_URL = "http://spcb.codeplex.com";

        /// <summary>
        /// Codeplex issue list URL.
        /// </summary>
        public const string CODEPLEX_ISSUE_LIST_URL = "http://spcb.codeplex.com/workitem/list/basic";

        /// <summary>
        /// Wordpress blog for Bram de Jager
        /// </summary>
        public const string PERSONAL_BLOG_URL = "http://bramdejager.wordpress.com";

        /// <summary>
        /// Twitter URL for Bram de Jager.
        /// </summary>
        public const string PERSONAL_TWITTER_URL = "http://www.twitter.com/bramdejager";

        /// <summary>
        /// Title for SharePoint Server 2013 Client Components SDK.
        /// </summary>
        public const string DOWNLOAD_SP2013_SDK_TITLE = "SharePoint Server 2013 Client Components SDK";

        /// <summary>
        /// Download URL for SharePoint Server 2013 Client Components SDK.
        /// </summary>
        public const string DOWNLOAD_SP2013_SDK_URL = "http://www.microsoft.com/en-us/download/details.aspx?id=35585";

        /// <summary>
        /// Title for SharePoint Server 2016 Client Components SDK.
        /// </summary>
        public const string DOWNLOAD_SP2016_SDK_TITLE = "SharePoint Server 2016 Client Components SDK";

        /// <summary>
        /// Download URL for SharePoint Server 2016 Client Components SDK.
        /// </summary>
        public const string DOWNLOAD_SP2016_SDK_URL = "http://www.microsoft.com/en-us/download/details.aspx?id=51679";

        /// <summary>
        /// Title for SharePoint Online Client Components SDK.
        /// </summary>
        public const string DOWNLOAD_SPONLINE_SDK_TITLE = "SharePoint Online Client Components SDK";

        /// <summary>
        /// Download URL for SharePoint Online Client Components SDK.
        /// </summary>
        public const string DOWNLOAD_SPONLINE_SDK_URL = "http://www.microsoft.com/en-us/download/details.aspx?id=42038";

        /// <summary>
        /// Truncate the logs directory, keep last number of log files.
        /// </summary>
        public const int LOGS_TRUNCATE_AFTER_FILES = 10;

        /// <summary>
        /// The 'local people results' content source identifier.
        /// </summary>
        public const string LOCAL_PEOPLE_RESULTS_CONTENT_SOURCE_ID = "B09A7990-05EA-4AF9-81EF-EDFAB16C4E31";

        /// <summary>
        /// The default row limit before thresshold kick in. 
        /// </summary>
        public const int LIST_ROW_LIMIT = 5000;

        /// <summary>
        /// The search batch size when executing a search query.
        /// </summary>
        public const int SEARCH_BATCH_SIZE = 100;

        /// <summary>
        /// The search row limit.
        /// </summary>
        public const int SEARCH_ROW_LIMIT = 500;

        /// <summary>
        /// Row limit for retrieving error logs.
        /// </summary>
        public const int ERROR_LOGS_ROW_LIMIT = 1000;

        /// <summary>
        /// Default time frame in months for retrieval of error logs.
        /// </summary>
        public const int ERROR_LOGS_ROW_TIME_FRAME_IN_MONTHS = 1;

        /// <summary>
        /// The default public key token for signing the assemblies for the SharePoint Client Components (CSOM).
        /// </summary>
        public const string SHAREPOINT_CLIENT_PUBLIC_KEY_TOKEN = "71e9bce111e9429c";

        /// <summary>
        /// Filename of old app.config file, bound to executable named SPCB2013.exe.
        /// </summary>
        public const string APP_CONFIG_FILE_SPCB2013 = "SPCB2013.exe.config";

        /// <summary>
        /// Label for indication in private mode.
        /// </summary>
        public const string BROWSER_IN_PRIVATE_MODE_LABEL = " (private)";

        /// <summary>
        /// The Picture Library template base identifier.
        /// </summary>
        public const int PICTURE_LIBRARY_BASE_ID = 109;
        
        #region Image file names

        private const string IMAGE_NOT_AVAILABLE = "16x16.png";

        public const string IMAGE_SITE = "sharepointfoundation16.png";
        public const string IMAGE_SITE_WARNING = "sharepointfoundation16-warning.png";
        public const string IMAGE_WEB = "SubSite.png";
        public const string IMAGE_APP_WEB = "App3.png";
        public const string IMAGE_LIST = "itgen.png";
        public const string IMAGE_LIST_TEMPLATES = "ListTemplate.png";
        public const string IMAGE_SITE_COLUMN = "SiteColumn.png";
        public const string IMAGE_SITE_COLUMN_GROUP = "GroupCollection1.png";
        public const string IMAGE_CONTENT_TYPE = "ContentType.png";
        public const string IMAGE_CONTENT_TYPE_GROUP = "GroupCollection1.png";
        public const string IMAGE_FEATURE = "GenericFeature.gif";
        public const string IMAGE_FEATURE_ACTIVATED = "GenericFeatureActivated.gif";
        public const string IMAGE_FEATURE_DEACTIVATED = "GenericFeatureDeactivated.gif";
        public const string IMAGE_FEATURE_CUSTOM = "GenericFeatureCustom.gif";
        public const string IMAGE_FEATURE_CUSTOM_ACTIVATED = "GenericFeatureCustomActivated.gif";
        public const string IMAGE_FEATURE_CUSTOM_DEACTIVATED = "GenericFeatureCustomDeactivated.gif";
        public const string IMAGE_SITE_USER = "User.png";
        public const string IMAGE_SITE_USER_EXCLAMATION = "UserExclamation.png";
        public const string IMAGE_SITE_GROUP = "SiteGroup.png";
        public const string IMAGE_SITE_GROUP_DISTRIBUTION = "SiteGroupDistributionList.png";
        public const string IMAGE_SITE_GROUP_SECURITY = "SiteGroupSecurityGroup.png";
        public const string IMAGE_WORKFLOW_ASSOCIATION = "workflows2.png";
        public const string IMAGE_WORKFLOW_TEMPLATE = "workflows2.png";
        public const string IMAGE_WORKFLOW_INSTANCE = "workflows2.png";
        public const string IMAGE_VIEW = "View.png";
        public const string IMAGE_FOLDER = "Folder.png";
        public const string IMAGE_FOLDER_ERROR = "FolderError.png";
        public const string IMAGE_FILE = "File.png";
        public const string IMAGE_FILE_VERSIONS = "VersionHistory.png";
        public const string IMAGE_PROPERTY = "Property.png";
        public const string IMAGE_EVENT_RECEIVER = "EventReceiver.png";
        public const string IMAGE_RECYCLE_BIN = "RecycleBin.png";
        public const string IMAGE_PUSH_NOTIFICATION_SUBSCRIBER = "itann.png";
        public const string IMAGE_ROLE_ASSIGNMENT = "Permission.png";
        public const string IMAGE_ROLE_DEFINITIONS = "Permission2.png";
        public const string IMAGE_WEB_TEMPLATES = "WebTemplate.png";
        public const string IMAGE_TERM_STORE = "EMMRoot.png";
        public const string IMAGE_TERM_GROUP = "EMMGroup.png";
        public const string IMAGE_TERM_SET = "EMMTermSet.png";
        public const string IMAGE_TERM = "EMMTerm.png";
        public const string IMAGE_USER_PROFILE = "UserProfile.png";
        public const string IMAGE_USER_PROFILE_PROPERTY = "Property.png";
        public const string IMAGE_USER_PROFILE_PEERS = "UserProfile.png";
        public const string IMAGE_USER_PROFILE_EXTENDED_REPORTS = "UserProfileReports.png";
        public const string IMAGE_USER_PROFILE_EXTENDED_MANAGERS = "UserProfileReports.png";
        public const string IMAGE_USER_PROFILE_DIRECT_REPORTS = "UserProfileReports.png";
        public const string IMAGE_DOTNET_OBJECT = "DotNetObject.png";
        public const string IMAGE_USER_CUSTOM_ACTIONS = "CustomAction.png";
        public const string IMAGE_APP_INSTANCES = "App.png";
        public const string IMAGE_WEBPART = "WebPart.png";
        public const string IMAGE_ITEM = "Item.png";
        public const string IMAGE_ITEM_RECORD = "ItemRecord.png";
        public const string IMAGE_ITEM_VALUE = "Item.png";
        public const string IMAGE_TENANT = "Office365.png";
        public const string IMAGE_SITE_PROPERTIES = "SiteProperties.png";
        public const string IMAGE_DELETED_SITE_PROPERTIES = "DeletedSiteProperties.png";
        public const string IMAGE_EXTERNAL_USERS = "ExternalUser.png";
        public const string IMAGE_APP_ERROR = "Warning.png";
        public const string IMAGE_TENANT_ERROR = "Warning.png";
        public const string IMAGE_PROJECT_POLICY = "ProjectPolicy.png";
        public const string IMAGE_SITE_USAGE = "Usage.png";
        public const string IMAGE_SITE_UPGRADE_INFO = "UpgradeInfo.png";
        public const string IMAGE_NAVIGATION = "Navigation.png";
        public const string IMAGE_THEME_INFO = "Theme.png";
        public const string IMAGE_DATA_LEAKAGE_PREVENTATION_STATUS_INFO = "DataLeakage.png";
        public const string IMAGE_APP_TILE = "AppTitles.png";
        public const string IMAGE_REGIONAL_SETTINGS = "RegionalSettings.png";
        public const string IMAGE_FIELD_LINK = "FieldLinks.png";
        public const string IMAGE_ALERT = "Alert.png";
        public const string IMAGE_SITE_AUDIT = "SiteAudit.png";
        public const string IMAGE_INFORMATION_RIGHTS_MANAGEMENT_SETTINGS = "InformationRightsManagement.png";
        public const string IMAGE_FILE_VERSIONS_EVENT = IMAGE_FILE_VERSIONS;
        public const string IMAGE_TIME_ZONES = IMAGE_REGIONAL_SETTINGS;
        public const string IMAGE_COMPLIANCE_INFO = "ComplianceInfo.png";
        public const string IMAGE_STORAGE_METRICS = "StorageMetrics.png";
        public const string IMAGE_SITE_ADMINS = "SiteCollectionAdmin.png";

        #endregion
    }
}
