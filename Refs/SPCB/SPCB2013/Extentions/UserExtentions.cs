using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SPClient = Microsoft.SharePoint.Client;

namespace SPBrowser.Extentions
{
    public static class UserExtentions
    {
        //public static string GetUserUrl(this SPClient.User user)
        //{
        //    return null;
        //}

        public static string GetSettingsUrl(this SPClient.User user)
        {
            // <sitecollection|web>/_layouts/userdisp.aspx?ID=10
            return string.Format("{0}/_layouts/userdisp.aspx?ID={1}&Force=True", user.Context.Url, user.Id);
        }

        /// <summary>
        /// Gets the tree node icon.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public static string GetTreeNodeIcon(this SPClient.User user)
        {
            string image = string.Empty;
            bool isExternal = false;

#if CLIENTSDKV161UP
            isExternal = user.IsShareByEmailGuestUser;
#endif

            if (isExternal)
            {
                image = Constants.IMAGE_EXTERNAL_USERS;
            }
            else
            {
                switch (user.PrincipalType)
                {
                    case Microsoft.SharePoint.Client.Utilities.PrincipalType.All:
                        image = Constants.IMAGE_SITE_USER_EXCLAMATION;
                        break;
                    case Microsoft.SharePoint.Client.Utilities.PrincipalType.DistributionList:
                        image = Constants.IMAGE_SITE_GROUP_DISTRIBUTION;
                        break;
                    case Microsoft.SharePoint.Client.Utilities.PrincipalType.None:
                        image = Constants.IMAGE_SITE_USER_EXCLAMATION;
                        break;
                    case Microsoft.SharePoint.Client.Utilities.PrincipalType.SecurityGroup:
                        image = Constants.IMAGE_SITE_GROUP_SECURITY;
                        break;
                    case Microsoft.SharePoint.Client.Utilities.PrincipalType.SharePointGroup:
                        image = Constants.IMAGE_SITE_GROUP;
                        break;
                    case Microsoft.SharePoint.Client.Utilities.PrincipalType.User:
                        image = Constants.IMAGE_SITE_USER;
                        break;
                    default:
                        break;
                }
            }

            return image;
        }
    }
}
