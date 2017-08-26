using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SPBrowser.Utils
{
    public class PowerShellUtil
    {
        /// <summary>
        /// Creates a PowerShell script file (.ps1) in the users temporary folder to support PowerShell based on CSOM.
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public static string CreatePowerShellScript(SiteAuth site)
        {
            string tempPSFilePath = Path.Combine(Path.GetTempPath(), "spcb_ps_startup.ps1");

            if (File.Exists(tempPSFilePath))
                File.Delete(tempPSFilePath);

            File.AppendAllText(tempPSFilePath, string.Format("$loc = \"{0}\"\n", Environment.CurrentDirectory));
            File.AppendAllText(tempPSFilePath, string.Format("$siteUrl = \"{0}\"\n", site.Url));
            File.AppendAllText(tempPSFilePath, string.Format("$loginname = \"{0}\"\n", site.Username));

            File.AppendAllText(tempPSFilePath, "Set-Location $loc\n");
            File.AppendAllText(tempPSFilePath, "Add-Type -Path (Resolve-Path \"Microsoft.SharePoint.Client.dll\")\n");
            File.AppendAllText(tempPSFilePath, "Add-Type -Path (Resolve-Path \"Microsoft.SharePoint.Client.Runtime.dll\")\n");

            File.AppendAllText(tempPSFilePath, "$ctx = New-Object Microsoft.SharePoint.Client.ClientContext($siteUrl)\n");

            if (!string.IsNullOrEmpty(site.Username))
            {
                File.AppendAllText(tempPSFilePath, "Write-Host \"Please enter password for $($siteUrl):\"\n");
                File.AppendAllText(tempPSFilePath, "$pwd = Read-Host -AsSecureString\n");

                if (site.Authentication == AuthN.Default)
                    File.AppendAllText(tempPSFilePath, "$ctx.Credentials = New-Object System.Net.NetworkCredential($loginname, $pwd)\n");
                else
                    File.AppendAllText(tempPSFilePath, "$ctx.Credentials = New-Object Microsoft.SharePoint.Client.SharePointOnlineCredentials($loginname, $pwd)\n");
            }

            File.AppendAllText(tempPSFilePath, "$web = $ctx.Web \n");
            File.AppendAllText(tempPSFilePath, "$ctx.Load($web) \n");
            File.AppendAllText(tempPSFilePath, "$ctx.ExecuteQuery() \n");

            File.AppendAllText(tempPSFilePath, "Write-Host \"\"\n");
            File.AppendAllText(tempPSFilePath, "Write-Host \"\"\n");
            File.AppendAllText(tempPSFilePath, "Write-Host \"HOW TO\"\n");
            File.AppendAllText(tempPSFilePath, "Write-Host \"\"\n");
            File.AppendAllText(tempPSFilePath, "Write-Host \"Variables:\"\n");
            File.AppendAllText(tempPSFilePath, "Write-Host \" `$siteUrl: URL for current site collection\"\n");
            File.AppendAllText(tempPSFilePath, "Write-Host \" `$ctx: The client context for current site collection\"\n");
            File.AppendAllText(tempPSFilePath, "Write-Host \" `$web: The current web within the client context\"\n");
            File.AppendAllText(tempPSFilePath, "Write-Host \"\"\n");
            File.AppendAllText(tempPSFilePath, "Write-Host \"Sample code:\"\n");
            File.AppendAllText(tempPSFilePath, "Write-Host \" `$web = `$ctx.Web\"\n");
            File.AppendAllText(tempPSFilePath, "Write-Host \" `$ctx.Load(`$web)\"\n");
            File.AppendAllText(tempPSFilePath, "Write-Host \" `$ctx.ExecuteQuery()\"\n");
            File.AppendAllText(tempPSFilePath, "Write-Host \" Write-Host `\" Current web title is '`$(`$web.Title)', `$(`$web.Url)`\"\"\n");
            File.AppendAllText(tempPSFilePath, "Write-Host \"\"\n");
            File.AppendAllText(tempPSFilePath, "Write-Host \"Output:\"\n");
            File.AppendAllText(tempPSFilePath, "Write-Host \" Current web title is '$($web.Title)', $($web.Url)\"\n");
            File.AppendAllText(tempPSFilePath, "Write-Host \"\"");

            return tempPSFilePath;
        }
    }
}
