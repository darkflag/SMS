using Microsoft.SharePoint.Client;
using SPBrowser.Extentions;
using SPBrowser.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Xml.Serialization;
using SPClient = Microsoft.SharePoint.Client;
using SPOnline = Microsoft.Online.SharePoint.TenantAdministration;

namespace SPBrowser.Entities
{
    [XmlRoot("Tenant")]
    public class TenantAuthentication
    {
        /// <summary>
        /// Gets the tenant admin URL.
        /// </summary>
        [XmlIgnore()]
        public Uri AdminUrl
        {
            get { return _url; }
            private set { _url = value.RemoveTrailingSlash(); }
        }
        private Uri _url;

        /// <summary>
        /// Gets the tenant root site collection URL.
        /// </summary>
        [XmlIgnore()]
        public Uri RootSiteUrl
        {
            get { return _rootSiteUrl; }
            private set { _rootSiteUrl = value.RemoveTrailingSlash(); }
        }
        private Uri _rootSiteUrl;

        /// <summary>
        /// Gets and sets the tenant admin URL as string.
        /// </summary>
        [XmlAttribute("Url")]
        public string AdminUrlAsString
        {
            get { return this.AdminUrl.OriginalString; }
            set { this.AdminUrl = new Uri(value); }
        }

        /// <summary>
        /// Gets and sets the administrator username for authentication with the tenant.
        /// </summary>
        [XmlAttribute()]
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        private string _userName;

        /// <summary>
        /// Gets and sets the password.
        /// </summary>
        [XmlIgnore()]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        private string _password;

        /// <summary>
        /// Is the client context for the tenant loaded?
        /// </summary>
        [XmlAttribute()]
        public bool IsLoaded
        {
            get { return _isLoaded; }
            set { _isLoaded = value; }
        }
        private bool _isLoaded = false;

        /// <summary>
        /// Gets and sets the date when the tenant was last loaded.
        /// </summary>
        [XmlAttribute()]
        public DateTime LoadDate
        {
            get { return _loadDate; }
            set { _loadDate = value; }
        }
        private DateTime _loadDate;

        /// <summary>
        /// Gets the client context for the tenant.
        /// </summary>
        [XmlIgnore()]
        public SPClient.ClientContext ClientContext
        {
            get
            {
                if (_ctx == null)
                    this.InitClientContext();

                return _ctx;
            }
            private set { _ctx = value; }
        }
        private SPClient.ClientContext _ctx;

        /// <summary>
        /// Default (empty) constructor.
        /// </summary>
        public TenantAuthentication()
        { }

        /// <summary>
        /// Loads the tenant based on authentication mode.
        /// </summary>
        /// <param name="adminUrl">Tenant admin URL</param>
        /// <param name="username">Username, if left empty current credentials are used</param>
        /// <param name="password">Password related to username</param>
        /// <param name="authn">Authentication mode</param>
        public TenantAuthentication(Uri adminUrl, string username, string password)
        {
            InitTenant(adminUrl, username, password);
        }

        /// <summary>
        /// Load tenant and initialize client context.
        /// </summary>
        /// <param name="adminUrl"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="authn"></param>
        private void InitTenant(Uri adminUrl, string username, string password)
        {
            this.AdminUrl = adminUrl.RemoveTrailingSlash();
            this.UserName = username;
            this.Password = password;

            this.InitClientContext();
        }

        /// <summary>
        /// Initializes the ClientContext.
        /// </summary>
        public void InitClientContext()
        {
            try
            {
                LogUtil.LogMessage("Initializing tenant (ClientContext) for {0}.", this.AdminUrl);

                // Set client context
                this.ClientContext = new SPClient.ClientContext(this.AdminUrl);
                this.ClientContext.ApplicationName = ProductUtil.GetProductName();

                // Set authentication mode and credentials
                LogUtil.LogMessage("Using SharePoint Online credentials for user '{0}'.", this.UserName);
                this.ClientContext.AuthenticationMode = ClientAuthenticationMode.Default;
                this.ClientContext.Credentials = new SharePointOnlineCredentials(this.UserName, this.Password.GetSecureString());

                // Try connection, to ensure site is available
                LogUtil.LogMessage("Retrieving (execute) the ClientContext.");
                SPOnline.Tenant tenant = new SPOnline.Tenant(this.ClientContext);
                this.ClientContext.Load(tenant);
                this.ClientContext.ExecuteQuery();

                // After succes set variables 
                this.IsLoaded = true;
                this.LoadDate = DateTime.Now;
                this.RootSiteUrl = new Uri(tenant.RootSiteUrl);

                LogUtil.LogMessage("ClientContext successful loaded.");
                LogUtil.LogMessage("ClientContext technical data. ServerVersion: {0}. ServerSchemaVersion: {1}. ServerLibraryVersion: {2}. RequestSchemaVersion: {3}. TraceCorrelationId: {4}",
                    this.ClientContext.ServerVersion,
                    this.ClientContext.ServerSchemaVersion,
                    this.ClientContext.ServerLibraryVersion,
                    this.ClientContext.RequestSchemaVersion,
                    this.ClientContext.TraceCorrelationId);
            }
            catch (FileNotFoundException ex)
            {
                LogUtil.LogException(string.Format("File '{0}' not found, check log file {1} for detailed information.", ex.FileName, ex.FusionLog), ex);

                throw;
            }
        }
    }

    [XmlRoot("Tenants")]
    public class TenantAuthenticationCollection : List<TenantAuthentication>
    {
        /// <summary>
        /// Gets the <see cref="TenantAuthentication"/> based on the <paramref name="rootSiteUrl"/>.
        /// </summary>
        /// <param name="rootSiteUrl">Url for the root-level site-collection within the tenant.</param>
        /// <returns>Returns the <see cref="TenantAuthentication"/> referred by the <paramref name="rootSiteUrl"/>.</returns>
        /// <exception cref="ArgumentNullException">Raised when <paramref name="rootSiteUrl"/> is null or empty.</exception>
        public TenantAuthentication this[string rootSiteUrl]
        {
            get
            {
                if (string.IsNullOrEmpty(rootSiteUrl))
                    throw new ArgumentNullException(rootSiteUrl);

                return this.SingleOrDefault(t => 
                    t.IsLoaded && 
                    t.RootSiteUrl != null && 
                    Uri.Compare(t.RootSiteUrl, new Uri(rootSiteUrl), UriComponents.HttpRequestUrl, UriFormat.SafeUnescaped, StringComparison.InvariantCultureIgnoreCase) == 0);
            }
        }

        /// <summary>
        /// Gets the <see cref="TenantAuthentication"/> based on the <paramref name="adminSiteUrl"/>.
        /// </summary>
        /// <param name="adminSiteUrl">Url for the administration site-collection for this tenant.</param>
        /// <param name="isLoaded">Indicates whether the tenant is already loaded.</param>
        /// <returns>Returns the <see cref="TenantAuthentication"/> referred by the <paramref name="adminSiteUrl"/>.</returns>
        /// <exception cref="ArgumentNullException">Raised when <paramref name="adminSiteUrl"/> is null or empty.</exception>
        public TenantAuthentication this[string adminSiteUrl, bool? isLoaded]
        {
            get
            {
                if (string.IsNullOrEmpty(adminSiteUrl))
                    throw new ArgumentNullException(adminSiteUrl);

                if (isLoaded == null)
                    return this.SingleOrDefault(t => 
                        t.AdminUrl != null && 
                        Uri.Compare(t.AdminUrl, new Uri(adminSiteUrl), UriComponents.HttpRequestUrl, UriFormat.SafeUnescaped, StringComparison.InvariantCultureIgnoreCase) == 0);
                else
                    return this.SingleOrDefault(t => 
                        t.IsLoaded == isLoaded && 
                        t.AdminUrl != null && 
                        Uri.Compare(t.AdminUrl, new Uri(adminSiteUrl), UriComponents.HttpRequestUrl, UriFormat.SafeUnescaped, StringComparison.InvariantCultureIgnoreCase) == 0);
            }
        }

        /// <summary>
        /// Adds new site to the collection or opens existing one.
        /// </summary>
        /// <param name="url">Admin URL for Tenant.</param>
        /// <param name="username">Administrator username.</param>
        /// <param name="password">Administrator password.</param>
        /// <param name="authn">Authentication mode used to connect to Tenant.</param>
        public void Add(Uri url, string username, string password)
        {
            TenantAuthentication tenant = this.SingleOrDefault(s => s.AdminUrl.Equals(url.RemoveTrailingSlash()));

            // Check if site already exists
            if (tenant == null)
            {
                // Add new site to collection
                tenant = new TenantAuthentication(url, username, password);
                this.Add(tenant);

                LogUtil.LogMessage("Added new tenant, not loaded before.");
            }
            else
            {
                // Open existing site within collection
                tenant.UserName = username;
                tenant.Password = password;
                tenant.InitClientContext();

                LogUtil.LogMessage("Added existing tenant, previously loaded.");
            }
        }

        /// <summary>
        /// Removes tenant from collection.
        /// </summary>
        /// <param name="tenant"></param>
        public new void Remove(TenantAuthentication tenant)
        {
            tenant.IsLoaded = false;
        }

        /// <summary>
        /// Loads previous loaded tenants from configuration file.
        /// </summary>
        public void Load()
        {
            this.AddRange(OpenAndRead(Constants.CONFIG_HISTORY_TENANTS_FILENAME).Distinct());
        }

        /// <summary>
        /// Saves the current set of tenants to configuration file.
        /// </summary>
        public void Save()
        {
            // Ensure sites are unloaded when saving the history
            foreach (TenantAuthentication tenant in this)
            {
                tenant.IsLoaded = false;
            }

            Write(Constants.CONFIG_HISTORY_TENANTS_FILENAME, this);
        }

        private static TenantAuthenticationCollection OpenAndRead(string fileName)
        {
            TenantAuthenticationCollection tenants = new TenantAuthenticationCollection();

            try
            {
                if (!string.IsNullOrEmpty(fileName) && System.IO.File.Exists(fileName))
                {
                    // Create the serializer
                    var serializer = new XmlSerializer(typeof(TenantAuthenticationCollection));

                    // Open config file
                    using (var stream = new System.IO.StreamReader(fileName))
                    {
                        // De-serialize the XML
                        tenants = serializer.Deserialize(stream) as TenantAuthenticationCollection;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Can't read the configuration, 'Recent Tenants' is empty.", ex);
            }

            return tenants;
        }

        private static void Write(string fileName, TenantAuthenticationCollection tenants)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                // Create the serializer
                var serializer = new XmlSerializer(typeof(TenantAuthenticationCollection));

                // Open config file
                using (var stream = new System.IO.StreamWriter(fileName))
                {
                    // Serialize the XML
                    serializer.Serialize(stream, tenants);
                }
            }
        }
    }
}
