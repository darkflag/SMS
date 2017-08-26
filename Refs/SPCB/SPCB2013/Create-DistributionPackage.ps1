# Script location needs to be the BIN folder

# Constants
$SPCB_2010_MAJOR_VERSION = 0
$SPCB_2013_MAJOR_VERSION = 1
$SPCB_2016_MAJOR_VERSION = 2
$SPCB_ONLINE_MAJOR_VERSION = 3

function Zip-Directory {
    Param(
      [Parameter(Mandatory=$True)][string]$DestinationFileName,
      [Parameter(Mandatory=$True)][string]$SourceDirectory,
      [Parameter(Mandatory=$False)][string]$CompressionLevel = "Optimal",
      [Parameter(Mandatory=$False)][switch]$IncludeParentDir
    )
    Add-Type -AssemblyName System.IO.Compression.FileSystem
    $CompressionLevel    = [System.IO.Compression.CompressionLevel]::$CompressionLevel  
    [System.IO.Compression.ZipFile]::CreateFromDirectory($SourceDirectory, $DestinationFileName, $CompressionLevel, $IncludeParentDir)
}

# Variables
$majorVersion = [System.Diagnostics.FileVersionInfo]::GetVersionInfo((Get-Item SPCB.exe).FullName).ProductMajorPart
$minorVersion = [System.Diagnostics.FileVersionInfo]::GetVersionInfo((Get-Item SPCB.exe).FullName).ProductMinorPart
$version = "$majorVersion.$minorVersion"

if ($majorVersion -eq $SPCB_2013_MAJOR_VERSION) {
    $releaseName = "SharePoint 2013 Client Browser v$($version)"
}
elseif($majorVersion -eq $SPCB_2016_MAJOR_VERSION) {
    $releaseName = "SharePoint 2016 Client Browser v$($version)"
}
elseif($majorVersion -eq $SPCB_ONLINE_MAJOR_VERSION) {
    $releaseName = "SharePoint Online Client Browser v$($version)"
}

$folderDistr = "distr"
$folderPath = "$folderDistr\$releaseName"

# Create folder, delete first if exists
if (Test-Path $folderPath) {
    Remove-Item -Path $folderPath -Force:$true -Recurse:$true
}
New-Item -Name $folderPath -ItemType directory

# Copy all files
Get-ChildItem * -include *.dll,*.exe, FeatureDefinitions.xml -Exclude *.vshost.exe | Copy-Item -Destination $folderPath
Get-ChildItem ..\..\..\_Assemblies\MSO\* -include *.dll | Copy-Item -Destination $folderPath

if ($majorVersion -eq $SPCB_2013_MAJOR_VERSION) {
    Get-ChildItem ..\..\..\_Assemblies\15.0\* -include *.dll | Copy-Item -Destination $folderPath
}
elseif($majorVersion -eq $SPCB_2016_MAJOR_VERSION) {
    Get-ChildItem ..\..\..\_Assemblies\16.0\* -include *.dll | Copy-Item -Destination $folderPath
}
elseif($majorVersion -eq $SPCB_ONLINE_MAJOR_VERSION) {
    Get-ChildItem ..\..\..\_Assemblies\16.1\* -include *.dll | Copy-Item -Destination $folderPath
}

# Create ZIP file
$zipFilePath = "$((Get-Item $folderDistr).FullName)\$releaseName.zip"
if (Test-Path $zipFilePath) {
    Remove-Item -Path $zipFilePath -Force:$true -Recurse:$true
}
Zip-Directory -DestinationFileName $zipFilePath -SourceDirectory (Get-Item $folderPath).FullName