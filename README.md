# MedLaunch
## A Windows Front-End for Mednafen

<a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/GameLibrary.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/GameLibrary.png" width="150" /></a><a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/RomScanComplete.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/RomScanComplete.png" width="150" /></a><a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/Configs.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/Configs.png" width="150" /></a><a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/Paths.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/Paths.png" width="150" /></a><a href="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/GameLaunch.png"><img src="http://medlaunch.asnitech.co.uk/user/pages/03.screenshots/GameLaunch.png" width="150" /></a>

[http://medlaunch.asnitech.co.uk/](http://medlaunch.asnitech.co.uk/)

**MedLaunch** is a .NET (Windows only) front-end for the excellent [Mednafen](http://mednafen.fobby.net/) multi-system emulator. I'm not a developer by profession so it started as a means to learn a bit more about C# .NET and to start learning about Windows Presentation Foundation (WPF). It really has been a steep learning curve and as such, there are a vast swathe of things in the code that are certainly not 'best-practice'. Top of this list is the fact that I have not used the 'Model-View-ViewModel' design pattern (MVVM) which means the source is pretty hard to decipher in places. I may or may not address this with a major re-write in the future.

! Please Note: You should consider this software very much pre-alpha. There are a number of core things that are not yet implemented properly (or in some cases not implemented at all). Some of these can be found in the '**Broken Features**' & '**To Do**' sections further down this page.

### Features
* Responsive GUI that allows for a wide range of monitor resolutions (although bigger is always better)
* No installation required
* Local (SQLite) auto-generated database where all settings are saved
* Built-in games library (with system filters and dynamic search)
* ROM scanner (for games library import)
* (Nearly) All Mednafen command line parameters available and configurable
* So far uses only Mednafen command line options (and not local configuration files)
* Optional per-system configuration options
* Built-in browser control with links to Mednafen help pages
* Built-in static netplay server selection along with the ability to specify a custom Mednafen netplay server to connect to

### Broken Features
* MedLaunch currently **only works for ROM based systems** as the CD image scanning import function has yet to be designed & implemented. Getting the disk-based systems (PSX, SS, PCFX etc) into the games library is currently the number one priority
* The launcher currently only handles absolute paths correctly. Relative path handling may or may not work but this part of the code needs a proper overhaul
* The 'About' tab is currently empty

### To Do
* Implement scanning (and importing into the games library) of disk images. This should include auto-detection of multi-disk games (and auto-generation of .m3u files)
* Implement data scraping from external sources (thegamesdb etc.)
* Add debugger config options
* Auto-hide system specific config option controls that are invalid (eg, nes.forcemono etc.)
* Add data to 'About' page
* Code option to import existing configuration information from Mednafen config files (default & system specific configs) into the MedLaunch database

### Requirements
* [Microsoft .NET Framework 4.5.2](https://www.microsoft.com/en-gb/download/details.aspx?id=42643)
* At least [Mednafen version 0.9.39.1](http://mednafen.fobby.net/releases/) 
* Windows 7 and above (may work on Vista but has not been tested)
* x64 (has not been tested on x86 but should still work on it)

### Setup
* Download the latest MedLaunch release (if one is available) and extract this somewhere
* Run MedLaunch.exe and select the location of your Mednafen folder

! It is recommended that you use a brand new Mednafen instance for the time being. You can download the latest version of Mednafen from the [**Mednafen website**](http://mednafen.fobby.net/releases/). If you do not do this you might experience issues with already existing system specific configuration files in the Mednafen folder. Specifically, these will override base settings in MedLaunch until said system configuration is enabled in the launcher. This will be addressed in future releases.

## Build From Source
The project has been built using Visual Studio Community 2015. It may or may not work with previous versions.
* Clone the Master branch (or download as zip)
* Open MedLaunch.sln in VisualStudio
* Restore NuGet packages
