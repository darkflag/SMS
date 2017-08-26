using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPBrowser
{
    public enum LoadType
    {
        GeneralGroup,
        SiteFeatures,
        SiteFields,
        SiteContentTypes,
        /// <summary>
        /// Not supported in SharePoint 2010.
        /// </summary>
        [Obsolete("Not supported in SharePoint 2010.")]
        SiteEventReceivers,
        SiteRecycleBin,
        /// <summary>
        /// Not supported in SharePoint 2010.
        /// </summary>
        [Obsolete("Not supported in SharePoint 2010.")]
        SiteWebTemplates,
        WebSubWebs,
        WebFields,
        WebContentTypes,
        WebLists,
        WebFeatures,
        WebUsers,
        WebGroups,
        /// <summary>
        /// Not supported in SharePoint 2010.
        /// </summary>
        [Obsolete("Not supported in SharePoint 2010.")]
        WebEventReceivers,
        WebProperties,
        /// <summary>
        /// Not supported in SharePoint 2010.
        /// </summary>
        [Obsolete("Not supported in SharePoint 2010.")]
        WebRecycleBin,
        WebListTemplates,
        /// <summary>
        /// Not supported in SharePoint 2010.
        /// </summary>
        [Obsolete("Not supported in SharePoint 2010.")]
        WebPushNotificationSubscribers,
        WebRoleAssignments,
        WebRoleDefinitions,
        WebWorkflowAssociations,
        WebWorkflowTemplates,
        WebTemplates,
        Folder,
        /// <summary>
        /// Not supported in SharePoint 2010.
        /// </summary>
        [Obsolete("Not supported in SharePoint 2010.")]
        FolderProperties,
        GroupUsers,
        ListWorkflowAssociations,
        ListContentTypes,
        ListFields,
        ListViews,
        /// <summary>
        /// Not supported in SharePoint 2010.
        /// </summary>
        [Obsolete("Not supported in SharePoint 2010.")]
        ListEventReceivers,
        ListRoleAssignments
    }
}
