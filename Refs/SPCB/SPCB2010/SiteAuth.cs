using SPBrowser.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Xml.Serialization;
using SPClient = Microsoft.SharePoint.Client;

namespace SPBrowser
{
    [XmlRoot("Site")]
    public class SiteAuth
    {
        [XmlIgnore()]
        public Uri Url
        {
            get { return _url; }
            set { _url = value.RemoveTrailingSlash(); }
        }
        private Uri _url;

        [XmlAttribute("Url")]
        public string UrlAsString
        {
            get { return this.Url.OriginalString; }
            set { this.Url = new Uri(value); }
        }

        [XmlAttribute()]
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        private string _username;

        [XmlIgnore()]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        private string _password;

        [XmlAttribute()]
        public AuthN Authentication
        {
            get { return _authN; }
            set { _authN = value; }
        }
        private AuthN _authN;

        [XmlAttribute()]
        public bool IsLoaded
        {
            get { return _isLoaded; }
            set { _isLoaded = value; }
        }
        private bool _isLoaded = false;

        [XmlAttribute()]
        public DateTime LoadDate
        {
            get { return _loadDate; }
            set { _loadDate = value; }
        }
        private DateTime _loadDate;

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

        [XmlElement()]
        public List<string> Webs
        {
            get { return _webs; }
            set { _webs = value; }
        }
        private List<string> _webs;

        public SiteAuth()
        { }

        public SiteAuth(Uri url, string username, string password, AuthN authn)
        {
            InitSite(url, username, password, authn);
        }

        private void InitSite(Uri url, string username, string password, AuthN authn)
        {
            this.Url = url.RemoveTrailingSlash();
            this.Authentication = authn;
            this.Username = username;
            this.Password = password;
            this.Webs = new List<string>();

            this.InitClientContext();
        }

        /// <summary>
        /// Initializes the ClientContext.
        /// </summary>
        public void InitClientContext()
        {
            this.IsLoaded = true;
            this.LoadDate = DateTime.Now;

            switch (this.Authentication)
            {
                case AuthN.Default:
                    this.ClientContext = new SPClient.ClientContext(this.Url);
                    if (!string.IsNullOrEmpty(this.Username))
                        this.ClientContext.Credentials = new NetworkCredential(this.Username, this.Password);
                    break;
                case AuthN.SharePointOnline:
                    this.ClientContext = MSDN.Samples.ClaimsAuth.ClaimClientContext.GetAuthenticatedContext(this.Url.OriginalString, 0, 0);
                    break;
            }

            // Try connection, to ensure site is available
            SPClient.Site site = this.ClientContext.Site;
            this.ClientContext.Load(site);
            this.ClientContext.ExecuteQuery();
        }
    }

    [XmlRoot("Sites")]
    public class SiteCollections : List<SiteAuth>
    {
        /// <summary>
        /// Adds new site to the collection or opens existing one.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="authn"></param>
        public void Add(Uri url, string username, string password, AuthN authn)
        {
            SiteAuth site = Globals.SiteCollections.SingleOrDefault(s => s.Url.Equals(url.RemoveTrailingSlash()));

            // Check if site already exists
            if (site == null)
            {
                // Add new site to collection
                site = new SiteAuth(url, username, password, authn);
                Globals.SiteCollections.Add(site);
            }
            else
            {
                // Open existing site within collection
                site.Password = password;
                site.InitClientContext();
            }
        }

        /// <summary>
        /// Removes site from collection.
        /// </summary>
        /// <param name="site"></param>
        public new void Remove(SiteAuth site)
        {
            site.IsLoaded = false;
        }

        /// <summary>
        /// Loads previous loaded site collections from configuration file.
        /// </summary>
        public void Load()
        {
            this.AddRange(OpenAndRead(Constants.CONFIG_FILENAME));
        }

        /// <summary>
        /// Saves the current set of site collections to configuration file.
        /// </summary>
        public void Save()
        {
            // Ensure sites are unloaded when saving the history
            foreach (SiteAuth site in this)
            {
                site.IsLoaded = false;
            }

            Write(Constants.CONFIG_FILENAME, this);
        }

        private static SiteCollections OpenAndRead(string fileName)
        {
            SiteCollections sites = new SiteCollections();

            try
            {
                if (!string.IsNullOrEmpty(fileName) && System.IO.File.Exists(fileName))
                {
                    // Create the serializer
                    var serializer = new XmlSerializer(typeof(SiteCollections));

                    // Open config file
                    using (var stream = new System.IO.StreamReader(fileName))
                    {
                        // De-serialize the XML
                        sites = serializer.Deserialize(stream) as SiteCollections;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Can't read the configuration, 'Recent Sites' is empty.", ex);
            }
            
            return sites;
        }

        private static void Write(string fileName, SiteCollections sites)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                // Create the serializer
                var serializer = new XmlSerializer(typeof(SiteCollections));

                // Open config file
                using (var stream = new System.IO.StreamWriter(fileName))
                {
                    // Serialize the XML
                    serializer.Serialize(stream, sites);
                }
            }
        }
    }

    public enum AuthN
    {
        /// <summary>
        /// Default authentication for classic and claims authentication provider.
        /// </summary>
        Default,
        /// <summary>
        /// Microsoft SharePoint Online (Office 365) authentication.
        /// </summary>
        SharePointOnline
    }
}
