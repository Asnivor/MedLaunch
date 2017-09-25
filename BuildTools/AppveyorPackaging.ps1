write-host "Starting MedLaunch pre-release packaging scripts.."

# get current directory (this will be something/something/BuildTools)
# so root would be $loc\..\
$loc = Split-Path $script:MyInvocation.MyCommand.Path
$root = (get-item $loc).parent.FullName
$releaseString = "$root\MedLaunch\bin\Release"
$lib = "$releaseString\lib"

# remove unneeded files
write-host "Removing unneeded *.xml and *.pdb files..."
$xml = get-childitem $lib -recurse
foreach ($thing in $xml)
{
    if ($thing -like "*.xml" -or $thing -like "*.pdb")
    {
        $relative = $lib.Replace($root, "") + "\$thing"
        $absolute = "$lib\$thing"
        write-host "Deleting $relative"
        #write-host "Deleting $absolute"
        
        Remove-Item -path $absolute -force
    }
    
}

write-host "Creating empty directories..."
md -force "$releaseString\Data\Games"
md -force "$releaseString\Data\Settings"
md -force "$releaseString\Data\MednafenCFGBackups"
md -force "$releaseString\Data\Updates"

# create branch ident file
write-host "Creating branch ident file..."
$identFile = "$releaseString\Data\Settings\DevStatus.txt"
$buildNo = $env:APPVEYOR_BUILD_NUMBER

New-Item $identFile -ItemType file
if ($env:APPVEYOR_REPO_BRANCH -eq "dev")
{
	$status = $buildNo
}
if ($env:APPVEYOR_REPO_BRANCH -eq "master")
{
	$status = "0"
}
$status | Set-Content $identFile

# convert changelog MD to html and save to the root of the output folder (for packaging)
. ("$loc\MarkdownConverter.ps1")

$md = "$loc\ChangeLog\default.md"
$html = ConvertFrom-Markdown -InputObject (Get-Content $md -raw)
$html | out-file "$releaseString\changelog.html" -force

# EOF   