using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace Quartz.SharePoint.Core.Auth
{
    public abstract class SPContextBuilder : ISPContextBuilder
    {
        public ClientContext GetSPContext(Uri uri)
        {
            try
            {
                // Set client context
                ClientContext cc = new ClientContext(uri);
                SetCredential(cc);
                Load(cc);

                return cc;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public abstract void SetCredential(ClientContext clcntx);

        public abstract void Load(ClientContext clcntx);
    }
}
