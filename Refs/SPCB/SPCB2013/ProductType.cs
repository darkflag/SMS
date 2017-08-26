using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPBrowser
{
    /// <summary>
    /// Represents the SharePoint Client Browser product type.
    /// </summary>
    /// <remarks>
    /// The enum value relates to the major version of the tool.
    /// </remarks>
    public enum ProductType
    {
        /// <summary>
        /// The SharePoint 2013 Client Browser product.
        /// </summary>
        /// <remarks>
        /// SharePoint 2013 Client Browser version is v1.x
        /// </remarks>
        SharePoint2013ClientBrowser = 1,
        /// <summary>
        /// The SharePoint 2016 Client Browser product.
        /// </summary>
        /// <remarks>
        /// SharePoint 2016 Client Browser version is v2.x
        /// </remarks>
        SharePoint2016ClientBrowser = 2,
        /// <summary>
        /// The SharePoint Online Client Browser product.
        /// </summary>
        /// <remarks>
        /// SharePoint Online Client Browser version is v3.x
        /// </remarks>
        SharePointOnlineClientBrowser = 3
    }
}
