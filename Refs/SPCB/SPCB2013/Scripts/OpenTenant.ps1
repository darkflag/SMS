<#
	Launch Windows PowerShell: http://technet.microsoft.com/en-us/magazine/ff629472.aspx
	- powershell.exe -NoExit -File "..\OpenTenant.ps1"

	Placeholders:
	- CurrentDirectory: [[CurrentDirectory]]
	- TenantUrl: [[TenantUrl]]
	- LoginName: [[LoginName]]
	- ScriptLocation: [[ScriptLocation]]
	- ClientSDK: [[ClientSDK]]
	- ClientSDKUrl: [[ClientSDKUrl]]
	- ClientSDKVersion: [[ClientSDKVersion]]

	Authentication placeholder:
	- Authentication: [[Authentication]]

#>

function Load-Assembly([string]$assemblyName, [string]$sdkVersion) {
    $isapiPath = "$($env:CommonProgramFiles)\Microsoft Shared\Web Server Extensions\$($sdkVersion)\ISAPI"

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

$loc = "[[CurrentDirectory]]"
$tenantUrl = "[[TenantUrl]]"
$loginname = "[[LoginName]]"
$scriptLocation = "[[ScriptLocation]]"
$clientSDK = "[[ClientSDK]]"
$clientSDKUrl = "[[ClientSDKUrl]]"
$clientSDKVersion = "[[ClientSDKVersion]]"

Write-Warning "Optionally you need the $clientSDK, $clientSDKUrl"
Write-Host ""

Set-Location $loc

Load-Assembly "Microsoft.SharePoint.Client.dll"
Load-Assembly "Microsoft.SharePoint.Client.Runtime.dll"
Load-Assembly "Microsoft.Online.SharePoint.Client.Tenant.dll"

[[Authentication]]

$tenant = New-Object Microsoft.Online.SharePoint.TenantAdministration.Tenant($ctx)
$web = $ctx.Web 
$ctx.Load($web) 
$ctx.Load($tenant) 
$ctx.ExecuteQuery() 

Write-Host ""
Write-Host ""
Write-Host "HOW TO"
Write-Host ""
Write-Host "Variables:"
Write-Host " `$tenantUrl: URL for tenant administration site collection"
Write-Host " `$ctx: The client context for current tenant"
Write-Host " `$web: The current web within the Tenant Administration site"
Write-Host " `$tenant: The Office 365 tenant object"
Write-Host ""
Write-Host "Sample code:"
Write-Host " `$web = `$ctx.Web"
Write-Host " `$ctx.Load(`$web)"
Write-Host " `$ctx.Load(`$tenant)"
Write-Host " `$ctx.ExecuteQuery()"
Write-Host " Write-Host `" Current web title is '`$(`$web.Title)', `$(`$web.Url)`""
Write-Host " Write-Host `" Current tenant RootSiteUrl is '`$(`$tenant.RootSiteUrl)'`""
Write-Host ""
Write-Host "Output:"
Write-Host " Current web title is '$($web.Title)', $($web.Url)"
Write-Host " Current tenant RootSiteUrl is '$($tenant.RootSiteUrl)'"
Write-Host ""