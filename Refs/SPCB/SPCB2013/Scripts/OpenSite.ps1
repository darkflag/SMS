<#
	Launch Windows PowerShell: http://technet.microsoft.com/en-us/magazine/ff629472.aspx
	- powershell.exe -NoExit -File "..\OpenSite.ps1"

	Placeholders:
	- CurrentDirectory: [[CurrentDirectory]]
	- SiteUrl: [[SiteUrl]]
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
$siteUrl = "[[SiteUrl]]"
$loginname = "[[LoginName]]"
$scriptLocation = "[[ScriptLocation]]"
$clientSDK = "[[ClientSDK]]"
$clientSDKUrl = "[[ClientSDKUrl]]"
$clientSDKVersion = "[[ClientSDKVersion]]"

Write-Warning "Optionally you need the $clientSDK, $clientSDKUrl"
Write-Host ""

Set-Location $loc

Load-Assembly "Microsoft.SharePoint.Client.dll" $clientSDKVersion

[[Authentication]]

$web = $ctx.Web 
$ctx.Load($web) 
$ctx.ExecuteQuery() 

Write-Host ""
Write-Host ""
Write-Host "HOW TO"
Write-Host ""
Write-Host "Variables:"
Write-Host " `$siteUrl: URL for current site collection"
Write-Host " `$ctx: The client context for current site collection"
Write-Host " `$web: The current web within the client context"
Write-Host ""
Write-Host "Sample code:"
Write-Host " `$web = `$ctx.Web"
Write-Host " `$ctx.Load(`$web)"
Write-Host " `$ctx.ExecuteQuery()"
Write-Host " Write-Host `" Current web title is '`$(`$web.Title)', `$(`$web.Url)`""
Write-Host ""
Write-Host "Output:"
Write-Host " Current web title is '$($web.Title)', $($web.Url)"
Write-Host ""