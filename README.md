# MedLaunch
## A Windows Front-End for Mednafen

<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/games1.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/games1.png" width="150" /></a>
<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/games2.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/games2.png" width="150" /></a>
<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/games3.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/games3.png" width="150" /></a>
<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/games4.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/games4.png" width="150" /></a>
<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/games6.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/games6.png" width="150" /></a>

<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/games5.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/games5.png" width="150" /></a>
<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/games7.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/games7.png" width="150" /></a>
<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/configs1.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/configs1.png" width="150" /></a>
<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/configs2.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/configs2.png" width="150" /></a>
<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/configs3.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/configs3.png" width="150" /></a>

<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/settings1.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/settings1.png" width="150" /></a>
<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/settings2.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/settings2.png" width="150" /></a>
<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/settings3.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/settings3.png" width="150" /></a>
<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/settings4.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/settings4.png" width="150" /></a>
<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/settings5.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/settings5.png" width="150" /></a>


<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/settings6.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/settings6.png" width="150" /></a>
<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/settings7.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/settings7.png" width="150" /></a>
<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/updates1.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/updates1.png" width="150" /></a>
<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/updates3.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/updates3.png" width="150" /></a>
<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/help1.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/help1.png" width="150" /></a>

[http://medlaunch.asnitech.co.uk/](http://medlaunch.asnitech.co.uk/)

**MedLaunch** is a .NET (Windows only) front-end for the excellent [Mednafen](http://mednafen.fobby.net/) multi-system emulator. I'm not a developer by profession so it started as a means to learn a bit more about C# .NET and to start learning about Windows Presentation Foundation (WPF). It really has been a steep learning curve and as such, there are a vast swathe of things in the code that are certainly not 'best-practice'. Top of this list is the fact that I have not used the 'Model-View-ViewModel' design pattern (MVVM) which means the source is pretty hard to decipher in places. I may or may not address this with a major re-write in the future.

! Please Note: You should consider this software alpha. There are a number of things that are not yet implemented properly (or in some cases not implemented at all). Some of these can be found in the '**Broken Features**' & '**To Do**' sections further down this page.

### Features
* Responsive GUI that allows for a wide range of monitor resolutions
* No installation required
* Local (SQLite) auto-generated database where all settings are saved
* Built-in games library (with system filters and dynamic search)
* ROM scanner (for games library import)
* Scraping of game data and media from thegamesdb.net
* Games library sidebar for game info, stats and media
* Manual import of disk-based games (both single and multiple disk games with auto-m3u playlist generation)
* All Mednafen command line parameters available and configurable
* So far uses only Mednafen command line options (and not local configuration files)
* Optional per-system configuration options
* Built-in browser control with links to Mednafen help pages
* Built-in static netplay server selection along with the ability to specify a custom Mednafen netplay server to connect to
* Customizable launcher color scheme

### Broken Features
* The launcher currently only handles absolute paths correctly. Relative path handling may or may not work but this part of the code needs a proper overhaul

### To Do
* Code option to import existing configuration information from Mednafen config files (default & system specific configs) into the MedLaunch database
* Option to export config information to Mednafen config files
* Configure controllers from within MedLaunch
* Option to bypass all MedLaunch command line parameters (and just use already existing Mednafen cfg files)

This upcoming changes (and many others) can be found in the current roadmap [here](http://medlaunch.asnitech.co.uk/roadmap).

### Requirements
* [Microsoft .NET Framework 4.5.2](https://www.microsoft.com/en-gb/download/details.aspx?id=42643)
* At least [Mednafen version 0.9.39.1](http://mednafen.fobby.net/releases/) - 64-bit version required for Saturn games
* Windows 7 and above (may work on Vista but has not been tested)
* x64 (has not been tested on x86 but should still work on it)

### Download
You can always get the latest alpha release build of MedLaunch on the [GitHub Releases](https://github.com/Asnivor/MedLaunch/releases) page. I am actively working on code in the [Master Branch](https://github.com/Asnivor/MedLaunch/tree/master) and you can see the active changes for the next release on the [ChangeLog](http://medlaunch.asnitech.co.uk/changelog). These pre-release changes will be released when I am happy with them, so please do not ask for builds ahead of the official releases. You can of course get yourself a copy of Visual Studio Community installed and build it yourself from the Master branch. 

### Setup
* Download the latest MedLaunch release (if one is available) and extract this somewhere
* Run MedLaunch.exe and select the location of your Mednafen folder

! It is recommended that you use a brand new Mednafen instance for the time being. You can download the latest version of Mednafen from the [**Mednafen website**](http://mednafen.fobby.net/releases/). If you do not do this you might experience issues with already existing system specific configuration files in the Mednafen folder. Specifically, these will override base settings in MedLaunch until said system configuration is enabled in the launcher. This will be addressed in future releases.

### Upgrade
* Extract the latest MedLaunch release over your existing MedLaunch folder.
* Run the MedLaunch application

## Build From Source
The project has been built using Visual Studio Community 2015. It may or may not work with previous versions.
* Clone the Master branch (or download as zip)
* Open MedLaunch.sln in VisualStudio
* Restore NuGet packages
