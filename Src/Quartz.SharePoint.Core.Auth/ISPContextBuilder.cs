using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace Quartz.SharePoint.Core.Auth
{
    interface ISPContextBuilder
    {
        ClientContext GetSPContext(Uri uri);

        void SetCredential(ClientContext clcntx);

        void Load(ClientContext clcntx);
    }
}
