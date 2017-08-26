using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPBrowser.Entities
{
    /// <summary>
    /// Represents SharePoint Server
    /// </summary>
    public class Server
    {
        public string ProductFullname { get; set; }
        public Version BuildVersion { get; set; }
        public string CompatibleRelease { get; set; }
    }
}
