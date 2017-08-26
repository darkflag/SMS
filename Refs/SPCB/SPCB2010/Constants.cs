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
        public const string CONFIG_FILENAME = "History.xml";

        /// <summary>
        /// File name for storing the custom feature definitions.
        /// </summary>
        public const string CUSTOM_FEATURES_FILENAME = "FeatureDefinitions.xml";

        /// <summary>
        /// Codeplex Project URL.
        /// </summary>
        public const string CODEPLEX_PROJECT_URL = "http://spcb.codeplex.com";

        /// <summary>
        /// Wordpress blog for Bram de Jager
        /// </summary>
        public const string PERSONAL_BLOG_URL = "http://bramdejager.wordpress.com";

        /// <summary>
        /// Twitter URL for Bram de Jager.
        /// </summary>
        public const string PERSONAL_TWITTER_URL = "http://www.twitter.com/bramdejager";

        #region Microsoft.SharePoint.Client Namespace

        /// <summary>
        /// Full namespace for "Microsoft.SharePoint.Client.Site" class.
        /// </summary>
        public const string NS_SITE = "Microsoft.SharePoint.Client.Site";

        /// <summary>
        /// Full namespace for "Microsoft.SharePoint.Client.Web" class.
        /// </summary>
        public const string NS_WEB = "Microsoft.SharePoint.Client.Web";

        /// <summary>
        /// Full namespace for "Microsoft.SharePoint.Client.List" class.
        /// </summary>
        public const string NS_LIST = "Microsoft.SharePoint.Client.List";

        /// <summary>
        /// Full namespace for "Microsoft.SharePoint.Client.Feature" class.
        /// </summary>
        public const string NS_FEATURE = "Microsoft.SharePoint.Client.Feature";

        /// <summary>
        /// Full namespace for "Microsoft.SharePoint.Client.Field" class.
        /// </summary>
        public const string NS_FIELD = "Microsoft.SharePoint.Client.Field";
        public const string NS_FIELD_CALCULATED = "Microsoft.SharePoint.Client.FieldCalculated";
        public const string NS_FIELD_COMPUTED = "Microsoft.SharePoint.Client.FieldComputed";
        public const string NS_FIELD_DATETIME = "Microsoft.SharePoint.Client.FieldDateTime";
        public const string NS_FIELD_GUID = "Microsoft.SharePoint.Client.FieldGuid";
        public const string NS_FIELD_LOOKUP = "Microsoft.SharePoint.Client.FieldLookup";
        public const string NS_FIELD_MULTICHOICE = "Microsoft.SharePoint.Client.FieldMultiChoice";
        public const string NS_FIELD_CHOICE = "Microsoft.SharePoint.Client.FieldChoice";
        public const string NS_FIELD_RATINGSCALE = "Microsoft.SharePoint.Client.FieldRatingScale";
        public const string NS_FIELD_MULTILINETEXT = "Microsoft.SharePoint.Client.FieldMultiLineText";
        public const string NS_FIELD_NUMBER = "Microsoft.SharePoint.Client.FieldNumber";
        public const string NS_FIELD_TEXT = "Microsoft.SharePoint.Client.FieldText";
        public const string NS_FIELD_URL = "Microsoft.SharePoint.Client.FieldUrl";

        /// <summary>
        /// Full namespace for "Microsoft.SharePoint.Client.ContentType" class.
        /// </summary>
        public const string NS_CONTENT_TYPE = "Microsoft.SharePoint.Client.ContentType";

        /// <summary>
        /// Full namespace for "Microsoft.SharePoint.Client.User" class.
        /// </summary>
        public const string NS_SITE_USER = "Microsoft.SharePoint.Client.User";

        /// <summary>
        /// Full namespace for "Microsoft.SharePoint.Client.Group" class.
        /// </summary>
        public const string NS_SITE_GROUP = "Microsoft.SharePoint.Client.Group";

        /// <summary>
        /// Full namespace for "Microsoft.SharePoint.Client.View" class.
        /// </summary>
        public const string NS_VIEW = "Microsoft.SharePoint.Client.View";

        /// <summary>
        /// Full namespace for "Microsoft.SharePoint.Client.Folder" class.
        /// </summary>
        public const string NS_FOLDER = "Microsoft.SharePoint.Client.Folder";

        /// <summary>
        /// Full namespace for "Microsoft.SharePoint.Client.File" class.
        /// </summary>
        public const string NS_FILE = "Microsoft.SharePoint.Client.File";
        
        #endregion

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
        public const string IMAGE_FEATURE_CUSTOM = "GenericFeatureCustom.gif";
        public const string IMAGE_SITE_USER = "User.png";
        public const string IMAGE_SITE_USER_EXCLAMATION = "UserExclamation.png";
        public const string IMAGE_SITE_GROUP = "SiteGroup.png";
        public const string IMAGE_SITE_GROUP_DISTRIBUTION = "SiteGroupDistributionList.png";
        public const string IMAGE_SITE_GROUP_SECURITY = "SiteGroupSecurityGroup.png";
        public const string IMAGE_WORKFLOW_ASSOCIATION = "workflows2.png";
        public const string IMAGE_WORKFLOW_TEMPLATE = "workflows2.png";
        public const string IMAGE_VIEW = "View.png";
        public const string IMAGE_FOLDER = "open.png";
        public const string IMAGE_FILE = "File.png";
        public const string IMAGE_PROPERTY = "Property.png";
        public const string IMAGE_EVENT_RECEIVER = "EventReceiver.png";
        public const string IMAGE_RECYCLE_BIN = "RecycleBin.png";
        public const string IMAGE_PUSH_NOTIFICATION_SUBSCRIBER = "itann.png";
        public const string IMAGE_ROLE_ASSIGNMENT = "Permission.png";
        public const string IMAGE_ROLE_DEFINITIONS = "Permission2.png";
        public const string IMAGE_WEB_TEMPLATES = "WebTemplate.png";

        #endregion
    }
}
