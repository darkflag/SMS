using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPBrowser.Entities
{
    /// <summary>
    /// Authentication type used for connecting to the site collection.
    /// </summary>
    public enum AuthenticationMode
    {
        /// <summary>
        /// Integrated Windows authentication supporting NTLM and Kerberos (like DOMAIN\Username).
        /// </summary>
        Default = 0,
        /// <summary>
        /// Microsoft SharePoint Online (Office 365) authentication.
        /// </summary>
        SharePointOnline = 1,
        /// <summary>
        /// Anonymous authentication.
        /// </summary>
        Anonymous = 2,
        /// <summary>
        /// ASP.NET forms authentication.
        /// </summary>
        Forms = 3,
        /// <summary>
        /// Claims authentication (AD FS / SAML-token).
        /// </summary>
        Claims = 4
    }
}
