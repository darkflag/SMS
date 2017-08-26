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
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using SPClient = Microsoft.SharePoint.Client;

namespace SPBrowser.Entities
{
    [XmlRoot("Site")]
    public class SiteAuthentication
    {
        /// <summary>
        /// Gets the site collection URL.
        /// </summary>
        [XmlIgnore()]
        public Uri Url
        {
            get { return _url; }
            private set { _url = value.RemoveTrailingSlash(); }
        }
        private Uri _url;

        /// <summary>
        /// Gets and sets the site collection URL as string.
        /// </summary>
        [XmlAttribute("Url")]
        public string UrlAsString
        {
            get { return this.Url.OriginalString; }
            set { this.Url = new Uri(value); }
        }

        /// <summary>
        /// Gets and sets the username for authentication with the site collection.
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
        /// Authentication mode used for authentication with the SharePoint.
        /// </summary>
        [XmlAttribute()]
        public AuthenticationMode Authentication
        {
            get { return _authN; }
            set { _authN = value; }
        }
        private AuthenticationMode _authN;

        /// <summary>
        /// Is the client context for the site collection loaded?
        /// </summary>
        [XmlAttribute()]
        public bool IsLoaded
        {
            get { return _isLoaded; }
            set { _isLoaded = value; }
        }
        private bool _isLoaded = false;

        /// <summary>
        /// Gets and sets the date when the site collection was last loaded.
        /// </summary>
        [XmlAttribute()]
        public DateTime LoadDate
        {
            get { return _loadDate; }
            set { _loadDate = value; }
        }
        private DateTime _loadDate;

        /// <summary>
        /// Gets the client context for the site collection.
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
        /// Gets the build version of the remote SharePoint Server.
        /// </summary>
        /// <value>
        /// The build version.
        /// </value>
        [XmlIgnore]
        public Version BuildVersion
        {
            get { return _buildVersion; }
            private set { _buildVersion = value; }
        }
        private Version _buildVersion;

        /// <summary>
        /// Gets a value indicating whether [use current credentials].
        /// </summary>
        /// <remarks>
        /// <see cref="UseCurrentCredentials"/> is TRUE when <see cref="UserName"/> is empty.
        /// </remarks>
        /// <value>
        /// <c>true</c> if [use current credentials]; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        public bool UseCurrentCredentials
        {
            get { return string.IsNullOrEmpty(this.UserName); }
        }

        /// <summary>
        /// Gets and sets a list of webs which are loaded separately and shown directly below the site collection.
        /// </summary>
        [XmlElement()]
        public List<string> Webs
        {
            get { return _webs; }
            set { _webs = value; }
        }
        private List<string> _webs;

        /// <summary>
        /// Default (empty) constructor.
        /// </summary>
        public SiteAuthentication()
        { }

        /// <summary>
        /// Loads the site collection based on authentication mode.
        /// </summary>
        /// <param name="url">Site collection URL</param>
        /// <param name="username">Username, if left empty current credentials are used</param>
        /// <param name="password">Password related to username</param>
        /// <param name="authn">Authentication mode</param>
        public SiteAuthentication(Uri url, string username, string password, AuthenticationMode authn)
        {
            InitSite(url, username, password, authn);
        }

        /// <summary>
        /// Load site collection and initialize client context.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="authn"></param>
        private void InitSite(Uri url, string username, string password, AuthenticationMode authn)
        {
            this.Url = url.RemoveTrailingSlash();
            this.Authentication = authn;
            this.UserName = username;
            this.Password = password;
            this.Webs = new List<string>();

            this.InitClientContext();
        }

        /// <summary>
        /// Initializes the ClientContext.
        /// </summary>
        public void InitClientContext()
        {
            try
            {
                LogUtil.LogMessage("Initializing site collection (ClientContext) for {0}.", this.Url);

                // Set client context
                this.ClientContext = new SPClient.ClientContext(this.Url);
                this.ClientContext.ApplicationName = ProductUtil.GetProductName();

                // Set authentication mode and credentials
                switch (this.Authentication)
                {
                    case AuthenticationMode.Default:
                        this.ClientContext.AuthenticationMode = ClientAuthenticationMode.Default;
                        if (this.UseCurrentCredentials)
                        {
                            LogUtil.LogMessage("Using current user credentials for user '{0}\\{1}'.", Environment.UserDomainName, Environment.UserName);
                            this.ClientContext.Credentials = CredentialCache.DefaultNetworkCredentials;
                        }
                        else
                        {
                            LogUtil.LogMessage("Using custom credentials for user '{0}'.", this.UserName);
                            this.ClientContext.Credentials = new NetworkCredential(this.UserName, this.Password);
                        }
                        break;
                    case AuthenticationMode.SharePointOnline:
                        LogUtil.LogMessage("Using SharePoint Online credentials for user '{0}'.", this.UserName);
                        this.ClientContext.AuthenticationMode = ClientAuthenticationMode.Default;
                        this.ClientContext.Credentials = new SharePointOnlineCredentials(this.UserName, this.Password.GetSecureString());
                        break;
                    case AuthenticationMode.Anonymous:
                        LogUtil.LogMessage("Using anonymous access.");
                        this.ClientContext.AuthenticationMode = ClientAuthenticationMode.Anonymous;
                        break;
                    case AuthenticationMode.Forms:
                        LogUtil.LogMessage("Using Forms Based authentication for user '{0}'.", this.UserName);
                        this.ClientContext.AuthenticationMode = ClientAuthenticationMode.FormsAuthentication;
                        this.ClientContext.FormsAuthenticationLoginInfo = new FormsAuthenticationLoginInfo(this.UserName, this.Password);
                        break;
                    case AuthenticationMode.Claims:
                        LogUtil.LogMessage("Using Claims authentication.");
                        bool isCancelled = false;
                        this.ClientContext = ClaimClientContext.GetAuthenticatedContext(this.Url.ToString(), out isCancelled);
                        if (isCancelled) throw new Exception("Could not load site. Please retry.", new Exception("Loading site cancelled by user."));
                        break;
                    default:
                        throw new NotImplementedException("Current authentication mode not supported.");
                }

                LogUtil.LogMessage("Valide (execute) the ClientContext.");

                // Try connection, to ensure site is available
                SPClient.Site site = this.ClientContext.Site;
                this.ClientContext.Load(site);
                this.ClientContext.ExecuteQuery();

                // After succes set variables 
                this.IsLoaded = true;
                this.LoadDate = DateTime.Now;

                LogUtil.LogMessage("ClientContext successful loaded.");
                LogUtil.LogMessage("ClientContext technical data. ServerVersion: {0}. ServerSchemaVersion: {1}. ServerLibraryVersion: {2}. RequestSchemaVersion: {3}. TraceCorrelationId: {4}",
                    this.ClientContext.ServerVersion,
                    this.ClientContext.ServerSchemaVersion,
                    this.ClientContext.ServerLibraryVersion,
                    this.ClientContext.RequestSchemaVersion,
                    this.ClientContext.TraceCorrelationId);

                //Try retrieving the SharePoint Server build version
                this.BuildVersion = TryGetServerVersion(site.Url);
            }
            catch (FileNotFoundException ex)
            {
                LogUtil.LogException(string.Format("File '{0}' not found, check log file {1} for detailed information.", ex.FileName, ex.FusionLog), ex);

                throw;
            }
        }

        private Version TryGetServerVersion(string siteUrl)
        {
            Version spVersion = null;
            string buildVersionUrl = string.Empty;

            try
            {
                Uri url = new Uri(siteUrl);
                buildVersionUrl = $"{url.Scheme}://{url.Host}/_vti_pvt/buildversion.cnf";

                WebRequest request = WebRequest.Create(buildVersionUrl);
                request.Timeout = 1000;

                if (this.UseCurrentCredentials)
                {
                    request.UseDefaultCredentials = true;
                }
                else
                {
                    request.Credentials =  new NetworkCredential(this.UserName, this.Password);
                }

                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());

                string lines = reader.ReadToEnd();

                response.Close();
                reader.Close();

                Regex regex = new Regex(@"((\d)+\.){3}(\d)+");
                Match result = regex.Match(lines);

                if (result.Success)
                {
                    spVersion = new Version(result.Value);

                    LogUtil.LogMessage($"Retrieved SharePoint Server build version: {spVersion} ({buildVersionUrl})");
                }
            }
            catch (Exception ex)
            {
                LogUtil.LogMessage($"Could not retrieve SharePoint Server build version ({buildVersionUrl}), reason: {ex.Message}.");
            }

            return spVersion;
        }
    }

    [XmlRoot("Sites")]
    public class SiteAuthenticationCollection : List<SiteAuthentication>
    {
        /// <summary>
        /// Gets the <see cref="SiteAuthentication"/> based on the <paramref name="siteUrl"/>.
        /// </summary>
        /// <remarks>
        /// Only retrieves <see cref="SiteAuthentication"/> objects where the <see cref="SiteAuthentication.IsLoaded"/> is set to <see cref="True"/>.
        /// </remarks>
        /// <param name="siteUrl">Url for the site-collection to retrieve.</param>
        /// <returns>Returns the <see cref="SiteAuthentication"/> referred by the <paramref name="siteUrl"/>.</returns>
        /// <exception cref="ArgumentNullException">Raised when <paramref name="siteUrl"/> is null or empty.</exception>
        public SiteAuthentication this[string siteUrl]
        {
            get
            {
                if (string.IsNullOrEmpty(siteUrl))
                    throw new ArgumentNullException(siteUrl);

                return this.SingleOrDefault(s =>
                    s.IsLoaded &&
                    s.Url != null &&
                    Uri.Compare(s.Url, new Uri(siteUrl), UriComponents.HttpRequestUrl, UriFormat.SafeUnescaped, StringComparison.InvariantCultureIgnoreCase) == 0);
            }
        }

        /// <summary>
        /// Gets the <see cref="SiteAuthentication"/> based on the <paramref name="siteUrl"/>.
        /// </summary>
        /// <param name="siteUrl">Url for the site-collection to retrieve.</param>
        /// <param name="isLoaded">Indicates whether the site collection is already loaded. When set to null, both loaded and unloaded sites are checked.</param>
        /// <returns>Returns the <see cref="SiteAuthentication"/> referred by the <paramref name="siteUrl"/>.</returns>
        /// <exception cref="ArgumentNullException">Raised when <paramref name="siteUrl"/> is null or empty.</exception>
        public SiteAuthentication this[string siteUrl, bool? isLoaded]
        {
            get
            {
                if (string.IsNullOrEmpty(siteUrl))
                    throw new ArgumentNullException(siteUrl);

                if (isLoaded == null)
                    return this.SingleOrDefault(s =>
                        s.Url != null &&
                        Uri.Compare(s.Url, new Uri(siteUrl), UriComponents.HttpRequestUrl, UriFormat.SafeUnescaped, StringComparison.InvariantCultureIgnoreCase) == 0);
                else
                    return this.SingleOrDefault(s =>
                        s.IsLoaded == isLoaded &&
                        s.Url != null &&
                        Uri.Compare(s.Url, new Uri(siteUrl), UriComponents.HttpRequestUrl, UriFormat.SafeUnescaped, StringComparison.InvariantCultureIgnoreCase) == 0);
            }
        }

        /// <summary>
        /// Adds new site to the collection or opens existing one.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="authn"></param>
        public SiteAuthentication Add(Uri url, string username, string password, AuthenticationMode authn)
        {
            SiteAuthentication site = Globals.Sites[url.RemoveTrailingSlash().OriginalString, null];

            // Check if site already exists
            if (site == null)
            {
                // Add new site to collection
                site = new SiteAuthentication(url, username, password, authn);
                Globals.Sites.Add(site);

                LogUtil.LogMessage("Added new site collection, not loaded before.");
            }
            else
            {
                // Open existing site within collection
                site.UserName = username;
                site.Password = password;
                site.InitClientContext();

                LogUtil.LogMessage("Added existing site collection, previously loaded.");
            }

            return site;
        }

        /// <summary>
        /// Removes site from collection.
        /// </summary>
        /// <param name="site"></param>
        public new void Remove(SiteAuthentication site)
        {
            site.IsLoaded = false;
        }

        /// <summary>
        /// Loads previous loaded site collections from configuration file.
        /// </summary>
        public void Load()
        {
            this.AddRange(OpenAndRead(Constants.CONFIG_HISTORY_FILENAME).Distinct());
        }

        /// <summary>
        /// Saves the current set of site collections to configuration file.
        /// </summary>
        public void Save()
        {
            // Ensure sites are unloaded when saving the history
            foreach (SiteAuthentication site in this)
            {
                site.IsLoaded = false;
            }

            Write(Constants.CONFIG_HISTORY_FILENAME, this);
        }

        private static SiteAuthenticationCollection OpenAndRead(string fileName)
        {
            SiteAuthenticationCollection sites = new SiteAuthenticationCollection();

            try
            {
                if (!string.IsNullOrEmpty(fileName) && System.IO.File.Exists(fileName))
                {
                    // Create the serializer
                    var serializer = new XmlSerializer(typeof(SiteAuthenticationCollection));

                    // Open config file
                    using (var stream = new System.IO.StreamReader(fileName))
                    {
                        // De-serialize the XML
                        sites = serializer.Deserialize(stream) as SiteAuthenticationCollection;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Can't read the configuration, 'Recent Sites' is empty.", ex);
            }

            return sites;
        }

        private static void Write(string fileName, SiteAuthenticationCollection sites)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                // Create the serializer
                var serializer = new XmlSerializer(typeof(SiteAuthenticationCollection));

                // Open config file
                using (var stream = new System.IO.StreamWriter(fileName))
                {
                    // Serialize the XML
                    serializer.Serialize(stream, sites);
                }
            }
        }
    }
}
