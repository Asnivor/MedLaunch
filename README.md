<img src="http://medlaunch.asnitech.co.uk/MedLaunch_sm.png" height="80" />
# MedLaunch
## A Windows Front-End for Mednafen

<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/games01.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/games01.png" width="150" /></a>
<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/games02.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/games02.png" width="150" /></a>
<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/romscanner01.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/romscanner01.png" width="150" /></a>
<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/romscanner02.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/romscanner02.png" width="150" /></a>
<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/scraper01.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/scraper01.png" width="150" /></a>

<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/scraper02.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/scraper02.png" width="150" /></a>
<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/configs01.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/configs01.png" width="150" /></a>
<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/configs02.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/configs02.png" width="150" /></a>
<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/configs03.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/configs03.png" width="150" /></a>
<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/settings01.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/settings01.png" width="150" /></a>

<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/updates01.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/updates01.png" width="150" /></a>
<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/help01.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/help01.png" width="150" /></a>
<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/help02.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/help02.png" width="150" /></a>
<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/colorselector.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/colorselector.png" width="150" /></a>


[http://medlaunch.asnitech.co.uk/](http://medlaunch.asnitech.co.uk/)

**MedLaunch** is a .NET (Windows only) front-end for the excellent [Mednafen](http://mednafen.fobby.net/) multi-system emulator. 

! Please Note: You should consider this software beta.

### Features
* Responsive GUI that allows for a wide range of monitor resolutions
* No installation required
* Local (SQLite) auto-generated database where all settings are saved
* Built-in games library (with system filters and dynamic search)
* ROM scanner (for games library import)
* Combined NoIntro/ToSec MD5 hash auto-lookup (for populating some of the games library data on ROM import).
* Scraping of game data and media from thegamesdb.net
* Games library sidebar for game info, stats and media
* Manual import of disk-based games (both single and multiple disk games with auto-m3u playlist generation)
* All Mednafen command line parameters available and configurable
* Built-in browser control with links to Mednafen help pages
* Built-in static netplay server selection along with the ability to specify a custom Mednafen netplay server to connect to
* Customizable launcher color scheme

This upcoming changes (and many others) can be found in the current roadmap [here](http://medlaunch.asnitech.co.uk/roadmap).

### Requirements
* [Microsoft .NET Framework 4.5.2](https://www.microsoft.com/en-gb/download/details.aspx?id=42643)
* At least [Mednafen version 0.9.42.x](http://mednafen.fobby.net/releases/) - 64-bit version required for Saturn games
* Windows 7 and above (may work on Vista but has not been tested)
* x64 (has not been tested on x86 but should still work on it)

### Download
You can always get the latest release build of MedLaunch on the [GitHub Releases](https://github.com/Asnivor/MedLaunch/releases) page. I am actively working on code in the [Master Branch](https://github.com/Asnivor/MedLaunch/tree/master) and you can see the active changes for the next release on the [ChangeLog](http://medlaunch.asnitech.co.uk/changelog). These pre-release changes will be released when I am happy with them, so please do not ask for builds ahead of the official releases. You can of course get yourself a copy of Visual Studio Community installed and build it yourself from the Master branch. 

! You can download the latest version of Mednafen from the [**Mednafen website**](http://mednafen.fobby.net/releases/).

#### New Install
* Extract this release to a folder, run the 'MedLaunch.exe' executable and choose your Mednafen directory (must be the Mednafen 0.9.42.x branch and 64-bit version if you want to emulate Saturn games). 
* You are then prompted to choose whether to import all config settings from your Mednafen folder into the MedLaunch database (this is recommended).
* If you currently use system-specific config files with Mednafen the settings from these will be imported with the above process. However MedLaunch will write to these files when you launch a game - so back them up somewhere if you are not happy with this.

#### Upgrade
##### Preferred Method: 
* Automatically download and upgrade using the 'Updates' tab within MedLaunch itself.
* Once MedLaunch has upgraded, go to the 'Configs' tab and click the 'IMPORT ALL CONFIGS FROM DISK' button

##### Manual Method: 
* You can safely extract this new release over an existing MedLaunch folder (providing you do not have MedLaunch running at the time) 
* Once extracted run the MedLaunch.exe executable and your current database will be upgraded before the application starts proper.
* Once MedLaunch has upgraded, go to the 'Configs' tab and click the 'IMPORT ALL CONFIGS FROM DISK' button

### Download
Check out the [**Releases**](http://medlaunch.asnitech.co.uk/releases) page or the project on [**GitHub**](https://github.com/Asnivor/MedLaunch).

## Build From Source
The project has been built using Visual Studio Community 2015. It may or may not work with previous versions.
* Clone the Master branch (or download as zip)
* Open MedLaunch.sln in VisualStudio
* Restore NuGet packages
* Cross your fingers?
