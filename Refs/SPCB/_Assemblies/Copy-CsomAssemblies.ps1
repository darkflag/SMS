# Copies the assemblies from the NuGet package to the _Assemblies folder.
# 
# Include in the Post-Build events of the CSOM projects.

function Copy-CsomAssemblies
{
	param(
		[Parameter(Position=0)]
		[ValidateSet('15.0','16.0','16.1')]
		[String]$SharePointVersion,

		[Parameter(Position=1)]
		[String]$SolutionPath
	)

	# Set variables
	switch ($SharePointVersion)
	{
		'15.0' {$directoryFilter="Microsoft.SharePoint2013.CSOM*"}
		'16.0' {$directoryFilter="Microsoft.SharePoint2016.CSOM*"}
		'16.1' {$directoryFilter="Microsoft.SharePointOnline.CSOM*"}
	}

	# Get CSOM assemblies
	$csomPath = Get-ChildItem -Path "$solutionPath\packages\" -Directory | ?{ $_.Name -like $directoryFilter } | Select-Object -Last 1

	# Copy CSOM assemblies
	$source = "$($csomPath.Fullname)\lib\net45\*.dll"
	$destination = "$solutionPath\_Assemblies\$SharePointVersion"
	Copy-Item $source $destination -Verbose
}

Write-Host "Num Args:" $args.Length
foreach($arg in $args) { Write-Host " » $arg" }

$solutionPath = $args[0]
$sharePointVersion = $args[1]

Copy-CsomAssemblies -SharePointVersion $sharePointVersion -SolutionPath $solutionPath