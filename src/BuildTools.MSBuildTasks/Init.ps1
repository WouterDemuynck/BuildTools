param($installPath, $toolsPath, $package, $project)

# Get the root folder of the current solution.
$solutionPath = (Get-Item $installPath).Parent.Parent.FullName
Write-Host "Installing BuildTools in the solution..."

# Create the .buildtools folder if it doesn't exist yet.
$buildtoolsFolder = "$solutionPath\.buildtools\"
if (!(Test-Path $buildtoolsFolder)) {
	MkDir $buildtoolsFolder
}

# Copy the required files to the .buildtools folder.
Copy-Item (Join-Path $toolsPath 'BuildTools.dll') $buildtoolsFolder -Force
Copy-Item (Join-Path $toolsPath 'BuildTools.MSBuildTasks.dll') $buildtoolsFolder -Force
Copy-Item (Join-Path $toolsPath 'BuildTools.targets') $buildtoolsFolder -Force

Write-Host "Creating solution items..."
$solution = Get-Interface $dte.Solution ([EnvDTE80.Solution2])
 
# Create the solution folder.
$buildtoolsSolutionFolder = $solution.Projects | where-object { $_.ProjectName -eq ".buildtools" } | select -first 1
if(!$buildtoolsSolutionFolder) {
	$buildtoolsSolutionFolder = $solution.AddSolutionFolder(".buildtools")
}

# Add the solution folder items.
$buildtoolsSolutionFolderItems = Get-Interface $buildtoolsSolutionFolder.ProjectItems ([EnvDTE.ProjectItems])
 
$buildtoolsSolutionFolderItems.AddFromFile((Join-Path $buildtoolsFolder 'BuildTools.dll'))
$buildtoolsSolutionFolderItems.AddFromFile((Join-Path $buildtoolsFolder 'BuildTools.MSBuildTasks.dll'))
$buildtoolsSolutionFolderItems.AddFromFile((Join-Path $buildtoolsFolder 'BuildTools.targets'))
