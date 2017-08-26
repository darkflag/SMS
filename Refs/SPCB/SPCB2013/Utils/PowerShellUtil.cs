using SPBrowser.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SPBrowser.Utils
{
    public class PowerShellUtil
    {
        #region Placeholders

        private const string POWERSHELL_OPENSITE_FILENAME = "OpenSite.ps1";
        private const string POWERSHELL_OPENTENANT_FILENAME = "OpenTenant.ps1";
        private const string PLACEHOLDER_CURRENT_DIRECTORY = "[[CurrentDirectory]]";
        private const string PLACEHOLDER_SITE_URL = "[[SiteUrl]]";
        private const string PLACEHOLDER_TENANT_URL = "[[TenantUrl]]";
        private const string PLACEHOLDER_LOGIN_NAME = "[[LoginName]]";
        private const string PLACEHOLDER_SCRIPT_LOCATION = "[[ScriptLocation]]";
        private const string PLACEHOLDER_AUTHENTICATION = "[[Authentication]]";
        private const string PLACEHOLDER_CLIENT_SDK = "[[ClientSDK]]";
        private const string PLACEHOLDER_CLIENT_SDK_URL = "[[ClientSDKUrl]]";
        private const string PLACEHOLDER_CLIENT_SDK_VERSION = "[[ClientSDKVersion]]";

        private const string AUTHENTICATION_DEFAULT_CURRENT_USER = @"$ctx = New-Object Microsoft.SharePoint.Client.ClientContext($siteUrl)
$ctx.AuthenticationMode = [Microsoft.SharePoint.Client.ClientAuthenticationMode]::Default
$ctx.Credentials = [System.Net.CredentialCache]::DefaultNetworkCredentials";

        private const string AUTHENTICATION_DEFAULT_CUSTOM = @"Write-Host Please enter password for $($siteUrl):
$pwd = Read-Host -AsSecureString
$ctx = New-Object Microsoft.SharePoint.Client.ClientContext($siteUrl)
$ctx.AuthenticationMode = [Microsoft.SharePoint.Client.ClientAuthenticationMode]::Default
$ctx.Credentials = New-Object System.Net.NetworkCredential($loginname, $pwd)";

        private const string AUTHENTICATION_SHAREPOINT_ONLINE = @"Write-Host Please enter password for $($siteUrl):
$pwd = Read-Host -AsSecureString
$ctx = New-Object Microsoft.SharePoint.Client.ClientContext($siteUrl)
$ctx.AuthenticationMode = [Microsoft.SharePoint.Client.ClientAuthenticationMode]::Default
$ctx.Credentials = New-Object Microsoft.SharePoint.Client.SharePointOnlineCredentials($loginname, $pwd)";

        private const string AUTHENTICATION_ANONYMOUS = @"$ctx = New-Object Microsoft.SharePoint.Client.ClientContext($siteUrl)
$ctx.AuthenticationMode = [Microsoft.SharePoint.Client.ClientAuthenticationMode]::Anonymous";

        private const string AUTHENTICATION_FORMS_BASED = @"Write-Host Please enter password for $($siteUrl):
$pwd = Read-Host -AsSecureString
$ctx = New-Object Microsoft.SharePoint.Client.ClientContext($siteUrl)
$ctx.AuthenticationMode = [Microsoft.SharePoint.Client.ClientAuthenticationMode]::FormsAuthentication
$ctx.FormsAuthenticationLoginInfo = New-Object Microsoft.SharePoint.Client.FormsAuthenticationLoginInfo($loginname, $pwd)";

        private const string AUTHENTICATION_TENANT = @"Write-Host Please enter password for $($tenantUrl):
$pwd = Read-Host -AsSecureString
$ctx = New-Object Microsoft.SharePoint.Client.ClientContext($tenantUrl)
$ctx.AuthenticationMode = [Microsoft.SharePoint.Client.ClientAuthenticationMode]::Default
$ctx.Credentials = New-Object Microsoft.SharePoint.Client.SharePointOnlineCredentials($loginname, $pwd)";

        #endregion

        /// <summary>
        /// Creates a PowerShell script file (.ps1) in the users temporary folder to support PowerShell based on CSOM.
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public static string CreatePowerShellScript(SiteAuthentication site)
        {
            string tempPSFilePath = Path.Combine(Path.GetTempPath(), string.Format("SPCB_PowerShell_{0}", POWERSHELL_OPENSITE_FILENAME));

            if (File.Exists(tempPSFilePath))
                File.Delete(tempPSFilePath);

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames().SingleOrDefault(s => s.Contains(POWERSHELL_OPENSITE_FILENAME));

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string powerShellScript = reader.ReadToEnd();

                powerShellScript = ReplacePlaceHolder(powerShellScript, PLACEHOLDER_SCRIPT_LOCATION, tempPSFilePath);
                powerShellScript = ReplacePlaceHolders(site, powerShellScript);

                File.AppendAllText(tempPSFilePath, powerShellScript);
            }

            LogUtil.LogMessage("Created PowerShell script on location: {0}", tempPSFilePath);

            return tempPSFilePath;
        }

        /// <summary>
        /// Creates a PowerShell script file (.ps1) in the users temporary folder to support PowerShell based on CSOM.
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns></returns>
        public static string CreatePowerShellScript(TenantAuthentication tenant)
        {
            string tempPSFilePath = Path.Combine(Path.GetTempPath(), string.Format("SPCB_PowerShell_{0}", POWERSHELL_OPENTENANT_FILENAME));

            if (File.Exists(tempPSFilePath))
                File.Delete(tempPSFilePath);

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames().SingleOrDefault(s => s.Contains(POWERSHELL_OPENTENANT_FILENAME));

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string powerShellScript = reader.ReadToEnd();

                powerShellScript = ReplacePlaceHolder(powerShellScript, PLACEHOLDER_SCRIPT_LOCATION, tempPSFilePath);
                powerShellScript = ReplacePlaceHolders(tenant, powerShellScript);

                File.AppendAllText(tempPSFilePath, powerShellScript);
            }

            LogUtil.LogMessage("Created PowerShell script on location: {0}", tempPSFilePath);

            return tempPSFilePath;
        }

        /// <summary>
        /// Replace place holders within script.
        /// </summary>
        /// <param name="site">Site authentication data.</param>
        /// <param name="script">PowerShell script with place holders.</param>
        /// <returns>Returns PowerShell script without place holders.</returns>
        private static string ReplacePlaceHolders(SiteAuthentication site, string script)
        {
            script = ReplacePlaceHolder(script, PLACEHOLDER_CURRENT_DIRECTORY, Environment.CurrentDirectory);
            script = ReplacePlaceHolder(script, PLACEHOLDER_SITE_URL, site.Url.ToString());
            script = ReplacePlaceHolder(script, PLACEHOLDER_LOGIN_NAME, site.UserName);
            script = ReplacePlaceHolder(script, PLACEHOLDER_CLIENT_SDK, HelpUtil.GetSDKDownloadTitle());
            script = ReplacePlaceHolder(script, PLACEHOLDER_CLIENT_SDK_URL, HelpUtil.GetSDKDownloadUrl());
            script = ReplacePlaceHolder(script, PLACEHOLDER_CLIENT_SDK_VERSION, HelpUtil.GetSDKMajorVersion());

            switch (site.Authentication)
            {
                case AuthenticationMode.Default:
                    if (string.IsNullOrEmpty(site.UserName))
                        script = ReplacePlaceHolder(script, PLACEHOLDER_AUTHENTICATION, AUTHENTICATION_DEFAULT_CURRENT_USER);
                    else
                        script = ReplacePlaceHolder(script, PLACEHOLDER_AUTHENTICATION, AUTHENTICATION_DEFAULT_CUSTOM);
                    break;
                case AuthenticationMode.SharePointOnline:
                    script = ReplacePlaceHolder(script, PLACEHOLDER_AUTHENTICATION, AUTHENTICATION_SHAREPOINT_ONLINE);
                    break;
                case AuthenticationMode.Anonymous:
                    script = ReplacePlaceHolder(script, PLACEHOLDER_AUTHENTICATION, AUTHENTICATION_ANONYMOUS);
                    break;
                case AuthenticationMode.Forms:
                    script = ReplacePlaceHolder(script, PLACEHOLDER_AUTHENTICATION, AUTHENTICATION_FORMS_BASED);
                    break;
                default:
                    break;
            }

            return script;
        }

        /// <summary>
        /// Replace place holders within script.
        /// </summary>
        /// <param name="tenant">Tenant authentication data.</param>
        /// <param name="script">PowerShell script with place holders.</param>
        /// <returns>Returns PowerShell script without place holders.</returns>
        private static string ReplacePlaceHolders(TenantAuthentication tenant, string script)
        {
            script = ReplacePlaceHolder(script, PLACEHOLDER_CURRENT_DIRECTORY, Environment.CurrentDirectory);
            script = ReplacePlaceHolder(script, PLACEHOLDER_TENANT_URL, tenant.AdminUrl.ToString());
            script = ReplacePlaceHolder(script, PLACEHOLDER_LOGIN_NAME, tenant.UserName);
            script = ReplacePlaceHolder(script, PLACEHOLDER_AUTHENTICATION, AUTHENTICATION_TENANT);
            script = ReplacePlaceHolder(script, PLACEHOLDER_CLIENT_SDK, HelpUtil.GetSDKDownloadTitle());
            script = ReplacePlaceHolder(script, PLACEHOLDER_CLIENT_SDK_URL, HelpUtil.GetSDKDownloadUrl());
            script = ReplacePlaceHolder(script, PLACEHOLDER_CLIENT_SDK_VERSION, HelpUtil.GetSDKMajorVersion());

            return script;
        }

        /// <summary>
        /// Replace place holder within <paramref name="text"/>.
        /// </summary>
        /// <param name="text">Text containing the place holder.</param>
        /// <param name="key">Place holder key, which will be replaced by the <paramref name="value"/>.</param>
        /// <param name="value">Value for the place holder.</param>
        /// <returns>Returns <paramref name="text"/> with <paramref name="key"/> being replaced by the <paramref name="value"/>.</returns>
        private static string ReplacePlaceHolder(string text, string key, object value)
        {
            return text.Replace(key, value.ToString());
        }
    }
}
