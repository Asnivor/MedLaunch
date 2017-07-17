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

Bugs/Suggestions/Feature Requests can be posted on the GitHub [Issue Tracker](https://github.com/Asnivor/MedLaunch/issues), or connect to our [Discord Server](https://discord.gg/nsbanNa) and let us know there.

### Features
* **No installation required (and no data is written outside of the MedLaunch folder)**
* **Local (SQLite) auto-generated database where all settings are saved**
* **Can import existing mednafen configuration options**
* **Responsive UI**
  * Customizable color scheme
  * Scaleable to aid usability on a wide range of resolutions
* **Supports versions of Mednafen 0.9.39.x - 0.9.45.x (latest)**
* **Nearly all Mednafen config parameters are supported and configurable through the launcher**
  * With the exception of control configuration parameters, all config settings are stored internally on per-emulated system basis
  * On game launch a {system}.cfg file is generated (so that per-system configurations can be utilized outside of MedLaunch)
* **Built-in update checking (with manual ability to grab the latest MedLaunch and supported Mednafen x64 binaries)**
* **Customizable games library**
  * Hide/show individual mednafen emulated systems and games library columns
  * Multi-column sorting and dynamic search
  * Library sorting and column sizing/positioning state saved on a per-filter basis
  * Sidebar that shows game information, boxart, screenshots, manuals etc. (once the game has been scraped)
* **Auto scan and import of ROM based games**
  * Imports all standard ROM formats that mednafen supports
  * Extended archive support - will import single or multiple ROMs from within .zip or .7z files
* **Auto scan and import of Disc based games (PSX, Saturn, PCFX & PCE-CD)**
  * All disc cue/image files must reside in game-specific sub-folders below the designated system game folder
  * Games that do not adhere to this file/folder structure can be imported manually
  * Auto-generation of .m3u playlist files for multi-disc games
* **DAT data lookup on import**
  * MedLaunch ships with a DAT database that combines NoIntro, Tosec, PsxDataCenter & Satakore information that enables detailed information (Country, Year, Publisher etc) to be populated in the games library upon import
  * ROM (and some disc-based system) files are matched based on MD5 hash
  * PSX and Saturn discs are matched based on game serial number lookup (that is obtained by interogating the disc image itself)
* **Online scraping of game data/media/docs**
  * Auto and manual scraping of data from thegamesdb and mobygames
  * PDF manuals downloaded from replacementdocs
* **Configuration of mednafen controls**
  * DirectInput and XInput are fully supported - covering keyboard, standard gamepads/joysticks and XInput devices (xbox/playstation controllers etc.)
  * Configure nearly all emulated gamepad/joystick/wheel etc. devices for all virtual input ports for every emulated system
  * Standard mouse bindings can be inserted using pre-defined templates
* **Extended game launch functionality**
  * Last played and total game time stats recorded on a per-game basis
  * Option to remember mednafen window/screen position (on a per-emulated system basis) and re-apply this on game launch (useful for people with multi-monitor setups)
  * Choose the CD image that should be inserted when mednafen starts (in the case of multi-disc games)
  * Edit the mednafen launch string before running the game
  * Option to copy launch string to clipboard (for use in other frontends or batch files)
* **Built-in netplay server list (with pre-populated servers)**
* **Browser control with links to the mednafen and medlaunch websites**

This upcoming changes (and many others) can be found in the current roadmap [here](http://medlaunch.asnitech.co.uk/roadmap).

### Requirements
* [Microsoft .NET Framework 4.5.2](https://www.microsoft.com/en-gb/download/details.aspx?id=42643)
* At least [Mednafen version 0.9.39.x](https://mednafen.github.io/releases/) - 64-bit version required for Saturn games
* Windows 7 and above (may work on Vista but has not been tested)
* x64 (has not been tested on x86 but should still work on it)

### Download
You can always get the latest release build of MedLaunch on the [GitHub Releases](https://github.com/Asnivor/MedLaunch/releases/latest) page. I am actively working on code in the [Dev Branch](https://github.com/Asnivor/MedLaunch/tree/dev) and you can see the active changes for the next release on the [ChangeLog](https://medlaunch.info/changelog). These pre-release changes will be released when I am happy with them, so please do not ask for builds ahead of the official releases. You can of course get yourself a copy of Visual Studio Community installed and build it yourself from the Master branch.

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
