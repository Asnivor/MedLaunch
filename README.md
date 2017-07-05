<img src="https://medlaunch.info/MedLaunch_sm.png" height="80" />
# MedLaunch
## A Windows Front-End for Mednafen


<a href="https://medlaunch.info/user/pages/03.screenshots/games01.png"><img src="https://medlaunch.info/user/pages/03.screenshots/games01.png" width="150" /></a>
<a href="https://medlaunch.info/user/pages/03.screenshots/games02.png"><img src="https://medlaunch.info/user/pages/03.screenshots/games02.png" width="150" /></a>
<a href="https://medlaunch.info/user/pages/03.screenshots/games06.png"><img src="https://medlaunch.info/user/pages/03.screenshots/games06.png" width="150" /></a>
<a href="https://medlaunch.info/user/pages/03.screenshots/games03.png"><img src="https://medlaunch.info/user/pages/03.screenshots/games03.png" width="150" /></a>
<a href="https://medlaunch.info/user/pages/03.screenshots/games04.png"><img src="https://medlaunch.info/user/pages/03.screenshots/games04.png" width="150" /></a>
<a href="https://medlaunch.info/user/pages/03.screenshots/games05.png"><img src="https://medlaunch.info/user/pages/03.screenshots/games05.png" width="150" /></a>
<a href="https://medlaunch.info/user/pages/03.screenshots/configs01.png"><img src="https://medlaunch.info/user/pages/03.screenshots/configs01.png" width="150" /></a>
<a href="https://medlaunch.info/user/pages/03.screenshots/configs02.png"><img src="https://medlaunch.info/user/pages/03.screenshots/configs02.png" width="150" /></a>
<a href="https://medlaunch.info/user/pages/03.screenshots/configs03.png"><img src="https://medlaunch.info/user/pages/03.screenshots/configs03.png" width="150" /></a>
<a href="https://medlaunch.info/user/pages/03.screenshots/configs04.png"><img src="https://medlaunch.info/user/pages/03.screenshots/configs04.png" width="150" /></a>
<a href="https://medlaunch.info/user/pages/03.screenshots/controls01.png"><img src="https://medlaunch.info/user/pages/03.screenshots/controls01.png" width="150" /></a>
<a href="https://medlaunch.info/user/pages/03.screenshots/controls02.png"><img src="https://medlaunch.info/user/pages/03.screenshots/controls02.png" width="150" /></a>
<a href="https://medlaunch.info/user/pages/03.screenshots/controls03.png"><img src="https://medlaunch.info/user/pages/03.screenshots/controls03.png" width="150" /></a>
<a href="https://medlaunch.info/user/pages/03.screenshots/controls04.png"><img src="https://medlaunch.info/user/pages/03.screenshots/controls04.png" width="150" /></a>
<a href="https://medlaunch.info/user/pages/03.screenshots/settings01.png"><img src="https://medlaunch.info/user/pages/03.screenshots/settings01.png" width="150" /></a>
<a href="https://medlaunch.info/user/pages/03.screenshots/settings02.png"><img src="https://medlaunch.info/user/pages/03.screenshots/settings02.png" width="150" /></a>
<a href="https://medlaunch.info/user/pages/03.screenshots/settings03.png"><img src="https://medlaunch.info/user/pages/03.screenshots/settings03.png" width="150" /></a>
<a href="https://medlaunch.info/user/pages/03.screenshots/settings04.png"><img src="https://medlaunch.info/user/pages/03.screenshots/settings04.png" width="150" /></a>
<a href="https://medlaunch.info/user/pages/03.screenshots/help01.png"><img src="https://medlaunch.info/user/pages/03.screenshots/help01.png" width="150" /></a>
<a href="https://medlaunch.info/user/pages/03.screenshots/help02.png"><img src="https://medlaunch.info/user/pages/03.screenshots/help02.png" width="150" /></a>
<a href="https://medlaunch.info/user/pages/03.screenshots/misc01.png"><img src="https://medlaunch.info/user/pages/03.screenshots/misc01.png" width="150" /></a>
<a href="https://medlaunch.info/user/pages/03.screenshots/misc02.png"><img src="https://medlaunch.info/user/pages/03.screenshots/misc02.png" width="150" /></a>
<a href="https://medlaunch.info/user/pages/03.screenshots/misc03.png"><img src="https://medlaunch.info/user/pages/03.screenshots/misc03.png" width="150" /></a>

[https://medlaunch.info/](https://medlaunch.info/)

**MedLaunch** is a .NET (Windows only) front-end for the excellent [Mednafen](http://mednafen.fobby.net/) multi-system emulator.

Latest Version: 0.5.7.0
Compatible with Mednafen: 0.9.39.x - 0.9.45.x

### Features
* Responsive GUI that allows for a wide range of monitor resolutions
* No installation required
* Local (SQLite) auto-generated database where all settings are saved
* Supports Mednafen versions 0.9.39.x through 0.9.45.x
* Nearly all Mednafen command line parameters available and configurable
* Mednafen controller configuration available using DirectInput and XInput (changes are saved directly to the mednafen config file rather than MedLaunch database)
* Built-in games library (with system filters and dynamic search)
* ROM scanner (for games library import) with NOINTRO/TOSEC matching
* Manual and Auto disc import (disc games must be in their own sub-folders within the system (PSX, SS, PCFX or PCECD) folder)
* DISC scanner (for games library import) with custom DAT matching based on game serial number (detected from disc image)
* Auto-M3U platlist generation for multi-disc games
* Supports the usual Mednafen rom and disc formats
* Supports multiple ROM files per archive (7zip and zip) - (no archives within archives and no subfolders within the archive)
* Games library sidebar for game info, stats and media (with the option to hide certain columns on a per-system basis)
* Scraping of game data and media from thegamesdb.net and mobygames
* Built-in netplay server selection
* Built-in browser control with links to Mednafen help pages
* Ability to hide mednafen cores completely 
* Customizable color scheme

This upcoming changes (and many others) can be found in the current roadmap [here](http://medlaunch.asnitech.co.uk/roadmap).

### Requirements
* [Microsoft .NET Framework 4.5.2](https://www.microsoft.com/en-gb/download/details.aspx?id=42643)
* At least [Mednafen versions 0.9.39.x through 0.9.45.x](https://mednafen.github.io/releases/) - 64-bit version required for Saturn games
* Windows 7 and above (may work on Vista but has not been tested)
* x64 (has not been tested on x86 but should still work on it)

### Download
You can always get the latest release build of MedLaunch on the [GitHub Releases](https://github.com/Asnivor/MedLaunch/releases) page. I am actively working on code in the [Dev Branch](https://github.com/Asnivor/MedLaunch/tree/dev) and you can see the active changes for the next release on the [ChangeLog](https://medlaunch.info/changelog). These pre-release changes will be released when I am happy with them, so please do not ask for builds ahead of the official releases. You can of course get yourself a copy of Visual Studio Community installed and build it yourself from the Master branch.

! You can download the latest version of Mednafen from the [**Mednafen website**](https://mednafen.github.io/releases/).

#### New Install
* Extract this release to a folder, run the 'MedLaunch.exe' executable and choose your Mednafen directory (must be 64-bit version if you want to emulate Saturn games).
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
Check out the [**Releases**](https://medlaunch.info/releases) page or the project on [**GitHub**](https://github.com/Asnivor/MedLaunch).

## Build From Source
The project has been built using Visual Studio Community 2015. It may or may not work with previous versions.
* Clone the Dev (or Master) branch (or download as zip)
* Open MedLaunch.sln in VisualStudio
* Restore NuGet packages
* Cross your fingers?
