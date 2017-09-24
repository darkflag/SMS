using Microsoft.SharePoint.Client;
using Quartz.Framework.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.SharePoint.Core.Auth
{
    public class Authentication
    {
        private ISPContextBuilder _cntxBuilder;

        private Uri _url;

        /// <summary>
        /// Gets the site collection URL.
        /// </summary>
        public Uri Url
        {
            get { return _url; }
            private set { _url = value.RemoveTrailingSlash(); }
        }

        private string _userName;

        /// <summary>
        /// Gets and sets the username for authentication.
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        private string _password;

        /// <summary>
        /// Gets and sets the password.
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        private AuthenticationMode _authM;

        /// <summary>
        /// Authentication mode used for authentication with the SharePoint.
        /// </summary>
        public AuthenticationMode AuthenticationMode
        {
            get { return _authM; }
            set { _authM = value; }
        }

        private ClientContext _ctx;

        /// <summary>
        /// Gets the client context.
        /// </summary>
        public ClientContext ClientContext
        {
            get
            {
                if (_ctx == null)
                    this.InitClientContext();

                return _ctx;
            }
            private set { _ctx = value; }
        }

        /// <summary>
        /// Is the client context loaded?
        /// </summary>
        public bool IsLoaded
        {
            get { return _isLoaded; }
            set { _isLoaded = value; }
        }
        private bool _isLoaded = false;

        /// <summary>
        /// Gets and sets the date when the context was last loaded.
        /// </summary>
        public DateTime LoadDate
        {
            get { return _loadDate; }
            set { _loadDate = value; }
        }
        private DateTime _loadDate;

        private string _group;

        ///<summary>
        /// Define Organisation level of Authentications items
        ///</summary>
        public string Group
        {
            get { return _group; }
            set { _group = value; }
        }

        private ContextScope _contextScope;

        ///<summary>
        /// Define scope of Authenticated context
        ///</summary>
        public ContextScope ContextScope
        {
            get { return _contextScope; }
            set { _contextScope = value; }
        }

        /// <summary>
        /// Default (empty) constructor.
        /// </summary>
        public Authentication()
        { }

        /// <summary>
        /// Loads SharePoint context based on authentication mode.
        /// </summary>
        /// <param name="url">Site collection URL</param>
        /// <param name="username">Username, if left empty current credentials are used</param>
        /// <param name="password">Password related to username</param>
        /// <param name="authm">Authentication mode</param>
        public Authentication(Uri url, string username, string password, AuthenticationMode authm)
        {
            Init(url, username, password, authm);
        }

        /// <summary>
        /// initialize client context.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="authn"></param>
        private void Init(Uri url, string username, string password, AuthenticationMode authm)
        {
            this.Url = url.RemoveTrailingSlash();
            this.AuthenticationMode = authm;
            this.UserName = username;
            this.Password = password;
            this._cntxBuilder = null;//factory her
            //this.Webs = new List<string>();

            this.InitClientContext();
        }

        /// <summary>
        /// Initialize the ClientContext.
        /// </summary>
        public void InitClientContext()
        {
            this.ClientContext = this._cntxBuilder.GetSPContext(this.Url);
            this.IsLoaded = true;
            this.LoadDate = DateTime.Now;
        }

    }
}
