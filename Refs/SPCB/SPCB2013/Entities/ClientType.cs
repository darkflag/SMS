using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPBrowser.Entities
{
    /// <summary>
    /// Represents the different classes/types within the SharePoint Client Side Object Model (CSOM)
    /// </summary>
    public enum ClientType
    {
        /// <summary>
        /// Unknown type (default)
        /// </summary>
        Unknown,
        /// <summary>
        /// Represents the <see cref="Microsoft.Online.SharePoint.TenantAdministration.Tenant"/> class type.
        /// </summary>
        Tenant,
        /// <summary>
        /// Represents the <see cref="Microsoft.SharePoint.Client.Site"/> class type.
        /// </summary>
        Site,
        /// <summary>
        /// Represents the <see cref="Microsoft.SharePoint.Client.Web"/> class type.
        /// </summary>
        Web,
        /// <summary>
        /// Represents the <see cref="Microsoft.SharePoint.Client.List"/> class type.
        /// </summary>
        List,
        /// <summary>
        /// Represents the <see cref="Microsoft.SharePoint.Client.ListItem"/> class type.
        /// </summary>
        ListItem,
        /// <summary>
        /// Represents the <see cref="Microsoft.SharePoint.Client.Feature"/> class type.
        /// </summary>
        Feature,
        /// <summary>
        /// Represents the <see cref="Microsoft.SharePoint.Client.UserProfiles.PersonProperties"/> class type.
        /// </summary>
        PersonProperties,
        /// <summary>
        /// Represents the <see cref="Microsoft.SharePoint.Client.Field"/> class type.
        /// </summary>
        Field,
        /// <summary>
        /// Represents the <see cref="Microsoft.SharePoint.Client.FieldCalculated"/> class type.
        /// </summary>
        FieldCalculated,
        /// <summary>
        /// Represents the <see cref="Microsoft.SharePoint.Client.FieldComputed"/> class type.
        /// </summary>
        FieldComputed,
        /// <summary>
        /// Represents the <see cref="Microsoft.SharePoint.Client.FieldDateTime"/> class type.
        /// </summary>
        FieldDateTime,
        /// <summary>
        /// Represents the <see cref="Microsoft.SharePoint.Client.FieldGuid"/> class type.
        /// </summary>
        FieldGuid,
        /// <summary>
        /// Represents the <see cref="Microsoft.SharePoint.Client.FieldLookup"/> class type.
        /// </summary>
        FieldLookup,
        /// <summary>
        /// Represents the <see cref="Microsoft.SharePoint.Client.FieldMultiChoice"/> class type.
        /// </summary>
        FieldMultiChoice,
        /// <summary>
        /// Represents the <see cref="Microsoft.SharePoint.Client.FieldChoice"/> class type.
        /// </summary>
        FieldChoice,
        /// <summary>
        /// Represents the <see cref="Microsoft.SharePoint.Client.FieldRatingScale"/> class type.
        /// </summary>
        FieldRatingScale,
        /// <summary>
        /// Represents the <see cref="Microsoft.SharePoint.Client.FieldMultiLineText"/> class type.
        /// </summary>
        FieldMultiLineText,
        /// <summary>
        /// Represents the <see cref="Microsoft.SharePoint.Client.FieldNumber"/> class type.
        /// </summary>
        FieldNumber,
        /// <summary>
        /// Represents the <see cref="Microsoft.SharePoint.Client.FieldText"/> class type.
        /// </summary>
        FieldText,
        /// <summary>
        /// Represents the <see cref="Microsoft.SharePoint.Client.FieldUrl"/> class type.
        /// </summary>
        FieldUrl,
        /// <summary>
        /// Represents the <see cref="Microsoft.SharePoint.Client.FieldUser"/> class type.
        /// </summary>
        FieldUser,
        /// <summary>
        /// Represents the <see cref="Microsoft.SharePoint.Client.ContentType"/> class type.
        /// </summary>
        ContentType,
        /// <summary>
        /// Represents the <see cref="Microsoft.SharePoint.Client.User"/> class type.
        /// </summary>
        User,
        /// <summary>
        /// Represents the <see cref="Microsoft.SharePoint.Client.Group"/> class type.
        /// </summary>
        Group,
        /// <summary>
        /// Represents the <see cref="Microsoft.SharePoint.Client.View"/> class type.
        /// </summary>
        View,
        /// <summary>
        /// Represents the <see cref="Microsoft.SharePoint.Client.Folder"/> class type.
        /// </summary>
        Folder,
        /// <summary>
        /// Represents the <see cref="Microsoft.SharePoint.Client.File"/> class type.
        /// </summary>
        File,
        /// <summary>
        /// Represents the <see cref="Microsoft.SharePoint.Client.AppInstance"/> class type.
        /// </summary>
        AppInstance,
        /// <summary>
        /// Represents the <see cref="Microsoft.SharePoint.Client.Taxonomy.TaxonomyField"/> class type.
        /// </summary>
        TaxonomyField,
        /// <summary>
        /// Represents the <see cref="Microsoft.SharePoint.Client.WorkflowServices.WorkflowDefinition"/> class type.
        /// </summary>
        WorkflowDefinition
    }
}