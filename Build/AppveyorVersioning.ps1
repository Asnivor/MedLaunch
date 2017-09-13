write-host "Starting MedLaunch pre-build scripts.."

# get current directory (this will be something/something/Build)
# so root would be $loc\..\
$loc = Split-Path $script:MyInvocation.MyCommand.Path
$root = (get-item $loc).parent.FullName

# get fileversion from assemblyinfo
$asi = get-content $root\MedLaunch\Properties\AssemblyInfo.cs
foreach ($line in $asi)
{
    if ($line -like '*AssemblyFileVersion(*')
    {
        $version = $line.Replace("[assembly: AssemblyFileVersion(", "").Replace(")]", "").Replace("`"", "").Replace(".", "_")
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
        $filename = "MedLaunch_v$($version)-DEVBUILD_$($buildNo).zip"
        $buildVer = $filename.Replace(".zip", "")
        # set enviroment version
        $env:APPVEYOR_BUILD_VERSION = $buildVer
        # set custom environment filename
        $env:ML_ARTIFACT_NAME=$filename
    }
    if ($env:APPVEYOR_REPO_BRANCH -eq "master")
    {
        $buildNo = $env:APPVEYOR_BUILD_NUMBER
        write-host "master branch detected"
        $filename = "MedLaunch_v$($version).zip"
        $tmp = $filename.Replace(".zip", "")
        $buildVer = "$($tmp)-MASTER_$($buildNo)"
        # set enviroment version
        $env:APPVEYOR_BUILD_VERSION = $buildVer
        # set custom environment filename
        $env:ML_ARTIFACT_NAME=$filename
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