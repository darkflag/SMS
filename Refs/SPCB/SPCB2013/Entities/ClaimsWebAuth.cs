using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Net.Security;

using System.Reflection;
using System.Text.RegularExpressions;
using System.Globalization;

namespace SPBrowser.Entities
{
    /// <summary>
    /// Provides means to authenticate a user via a pop up login form.
    /// </summary>
    public class ClaimsWebAuth : IDisposable
    {
        #region Construction

        /// <summary>
        /// Displays a pop up window to authenticate the user
        /// </summary>
        /// <param name="targetSiteUrl"></param>
        /// <param name="popUpWidth"></param>
        /// <param name="popUpHeight"></param>
        public ClaimsWebAuth(string targetSiteUrl, int popUpWidth, int popUpHeight)
        {
            if (string.IsNullOrEmpty(targetSiteUrl)) throw new ArgumentException(MSG_REQUIRED_SITE_URL);
            this.fldTargetSiteUrl = targetSiteUrl;

            // set login page url and success url from target site
            this.GetClaimParams(this.fldTargetSiteUrl, out this.fldLoginPageUrl, out  this.fldNavigationEndUrl);

            this.fldNavigationEndUrl = new Uri(targetSiteUrl);
            this.PopUpHeight = popUpHeight;
            this.PopUpWidth = popUpWidth;

            this.webBrowser = new WebBrowser();
            this.webBrowser.Navigated += new WebBrowserNavigatedEventHandler(ClaimsWebBrowser_Navigated);
            this.webBrowser.ScriptErrorsSuppressed = true;
            this.webBrowser.Dock = DockStyle.Fill;
        }

        #endregion

        #region Constants

        public const string CLAIM_HEADER_RETURN_URL = "X-Forms_Based_Auth_Return_Url";
        public const string CLAIM_HEADER_AUTH_REQUIRED = "X-FORMS_BASED_AUTH_REQUIRED";

        public const string MSG_REQUIRED_SITE_URL = "The Site URL is required.";
        public const string MSG_NOT_CLAIM_SITE = "The requested site does not appear to have claims enabled or the Login Url has not been set.";

        public const int DEFAULT_POP_UP_WIDTH = 925;
        public const int DEFAULT_POP_UP_HEIGHT = 525;

        #endregion

        #region private Fields
        private WebBrowser webBrowser;
       
        private CookieCollection fldCookies;
        private Form DisplayLoginForm;

        #endregion

        #region Public Properties

        private string fldLoginPageUrl;
        /// <summary>
        /// Login form Url
        /// </summary>
        public string LoginPageUrl
        {
            get { return fldLoginPageUrl; }
            set { fldLoginPageUrl = value; }
        }

        private Uri fldNavigationEndUrl;
        /// <summary>
        /// Success Url
        /// </summary>
        public Uri NavigationEndUrl
        {
            get { return fldNavigationEndUrl; }
            set { fldNavigationEndUrl = value; }
        }

        /// <summary>
        /// Target site Url
        /// </summary>
        private string fldTargetSiteUrl = null;
        public string TargetSiteUrl
        {
            get { return fldTargetSiteUrl; }
            set { fldTargetSiteUrl = value; }
        }

        /// <summary>
        /// Cookies returned from CLAIM server.
        /// </summary>
        public CookieCollection AuthCookies
        {
            get { return fldCookies; }
        }

        private bool fldIsCLAIMSite = false;
        /// <summary>
        /// Is set to true if the CLAIM site did not return the proper headers -- hence it's not an CLAIM site or does not support CLAIM style authentication
        /// </summary>
        public bool IsCLAIMSite
        {
            get { return fldIsCLAIMSite; }
        }

        private int fldPopUpWidth = 0;
        /// <summary>
        /// Width of Login dialog
        /// </summary>
        public int PopUpWidth
        {
            get { return fldPopUpWidth; }
            set { fldPopUpWidth = value; }
        }

        private int fldPopUpHeight;
        /// <summary>
        /// Height of Login dialog
        /// </summary>
        public int PopUpHeight
        {
            get { return fldPopUpHeight; }
            set { fldPopUpHeight = value; }
        }

        private bool fldIsCancelled = false;
        /// <summary>
        /// Is the authentication request cancelled?
        /// </summary>
        public bool IsCancelled
        {
            get { return fldIsCancelled; }
            set { fldIsCancelled = value; }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Opens a Windows Forms Web Browser control to authenticate the user against an CLAIM site.
        /// </summary>
        /// <param name="popUpWidth"></param>
        /// <param name="popUpHeight"></param>
        public CookieCollection Show()
        {
            if (string.IsNullOrEmpty(this.LoginPageUrl)) throw new ApplicationException(MSG_NOT_CLAIM_SITE);

            // navigate to the login page url.
            this.webBrowser.Navigate(this.LoginPageUrl);

            DisplayLoginForm = new Form();
            DisplayLoginForm.SuspendLayout();

            // size the login form
            int dialogWidth = DEFAULT_POP_UP_WIDTH;
            int dialogHeight = DEFAULT_POP_UP_HEIGHT;

            if (PopUpHeight != 0 && PopUpWidth != 0)
            {
                dialogWidth = Convert.ToInt32(PopUpWidth);
                dialogHeight = Convert.ToInt32(PopUpHeight);
            }

            DisplayLoginForm.Width = dialogWidth;
            DisplayLoginForm.Height = dialogHeight;
            DisplayLoginForm.Text = this.fldTargetSiteUrl;

            DisplayLoginForm.Controls.Add(this.webBrowser);
            DisplayLoginForm.ResumeLayout(false);
            
            //Application.Run(DisplayLoginForm);
            Form parentForm = Application.OpenForms.Cast<Form>().SingleOrDefault(f => f is SPBrowser.MainForm);
            DisplayLoginForm.Icon = parentForm.Icon;
            DisplayLoginForm.StartPosition = FormStartPosition.CenterParent;
            DisplayLoginForm.ShowDialog(parentForm);

            if (DisplayLoginForm.DialogResult == DialogResult.Cancel)
                IsCancelled = true;

            // see ClaimsWebBrowser_Navigated event
            return this.fldCookies;
        }
        

        #endregion

        #region Private Methods

        private void GetClaimParams(string targetUrl, out string loginUrl, out Uri navigationEndUrl)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(targetUrl);
            //webRequest.Method = Constants.WR_METHOD_OPTIONS;
            webRequest.Method = "GET";
            webRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.3; WOW64; Trident/7.0; .NET4.0E; ";
            webRequest.AllowAutoRedirect = false;
            webRequest.Accept = "image/jpeg, application/x-ms-application, image/gif, application/xaml+xml, image/pjpeg, application/x-ms-xbap, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
#if DEBUG
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(IgnoreCertificateErrorHandler);
#endif

            try
            {
                WebResponse response = (WebResponse)webRequest.GetResponse();
                ExtraHeadersFromResponse(response, out loginUrl, out navigationEndUrl);
            }
            catch (WebException webEx)
            {
                ExtraHeadersFromResponse(webEx.Response, out loginUrl, out navigationEndUrl);
            }
        }

        private bool ExtraHeadersFromResponse(WebResponse response, out string loginUrl, out Uri navigationEndUrl)
        {
            loginUrl = null;
            navigationEndUrl = null;

            try
            {
                navigationEndUrl = new Uri(response.Headers[CLAIM_HEADER_RETURN_URL]);
                loginUrl = (response.Headers[CLAIM_HEADER_AUTH_REQUIRED]);
                response.Close();
                return true;
            }
            catch
            {
                if(response.ResponseUri != null){
                    loginUrl = response.ResponseUri.ToString();
                }
                response.Close();
                return false;
            }
        }

        private CookieCollection ExtractAuthCookiesFromUrl(string url)
        {
            Uri uriBase = new Uri(url);
            Uri uri = new Uri(uriBase, "/");
            // call WinInet.dll to get cookie.
            string stringCookie = CookieReader.GetCookie(uri.ToString());
            if (string.IsNullOrEmpty(stringCookie)) return null;
            stringCookie = stringCookie.Replace("; ", ",").Replace(";", ",");
            // use CookieContainer to parse the string cookie to CookieCollection
            CookieContainer cookieContainer = new CookieContainer();
            cookieContainer.SetCookies(uri, stringCookie);
            return cookieContainer.GetCookies(uri);
        }

        #endregion

        #region Private Events

        private void ClaimsWebBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            // check whether the url is same as the navigationEndUrl.
            if (fldNavigationEndUrl != null && fldNavigationEndUrl.Host.Equals(e.Url.Host))
            {
                this.fldCookies = ExtractAuthCookiesFromUrl(this.LoginPageUrl);
                this.DisplayLoginForm.DialogResult = DialogResult.OK;
                this.DisplayLoginForm.Close();
            }
        }
       
        #endregion

        #region IDisposable Methods
        /// <summary> 
        /// Disposes of this instance. 
        /// </summary> 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.webBrowser != null) this.webBrowser.Dispose();

                if (this.DisplayLoginForm != null) this.DisplayLoginForm.Dispose();
            }
        }

        #endregion

        #region Utilities
#if DEBUG
        private bool IgnoreCertificateErrorHandler
           (object sender,
           System.Security.Cryptography.X509Certificates.X509Certificate certificate,
           System.Security.Cryptography.X509Certificates.X509Chain chain,
           System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {

            return true;
        }
#endif // DEBUG
        #endregion
    }
}
