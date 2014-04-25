param($installPath, $toolsPath, $package, $project)

Write-Host 'Setting up BuildTools...'
$solutionPath = (Get-Item $installPath).Parent.Parent.FullName
$targetsPath = "$solutionPath\.buildtools\BuildTools.targets"

# Make the path to the targets file relative...
$projectUri = new-object Uri('file://' + $project.FullName)
$targetsUri = new-object Uri('file://' + $targetsPath)
$relativePath = $projectUri.MakeRelativeUri($targetsUri).ToString().Replace([System.IO.Path]::AltDirectorySeparatorChar, [System.IO.Path]::DirectorySeparatorChar)
 
# Add the properties if they're not present...
Add-Type -AssemblyName 'Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
$msbuildProject =  [Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection.GetLoadedProjects($project.FullName) | Select-Object -First 1
$majorVersionProperty = $msbuildProject.Xml.Properties | Where-Object { $_.Name -eq 'MajorVersion' }
if (!$majorVersionProperty) {
	Write-Host 'Adding MajorVersion property...'
	$msbuildProject.Xml.AddProperty('MajorVersion', '1')
}
$majorVersionProperty = $msbuildProject.Xml.Properties | Where-Object { $_.Name -eq 'MinorVersion' }
if (!$majorVersionProperty) {
	Write-Host 'Adding MinorVersion property...'
	$msbuildProject.Xml.AddProperty('MinorVersion', '0')
}

# Add the BuildTools.targets file if necessary...
$import = $msbuildProject.Xml.Imports | Where-Object { $_.Project.EndsWith('BuildTools.targets') }
if (!$import) {
	Write-Host 'Referencing BuildTools MSBuild targets file...'
	$msbuildProject.Xml.AddImport($relativePath)
}

# Save the changes...
$project.Save()
$msbuildProject.Save()

