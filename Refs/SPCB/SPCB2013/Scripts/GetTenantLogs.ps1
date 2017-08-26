<#
	Launch Windows PowerShell: http://technet.microsoft.com/en-us/magazine/ff629472.aspx
	- powershell.exe -NoExit -File "..\OpenSite.ps1"

	Placeholders:
	- [[CurrentDirectory]]
	- [[SiteUrl]]
	- [[LoginName]]
	- [[ScriptLocation]]

	Authentication placeholder:
	- [[Authentication]]

#>

function Load-Assembly([string]$assemblyName) {
    $isapiPath = $env:CommonProgramFiles + "\Microsoft Shared\Web Server Extensions\15\ISAPI"

    if (Test-Path $assemblyName) {
        Add-Type -Path (Resolve-Path $assemblyName)
    }
    elseif (Test-Path "$isapiPath\$assemblyName") {
        Add-Type -Path "$isapiPath\$assemblyName"
    }
    else {
        [System.Reflection.Assembly]::LoadWithPartialName($assemblyName)
    }
}

Write-Warning "Optionally you need the SharePoint Server 2013 Client Components SDK, http://www.microsoft.com/en-us/download/details.aspx?id=35585"
Write-Host ""

$loc = "[[CurrentDirectory]]"
$siteUrl = "[[SiteUrl]]" # https://yoursite-admin.sharepoint.com/
$loginname = "[[LoginName]]"
$scriptLocation = "[[ScriptLocation]]"

Set-Location $loc

Load-Assembly "Microsoft.SharePoint.Client.dll"
Load-Assembly "Microsoft.SharePoint.Client.Runtime.dll"
Load-Assembly "Microsoft.Online.SharePoint.Client.Tenant.dll"

[[Authentication]]

$web = $ctx.Web 
$ctx.Load($web) 
$ctx.ExecuteQuery() 

$tenant = New-Object Microsoft.Online.SharePoint.TenantAdministration.Tenant($ctx)
$tenantLog = New-Object Microsoft.Online.SharePoint.TenantAdministration.TenantLog($ctx)
$dateTimeUTCNow = [DateTime]::UtcNow
$logEntries = $tenantLog.GetEntries((Get-Date).AddDays(-14), $dateTimeUTCNow, 50)
$ctx.Load($logEntries)
$ctx.ExecuteQuery() 
