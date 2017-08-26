using SPBrowser.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPClient = Microsoft.SharePoint.Client;

namespace SPBrowser.Extentions
{
    public static class ObjectExtentions
    {
        /// <summary>
        /// Determines the <see cref="Entities.ClientType"/> for the current object.
        /// </summary>
        /// <remarks>
        /// This extention method is mostly used to determine the original class located in the <see cref="TreeNode.Tag"/> property of a <see cref="TreeNode"/>
        /// </remarks>
        /// <param name="obj">Extended object.</param>
        /// <returns>Returns the <see cref="Entities.ClientType"/> for the current object.</returns>
        public static ClientType GetClientType(this object obj)
        {
            switch (obj.GetType().ToString())
            {
                case "Microsoft.Online.SharePoint.TenantAdministration.Tenant":
                    return ClientType.Tenant;
                case "Microsoft.SharePoint.Client.Site":
                    return ClientType.Site;
                case "Microsoft.SharePoint.Client.Web":
                    return ClientType.Web;
                case "Microsoft.SharePoint.Client.List":
                    return ClientType.List;
                case "Microsoft.SharePoint.Client.ListItem":
                    return ClientType.ListItem;
                case "Microsoft.SharePoint.Client.Feature":
                    return ClientType.Feature;
                case "Microsoft.SharePoint.Client.UserProfiles.PersonProperties":
                    return ClientType.PersonProperties;
                case "Microsoft.SharePoint.Client.Field":
                    return ClientType.Field;
                case "Microsoft.SharePoint.Client.FieldCalculated":
                    return ClientType.FieldCalculated;
                case "Microsoft.SharePoint.Client.FieldComputed":
                    return ClientType.FieldComputed;
                case "Microsoft.SharePoint.Client.FieldDateTime":
                    return ClientType.FieldDateTime;
                case "Microsoft.SharePoint.Client.FieldGuid":
                    return ClientType.FieldGuid;
                case "Microsoft.SharePoint.Client.FieldLookup":
                    return ClientType.FieldLookup;
                case "Microsoft.SharePoint.Client.FieldMultiChoice":
                    return ClientType.FieldMultiChoice;
                case "Microsoft.SharePoint.Client.FieldChoice":
                    return ClientType.FieldChoice;
                case "Microsoft.SharePoint.Client.FieldRatingScale":
                    return ClientType.FieldRatingScale;
                case "Microsoft.SharePoint.Client.FieldMultiLineText":
                    return ClientType.FieldMultiLineText;
                case "Microsoft.SharePoint.Client.FieldNumber":
                    return ClientType.FieldNumber;
                case "Microsoft.SharePoint.Client.FieldText":
                    return ClientType.FieldText;
                case "Microsoft.SharePoint.Client.FieldUrl":
                    return ClientType.FieldUrl;
                case "Microsoft.SharePoint.Client.FieldUser":
                    return ClientType.FieldUser;
                case "Microsoft.SharePoint.Client.ContentType":
                    return ClientType.ContentType;
                case "Microsoft.SharePoint.Client.User":
                    return ClientType.User;
                case "Microsoft.SharePoint.Client.Group":
                    return ClientType.Group;
                case "Microsoft.SharePoint.Client.View":
                    return ClientType.View;
                case "Microsoft.SharePoint.Client.Folder":
                    return ClientType.Folder;
                case "Microsoft.SharePoint.Client.File":
                    return ClientType.File;
                case "Microsoft.SharePoint.Client.AppInstance":
                    return ClientType.AppInstance;
                case "Microsoft.SharePoint.Client.Taxonomy.TaxonomyField":
                    return ClientType.TaxonomyField;
                case "Microsoft.SharePoint.Client.WorkflowServices.WorkflowDefinition":
                    return ClientType.WorkflowDefinition;
                default:
                    return ClientType.Unknown;
            }
        }
    }
}
