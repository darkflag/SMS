using SPBrowser.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace SPBrowser.Utils
{
    public class ProductUtil
    {
        /// <summary>
        /// Gets the current product version.
        /// </summary>
        /// <returns></returns>
        public static Version GetCurrentProductVersion()
        {
            FileVersionInfo fvi = GetProductVersionInfo();

            return new Version(fvi.FileVersion);
        }

        /// <summary>
        /// Gets the File Version Information for the current assembly.
        /// </summary>
        /// <returns></returns>
        public static FileVersionInfo GetProductVersionInfo()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi;
        }

        /// <summary>
        /// Gets the release notes.
        /// </summary>
        /// <returns>Returns the release notes for the current version.</returns>
        public static string GetReleaseNotes()
        {
            string releaseNotes = string.Empty;

#if CLIENTSDKV150
            releaseNotes = Properties.Resources.ReleaseNotes15_0;
#elif CLIENTSDKV160
            releaseNotes = Properties.Resources.ReleaseNotes16_0;
#elif CLIENTSDKV161
            releaseNotes = Properties.Resources.ReleaseNotes16_1;
#endif

            return releaseNotes;
        }

        /// <summary>
        /// Gets the product icon (Bitmap of 32x32 pixels).
        /// </summary>
        /// <returns>Returns the product icon for the current product.</returns>
        public static Bitmap GetProductIcon32x32()
        {
            Bitmap icon;

#if CLIENTSDKV150
            icon = Properties.Resources.SharePoint;
#elif CLIENTSDKV160
            icon = Properties.Resources.SharePoint;
#elif CLIENTSDKV161
            icon = Properties.Resources.Office365_32x32;
#endif

            return icon;
        }

        /// <summary>
        /// Gets the name of the product.
        /// </summary>
        /// <returns></returns>
        internal static string GetProductName()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            return assembly.FullName;
        }

        /// <summary>
        /// Gets the product icon.
        /// </summary>
        /// <returns>Returns the product icon for the current product.</returns>
        public static Icon GetProductIcon()
        {
            Icon icon;

#if CLIENTSDKV150
            icon = Properties.Resources.SharePoint2013;
#elif CLIENTSDKV160
            icon = Properties.Resources.SharePoint2013;
#elif CLIENTSDKV161
            icon = Properties.Resources.Office365;
#endif

            return icon;
        }

        /// <summary>
        /// Validates if the SharePoint build version matches this release.
        /// </summary>
        /// <remarks>
        /// Checks if the SharePoint build version is in range of expected build version. 
        /// </remarks>
        /// <returns></returns>
        public static bool DoesServerBuildVersionMatchThisRelease(Version buildVersion, out Server sharePointServer)
        {
            bool isMatch = false;
            sharePointServer = new Server() { BuildVersion = buildVersion };

            // Unknown server version
            if (buildVersion == null)
            {
                sharePointServer.ProductFullname = "unknown SharePoint Server";
                sharePointServer.CompatibleRelease = "unable to determine release";
            }
            else
            {
                // Compatibility with SP2013
                if (buildVersion.Major == 15)
                {
                    sharePointServer.ProductFullname = "SharePoint Server 2013";
                    sharePointServer.CompatibleRelease = "SharePoint 2013 Client Browser";
#if CLIENTSDKV150
                    isMatch = true;
#endif
                }

                // Compatibility with SP2016
                if (buildVersion.Major == 16 && buildVersion.Build > 4000 && buildVersion.Build < 5000)
                {
                    sharePointServer.ProductFullname = "SharePoint Server 2016";
                    sharePointServer.CompatibleRelease = "SharePoint 2016 Client Browser";
#if CLIENTSDKV160
                    isMatch = true;
#endif
                }

                // Compatibility with SPO
                if (buildVersion.Major == 16 && buildVersion.Build > 6000)
                {
                    sharePointServer.ProductFullname = "SharePoint Online";
                    sharePointServer.CompatibleRelease = "SharePoint Online Client Browser";
#if CLIENTSDKV161
                    isMatch = true;
#endif
                }
            }

            return isMatch;
        }
    }
}
