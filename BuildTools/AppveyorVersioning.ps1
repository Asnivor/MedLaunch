write-host "...Starting MedLaunch pre-build scripts.."

# get current directory (this will be something/something/BuildTools)
# so root would be $loc\..\
$loc = Split-Path $script:MyInvocation.MyCommand.Path
$root = (get-item $loc).parent.FullName

# get fileversion from assemblyinfo
$asi = get-content $root\MedLaunch\Properties\AssemblyInfo.cs
foreach ($line in $asi)
{
    if ($line -like '*AssemblyFileVersion(*')
    {
        $versionDot = $line.Replace("[assembly: AssemblyFileVersion(", "").Replace(")]", "").Replace("`"", "")
        Set-AppVeyorBuildVariable -Name 'MEDLAUNCH_VERSION_DOT' -Value $versionDot
        
        $version = $versionDot.Replace(".", "_")
        Set-AppVeyorBuildVariable -Name 'MEDLAUNCH_VERSION_UNDERSCORE' -Value $version
    }
}
write-host "Detected file version: $version"

# run only if this is within appveyor
if ($env:APPVEYOR -eq $true)
{
    write-host "Appveyor detected"
    if ($env:APPVEYOR_REPO_BRANCH -eq "dev")
    {
        $buildNo = $env:APPVEYOR_BUILD_NUMBER
        write-host "dev branch detected"
        $filename = "MedLaunch_v$($version)-DEVBUILD-$($buildNo)"
        $buildVer = $filename.Replace(".zip", "")
        
        # set enviroment version
        Set-AppveyorBuildVariable -Name 'APPVEYOR_BUILD_VERSION' -Value $buildVer
        #$env:APPVEYOR_BUILD_VERSION = $buildVer
        
        ## update version
        $newVerDev = "ML-$versionDot-DEV-$buildNo"
        Update-AppveyorBuild -Version $newVerDev
        
        # set custom environment filename
        Set-AppveyorBuildVariable -Name 'ML_ARTIFACT_NAME' -Value $filename
        #$env:ML_ARTIFACT_NAME=$filename
    }
    if ($env:APPVEYOR_REPO_BRANCH -eq "master")
    {
        $buildNo = $env:APPVEYOR_BUILD_NUMBER
        write-host "master branch detected"
        $filename = "MedLaunch_v$($version)"
        $tmp = $filename.Replace(".zip", "")
        $buildVer = "$($tmp)-MASTER_$($buildNo)"
        
        # set enviroment version
        Set-AppveyorBuildVariable -Name 'APPVEYOR_BUILD_VERSION' -Value $buildVer
        #$env:APPVEYOR_BUILD_VERSION = $buildVer
        
        ## update version
        $newVerMas = "ML-$versionDot-MASTER-$buildNo"
        Update-AppveyorBuild -Version $newVerMas
        
        # set custom environment filename
        Set-AppveyorBuildVariable -Name 'ML_ARTIFACT_NAME' -Value $filename
        #$env:ML_ARTIFACT_NAME=$filename
    }
    
    
    # set environment variable for release description (if a release description exists)
    $rPath = "$loc\ReleaseNotes\" + $env:MEDLAUNCH_VERSION_DOT + ".md"
    if ([System.IO.File]::Exists($rPath))
    {
        $content = [System.IO.File]::ReadAllText($rPath)
        Set-AppveyorBuildVariable -Name 'MEDLAUNCH_RELEASE_DESCRIPTION' -Value $content
    }
    
}
else
{
    write-host "No Appveyor detected - ignoring branches"
    $filename = "MedLaunch_v$($version).zip"
}

write-host "Artifact name will be: $filename"
write-host "Environment variable for artifact is stored as: $env:ML_ARTIFACT_NAME"
write-host "buildversion has been set to: $buildVer"