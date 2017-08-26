using SPBrowser.Entities;
using SPBrowser.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPBrowser
{
    public static class Globals
    {
        /// <summary>
        /// Site collections history loaded previously in the tool.
        /// </summary>
        /// <remarks>
        /// These site collections are stored in the <see cref="Constants.CONFIG_HISTORY_FILENAME"/> (History.xml).
        /// </remarks>
        public static SiteAuthenticationCollection Sites { get; set; }

        /// <summary>
        /// Tenants history loaded previously in the tool.
        /// </summary>
        /// <remarks>
        /// These tenants are stored in the <see cref="Constants.CONFIG_HISTORY_FILENAME"/> (History.xml).
        /// </remarks>
        public static TenantAuthenticationCollection Tenants { get; set; }

        /// <summary>
        /// User defined features used to display friendly names.
        /// </summary>
        /// <remarks>
        /// Allows users to add friendly names to features who are not out-of-the-box. The features are stored in the <see cref="Constants.CUSTOM_FEATURES_FILENAME"/> (FeatureDefinitions.xml).
        /// </remarks>
        public static FeatureCollection CustomFeatureDefinitions { get; set; }


        /// <summary>
        /// Gets or sets the latest release for the SharePoint Client Browser.
        /// </summary>
        /// <remarks>
        /// Retrieves the lastest release from CodePlex project RSS feed.
        /// </remarks>
        /// <value>
        /// The latest release.
        /// </value>
        public static Release LatestRelease { get; set; }

        /// <summary>
        /// Command-line arguments provided during startup of the application.
        /// </summary>
        public static CommandArguments Arguments { get; set; }

        /// <summary>
        /// Gets or sets the SharePoint Client Browser product type.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        public static ProductType Product { get; private set; }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static Globals()
        {
            Sites = new SiteAuthenticationCollection();
            Tenants = new TenantAuthenticationCollection();
            CustomFeatureDefinitions = new FeatureCollection();
            Product = (ProductType)ProductUtil.GetCurrentProductVersion().Major;
        }
    }
}
