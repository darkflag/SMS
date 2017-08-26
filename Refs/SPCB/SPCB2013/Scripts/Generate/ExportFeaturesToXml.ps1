Add-PSSnapin microsoft.sharepoint.powershell -ea 0

$features = Get-SPFeature | ?{$_.GetTitle([Threading.Thread]::CurrentThread.CurrentCulture).Replace("`"", "`\`"") -like "*nintex*"}
foreach ($feature in $features) {
    $displayName = $feature.GetTitle([Threading.Thread]::CurrentThread.CurrentCulture).Replace("`"", "`\`"")
    $internalName = $feature.RootDirectory.Replace("C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14\Template\Features\", "")

    Write-Host "`<SPFeature`>"
    Write-Host "`<Id`>$($feature.Id)`<`/Id`>"
    Write-Host "`<DisplayName`>$($displayName)`<`/DisplayName`>"
    Write-Host "`<InternalName`>$($internalName)`<`/InternalName`>"
    Write-Host "`<Hidden`>$($feature.Hidden.ToString().ToLower())`<`/Hidden`>"
    Write-Host "`<Scope`>$($feature.Scope)`<`/Scope`>"
    Write-Host "`</SPFeature`>"
}