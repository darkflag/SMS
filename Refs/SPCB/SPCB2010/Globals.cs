using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPBrowser
{
    public static class Globals
    {
        public static SiteCollections SiteCollections { get; set; }
        public static FeatureCollection CustomFeatureDefinitions { get; set; }

        static Globals()
        {
            SiteCollections = new SiteCollections();
            CustomFeatureDefinitions = new FeatureCollection();
        }
    }
}
