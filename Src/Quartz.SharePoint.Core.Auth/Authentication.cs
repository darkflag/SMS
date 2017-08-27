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
        private Uri _url;

        /// <summary>
        /// Gets the site collection URL.
        /// </summary>
        public Uri Url
        {
            get { return _url; }
            private set { _url = value.RemoveTrailingSlash(); }
        }

        private string _group;

        ///<summary>
        /// Define Organisation level of Authentications items
        ///</summary>
        public string Group
        {
            get { return _group; }
            set { _group = value; }
        }
    }
}
