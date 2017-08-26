<#
	Launch Windows PowerShell: http://technet.microsoft.com/en-us/magazine/ff629472.aspx
	- powershell.exe -NoExit -File "..\OpenSite.ps1"

	Placeholders:
	- [[CurrentDirectory]]
	- [[SiteUrl]]
	- [[LoginName]]
	- [[ScriptLocation]]
	- [[AppId]]

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
$siteUrl = "[[SiteUrl]]"
$loginname = "[[LoginName]]"
$scriptLocation = "[[ScriptLocation]]"
$appId = "[[AppId]]"

Set-Location $loc

Load-Assembly "Microsoft.SharePoint.Client.dll"

[[Authentication]]

$web = $ctx.Web 
$ctx.Load($web) 

$app = $web.GetAppInstanceById($appId)
$ctx.Load($app) 

$errors = $app.GetErrorDetails()
$ctx.Load($errors) 

$ctx.ExecuteQuery() 

Write-Host ""
Write-Host ""
Write-Host "App instance details:"
Write-Host $app
Write-Host ""
Write-Host "App error details (if any):"
Write-Host $errors