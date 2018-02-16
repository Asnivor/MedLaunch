---
title: Changelog
---

# Changelog
------------------------------------
#### Development Build Changes ([Dev](https://github.com/Asnivor/MedLaunch/tree/dev) branch) - [![Build status](https://ci.appveyor.com/api/projects/status/4maii9la7yb72bw8/branch/dev?svg=true)](https://ci.appveyor.com/project/Asnivor/medlaunch/branch/dev/artifacts)
------------------------------------
* (Enhancement)	- Background image - added 'stretch and maintain aspect' and 'original size' options ([#196](https://github.com/Asnivor/MedLaunch/issues/196))
* (BugFix) - Controller and Misc Binding configuration windows now instantiate based on the current MedLaunch window height (so that buttons at the bottom of the window should never be cut off) ([#197](https://github.com/Asnivor/MedLaunch/issues/197))
* (Enhancement) - Added shared memcard option for PSX ([#194](https://github.com/Asnivor/MedLaunch/issues/194)) - Caveat: this may not play nice with savestates, use with caution
* (BugFix) - Added better error handling when inspecting corrupt archive files ([#198](https://github.com/Asnivor/MedLaunch/issues/198))
* (BugFix) - Fixed code typo that was causing an exception when choosing pce gecdbios ([#200](https://github.com/Asnivor/MedLaunch/issues/200))

* ## Mednafen 1.21.x support ##
* * Updated LogParser to handle new Mednafen version numbering and changes in stdout.txt behaviour
* * Added setting "video.fs.display" for all cores

##### [0.5.18.5](https://medlaunch.info/releases/0-5-18-5)
###### 2018-01-15
* (BugFix)	-	Fixed UI transparency issue
* (BugFix)	-	Fixed issues with games library sidebar text 'jumping' when changing the selected game
* (Enhancement)	-	Some non-system related mednafen key bindings can now be set within MedLaunch
* (Enhancement)	-	Added detection of system for discs (discs will be added to the correct system on import, even if they are in the wrong folder)
* (Enhancement)	-	Megadrive *.smd interleaved format games can now be imported. On game launch, if interleaving is detected, the ROM will be converted into the cache folder and launched from there (this is still experimental - its easier to ditch your old *.smd roms and get something newer)
* (BugFix)	-	MegaDrive 6-button pad was missing the 'mode' button in control configuration

##### [0.5.18.1](https://medlaunch.info/releases/0-5-18-1)
###### 2017-09-25
* (BugFix)	-	New version downloads don't stall at 100% anymore (but obviously you will still have to manually update until you are running this version)
* (Enhancement)	-	Added setting to reformat game title case when displayed in the library. Options - no change, Title (Camel) Case, All Caps
* (Enhancement)	-	Context menu option to delete ROM(s) from disk - currently only ROM games that are either uncompressed, or are the only ROM file within a zip file (disc-based games and 7zipped ROMs not supported)
* (UI)	-	Re-styled the games library favourite 'tick box' so that it no longer looks terrible
* (Enhancement)	- Added the following controller configuration options: Saturn (light gun & mouse), SNES (superscope & mouse), PSX (guncon, justifier & mouse), PCE/PCE_FAST (mouse), PCFX (mouse), NES (zapper) and MD (mouse)
* (UI)	-	Controller configuration now shows 'pretty' names for Keyboard & Mouse bindings (hovering over each binding does however show the actual mednafen command in a tooltip)
* (BugFix)	-	Binding of tertiary controller settings is now possible
* (Enhancement) - Compatibility and download links for mednafen 0.9.48
* (Enhancement) - Added basic ROM/Disc inspector functionality
* (BugFix)	-	Library game info text now auto-scrolls if right boundary is breached
* (Enhancement) - Games library now converts/translates the majority of returned country codes (to give a more uniform display)
* (BugFix)	-	Fixed library sidebar scroll events not bubbling through child items (so now library sidebar will vertically scroll with mousewheel whever the mouse is located on the sidebar)
* (CodeChange)	-	Added custom DiscSN lookup library (for obtaining PSX game serials from disc images) and removed the BizHawk DLLs
* (BugFix)	-	Fixed SlimDX.dll exception on some systems - implemented pre-req check on startup with option to install vcred_x86.exe if it is not detected (required for MedLaunch to work)
* (BugFix)	-	Fixed (finally) exception generated when changing the GUI Zoom slider on non-English OSes
* (BugFix)	-	Fixed exception generated when opening the Theme Changer on non-English OSes
* (BugFix)	-	Fixed exception when importing configs on non-English OSes
* (Enhancement)	-	Implemented SevenZipSharp library in place of SharpCompress. This gives better speed when dealing with/importing games within 7zip archives.
* (BugFix)	-	ROMs within archives files now have their hash detected correctly
* (Enhancement)	-	Multiple ROMs within archive files can now reside in nested subfolders within the archive itself
* (Enhancement)	-	DAT lookup database (AsniDAT.db) now contains CRC32 and SHA1 hashes (along with MD5 that it had previously) - this reduces import/scan time for ROMs within archives
* (Enhancement)	-	Major disc-scanning improvements, just point MedLaunch at the root folder for that system (psx, saturn etc.), and it will parse and import all games (and create .m3u files if they are needed). With multi-disc games, all the files for the game must be in the same folder or subfolder
* (BugFix)	-	Fixed issue in DevBuild where mednafen update wasnt extracting all the files from the downloaded archive

##### [0.5.8.0](https://medlaunch.info/releases/0-5-8-0)
###### 2017-08-11
* (Compatibility)	-	Implemented changes for the upcoming mednafen release
* (UI Change)	-	Removed the collapsable panels within Configs, Settings & Controls (as they were pretty pointless)
* (UI Change)	-	Configs, Settings & Controls panes now stack horizontally but scroll vertically (so the middle mouse button can be used to scroll the pane up and down)
* (BugFix)	-	Added some download timeout error handling
* (Enhancement)	-	Games library sidebar width is now resizable (by clicking and dragging the edge). Selected size persists across sessions
* (Enhancement)	-	ToolTips (text and image) now stay open until user moves the mouse away
* (Enhancement)	-	Background image can now be changed to one of your choosing. You can also set the image opacity and whether it is tiled or not
* (BugFix)	-	Fonts are no longer blurry
* (BugFix)	-	Fixed exception generated when opening theme changer window on non-English OSes

##### [0.5.7.2](https://medlaunch.info/releases/0-5-7-2)
###### 2017-07-28
* (Feature)	-	Added progress bar for MedLaunch and Mednafen update downloads
* (Feature)	-	Modified MedLaunch updater to download from the official download mirror (meaning much faster update times)
* (BugFix)	-	Re-designed auto-scrape methods in an attempt to eliminate games 'missed'
* (BugFix)	-	Fixed exception generated when using the 'Unscraped Games' filter with sub-system games present in the library (FDS etc)
* (BugFix)	-	Re-added missing video.driver config setting (that went walkabout at some point in the last 5 months)
* (Change)	-	Increased dialog window show-time on game launch errors (so users can actually see what the problem was)
* (BugFix)	-	Mitigated exception on PSX game launch where game files/path had changed and game had not been rescanned
* (BugFix)	-	Mednafen version now detected if user has deleted stdout.txt (for whatever reason)
* (BugFix)	-	Exception when scraping megadrive game that does not have a country/region detected
* (Feature)	-	Asyncronous loading of sidebar images (to make navigating the games library a little less sluggish)
* (Addition) - 	Added .sgx as an acceptable rom extension (suprgrafx)
* (Feature)	-	Removed Configs 'SAVE CHANGES' button. Config settings are now saved automatically when you navigate to another tab or system filter

##### [0.5.7.1](https://medlaunch.info/releases/0-5-7-1)
###### 2017-07-19
* (BugFix)	-	Fixed on-game-launch exception when starting a netplay game
* (BugFix)	-	Fixed 'import config' prompt on launch always appearing when user has explictly said NO to this previously
* (BugFix)	-	Manual download method for mednafen no longer 'hangs' the UI
* (BugFix)	-	Mednafen folder selection method now no longer results in an inifinate loop of menus under certain conditions

##### [0.5.7.0](https://medlaunch.info/releases/0-5-7-0)
###### 2017-07-05
* (BugFix) 	-	Fixed issue with ROMs inside zip files not launching correctly (ROMs will need a rescan for fix to take effect)
* (BugFix)	-	Removed '-wswan.rotateinput' command that was causing more recent versions of Mednafen to not launch wswan games
* (BugFix)	-	Fixed issue with SBI file import for m3u games
* (BugFix)	-	Cancel button on scanning and scraping dialogs now works correctly
* (Enhancement)	-	All separate disparate DAT files have been combined into a smaller, fast SQLite DB

##### [0.5.6.2](http://medlaunch.asnitech.co.uk/releases/0-5-6-2)
###### 2017-06-28
* (BugFix) 'Updates' tab now changes properly depending on the updates that are available

##### [0.5.6.1](http://medlaunch.asnitech.co.uk/releases/0-5-6-1)
###### 2017-06-28
* (BugFix) Fixed re-occuring prompt (on whether or not to import settings from mednafen config files) on MedLaunch start. This will only happen once now
* (Cleanup) Removed old MasterGames.json scrape file to make the release smaller (this is no longer needed)
* (BugFix) Fixed issue where updates tab heading did not change when updates were available

##### [0.5.6.0](http://medlaunch.asnitech.co.uk/releases/0-5-6-0)
###### 2017-06-27
* (BugFix) Fixed edge-case bug where certain disc cue file would not be parsed properly (eg Valkyrie Profile)
* (Feature) For multi-disc games added a context menu launch option to choose the disc you wish to boot from initially
* (BugFix) Fixed issues with the older mednafen versions compatibility code
* (Feature) Added sub-categorisation to the games library view (based on ROM extension). Gameboy/Gameboy Color, WSWAN/WSWAN-Color etc..
* (Feature) Megadrive/Genesis games scraping updated to use detected region (based on DAT lookup). Manual scrape where region is not explitly US or EUR/JPN displays results from both sets. Auto scrape performs the same, except the 'prefer genesis' scraping option is used to determine which system to auto-scrape if region info is not found
* (BugFix) Improved auto-scraping accuracy (more accurate scrapes, less 'missed' games
* (BugFix) Fixed first-time initialisation code (bugs in choosing mednafen directory
* (Feature) Implemented code to check for, and manually download the latest compatible mednafen version (x64 only) and extract into configured mednafen folder (resides under the 'updates' tab)
* (Feature) First time init now gives the option to choose/create a new directory (rather than just an existing mednafen directory) so that the latest compatible mednafen version can be downloaded and extracted (mednafen x64 only)

##### [0.5.5.2](http://medlaunch.asnitech.co.uk/releases/0-5-5-2)
###### 2017-06-23
* Fixed MobyScraper bug that was forcing MedLaunch into an infinite scraping loop and causing the application to hang

##### [0.5.5.1](http://medlaunch.asnitech.co.uk/releases/0-5-5-1)
###### 2017-06-22
* **Minor bugfix release to rectify the following issues with 0.5.5.0:**
* WonderSwan games now launch
* 'Launch game and view/edit launch string' window text area is now properly visable on smaller resolutions

##### [0.5.5.0](http://medlaunch.asnitech.co.uk/releases/0-5-5-0)
###### 2017-06-22
* (BugFix) Added a horizontal scrollbar to the 'game stats' grid in the sidebar (kicks in automatically when the text overflows the right boundary)
* (BugFix) Fixed exception when PSX serial number cannot be obtained during disc import
* (BugFix) All Disc games are now marked as hidden during a disc-scan if they no longer exist on the file-system
* (BugFix) Fixed intermittent exception on game scraping when thegamesdb.net does not return a game even though there is a game ID present
* (Feature) Games library overhaul - implemented multi-tier sorting and library state saving (per system filter)
* (Feature) Migrated gamesdb and mobygames json lookup files into a more manageable SQLite database
* (Feature) Added compatibility with mednafen 0.9.44.1 (snes_faust aspect ratio commands, vb LED onstate option, Wonderswan config changes and new virtual gamepad device)
* (BugFix) Enabled lookup of Famicom Disk System games on thegamesdb.net (for scraping)
* (BugFix) Games library view now updates correctly when disc-based games are manually added
* (Feature) Added compatibility with mednafen 0.9.45.1 (ss lightgun selection and crosshair color for all virtual ports)
* (BugFix) Fixed random exception on disc-scanning
* (Feature) Added official MedLaunch netplay server into server list

##### [0.5.0.0](http://medlaunch.asnitech.co.uk/releases/0-5-0-0)
###### 2017-03-13
* ** Now compatible with versions 0.9.39 through 0.9.43 of Mednafen **
* (BugFix) Handle exception that is generated when MedLaunch is set to remember the mednafen windows position but medlaunch doesnt start correctly
* (BugFix) Enable SNES games to be launched (removed problem unneeded commands - snes.input.port3-port8)
* (Feature) Implemented mednafen historical compatibility. MedLaunch will now work with the latest version of mednafen back to v0.9.39
* (Feature) Game controls can now be configured from within MedLaunch under the 'Controls' tab. This attempts to mirror the way mednafen configures devices (xinput for compatible devices, directinput for keyboard and other gamepads/joysticks). This is currently experimental and any bugs should be reported
* (BugFix) Scraper now handles underscores correctly
* (Feature) Auto-scanning & import of disc games! (see release notes for caviats)
* (Feature) On launch, psx game serial numbers are extracted from disc image(s) and an SBI file offered (if one is needed and available)
* (Feature) MedLaunch can now import multiple ROMs per archive (zip/7z) file (see release notes for restrictions)
* (BugFix) Prevented exception caused by null values in the database netplay_servers table
* (BugFix) Fixed MedLaunch crash when trying to access mednafen's stdout.txt log file whilst mednafen is running
* (Feature) PSX and Saturn games now have library details automatically populated on scan (by serial number lookup)
* (Feature) Added country filter radiobuttons (ALL, USA, EUR, JPN) to the games library. These filters obviously only work if the 'Country' field has been populated (either by DAT lookup or scrape). There is also an option to hide these filters on the settings tab

##### [0.4.4.0](http://medlaunch.asnitech.co.uk/releases/0-4-4-0)
###### 2017-02-26
* ** Only compatible with version 0.9.43.x of Mednafen (although 0.9.41 and up will probably still launch games) **
* (Fixed) Fixed some spelling mistakes
* (Removal) Removed duplicate scanning commands from the top-left menu (still present in games library filter context menus)
* (Feature) Added option (under top-left menu 'Visual') to show/hide specific mednafen systems in the launcher
* (Feature) Added right-click context menu options to the 'favorites' filter to 'Scrape previously unscraped games' and 'Re-scrape all games'. This is a similar process to the other system filter scraping options
* (Feature) Enabled multi-select of games in the library along with additional context menu for multiple games (add/remove from favorites, auto-scrape and delete games)
* (Feature) Added option (to Settings>Emulator Engine) to automatically save and set the mednafen windows position on a per-system basis (disabled by default)
* Also added button to remove all saved mednafen window positions (in case of a screw up)
* (BugFix) Fixed missing snes.input.port3-8.gampad controls
* (Change) Log files now open in the system default text editor (falling back to notepad if there isnt one set)
* (BugFix) Fixed intermittent exceptions caused by save to clipboard functions
* (BugFix) Added 'Sound Device' textbox to config tab (accidentaly left out of the initial UI build)

##### [0.4.3.1](http://medlaunch.asnitech.co.uk/releases/0-4-3-1)
###### 2017-02-17
* ** Only compatible with version 0.9.42.x of Mednafen (although 0.9.41 will probably still launch games) **
* (Bugfix) Fixed exception(crash) caused by mednafen application not being properly initialised before importing configs into MedLaunch
* (Bugfix) Added null-check on netplay servers datagrid (mitigates application exception if the database didnt initialise correctly with the pre-defined servers)
* (Bugfix) Fixed localisation lookup issue when attempting to determine the mednafen version from the mednafen output log (stdout.txt)
* (Addition) Added another public netplay server to the list

##### [0.4.3.0](http://medlaunch.asnitech.co.uk/releases/0-4-3-0)
###### 2017-02-16
* ** Only compatible with version 0.9.42.x of Mednafen (although 0.9.41 will probably still launch games) **
* Implemented option to choose games library column visibility on a per-system basis (in top-left menu and settings page)
* Added new MedLaunch icon and 'Loading' splash-screen
* Fixed default system videoip and x/y scale values
* Changes made on 'Settings' tab are now automatically saved in realtime (and the 'Save Settings' button has been removed)
* Added a games library right-click option to view/edit the Mednafen launch string before the game runs
* Added (in top-left menu system) an option to Audit the Scraped Data Folder (so that you can determine which folder is for which game regardless of whether that data is currently liked to a game in the games library)
* Modified the 'Netplay Server Settings' module to be a datagrid, enabling as many custom servers as needed

##### [0.4.2.0](http://medlaunch.asnitech.co.uk/releases/0-4-2-0)
###### 2017-02-13
* ** Only compatible with version 0.9.42.x of Mednafen (although 0.9.41 will probably still launch games) **
* Fixed launcher crash when clicking the 'rescrape game' button if game has not been scraped before
* Changed 'Disks' to 'Discs' in games library menu
* Fixed arbitrary game launch when double-clicking on the games library scrollbars
* Implemented 7zip archive support for non-disc systems (roms are extracted to local cache folder before game launch) - one rom per archive file
* Added new setting to clear rom cache folder on application exit (enabled by default)
* Added an option to open the MedLaunch unhandled exception log (if one if present)
* Merged the 'top-right' title-bar buttons into a new (more standard) 'top-left' dropdown menu system
* Duplicated some of the ROM scanning functions in the top menu
* Added top-menu option to locate and launch a game through the filesystem (bypassing MedLaunch config database options meaning the game launches using the Mednafen config files on-disk)
* Moved hidden debug options to new hidden top-right debug menu (visible in Visual Studio debug-mode only)
* Fixed 'Unscraped Games' filter in games library
* Swapped text boxes that require decimal input for NumericUpDown controls (so that decimal and negative numbers can be entered by hand)

##### [0.4.1.0](http://medlaunch.asnitech.co.uk/releases/0-4-1-0)
###### 2017-02-09
* ** Only compatible with version 0.9.42.x of Mednafen (although 0.9.41 will probably still launch games) **
* Added new config controls for mednafen 0.9.42 (snes_faust multitap and additional virtual controller selection)
* Fixed a few small UI bugs
* Modified the update checker to use github's API rather than individually generated json files

##### [0.4.0.0](http://medlaunch.asnitech.co.uk/releases/0-4-0-0)
###### 2017-01-21
* ** Only compatible with version 0.9.41.x of Mednafen **
* Implemented config control changes for the latest Mednafen release
* Added a Mednafen version check on game launch (to avoid errors with wrong versions)
* Fixed some minor errors in the Mednafen config control names
* Added tooltips on config controls based on Mednafen documentation (can be turned off/on in settings)

##### [0.3.1.0](http://medlaunch.asnitech.co.uk/releases/0-3-1-0)
###### 2017-01-09
* Fixed 'Joystick Axis Threshold' config value not saving to database

##### [0.3.0.1](http://medlaunch.asnitech.co.uk/releases/0-3-0-1)
###### 2016-12-22
* Implemented color picker/canvas control for all Mednafen hex-color related options (lightgun crosshair colors etc..)
* Bugfix to stop application exception when trying to 'Browse Game Data Folder' when the game does not have a GDBID set in the database

##### [0.3.0.0](http://medlaunch.asnitech.co.uk/releases/0-3-0-0)
###### 2016-12-22
* Fixed netplay launch string issue affecting Windows 10 installs
* Fixed netplay nullable gamekey and password issue
* Various UI style enhancements (more consistent controls and mouse cursor change over controls)
* Implemented games library datagrid sorting memory (per session only)
* Vastly improved 'Save Settings' execution speed
* Added ability to set custom fullscreen resolutions
* Details of unhandled exceptions (application crashes) are now written to 'Exceptions.log' in the root of the MedLaunch directory
* A merged 'DAT' file (json) has been created and will be shipped with each release. Using TOSEC, NOINTRO, TRURIP & REDUMP roms will be detected on import (and re-scan) based on MD5 checksum and game name, country, year and publisher will be populated in the games library grid (where information is available). Year and publisher will be overwritten when game data is scraped from online
* Added context menu option to re-scrape all game information for a system (rather than just trying to scrape for games that have no previously been scraped). This will download all content again
* Added link to top bar that opens the Mednafen log file (stdout.txt) in the default text viewer

##### [0.2.20.0](http://medlaunch.asnitech.co.uk/releases/0-2-20-0)
###### 2016-11-22
* Fixed pce-cd games not being found when doing a batch scrape
* Added option (disabled by default) to always import Mednafen cfg files when MedLaunch starts
* Major performance gains on games library datagrid (dirty flag updates to in-memory data rather than individual database calls everywhere)
* Several secondary scraper edge-case bugfixes
* Added scraped data related columns to main games library datagrid

##### [0.2.15.0](http://medlaunch.asnitech.co.uk/releases/0-2-15-0)
###### 2016-11-17
* Fixed media content from downloading when option is de-selected in settings
* Fixed launch strings not being passed to mednafen for PCE CD games
* Fixed (unneeded) pcecd.cfg config being created in mednafen folder
* Fixed game scraping local match for PCD-CD games (games were not being found)
* Added option to set the size of the image tooltip popups (based on a percentage of the current MedLaunch window size)

##### [0.2.14.5](http://medlaunch.asnitech.co.uk/releases/0-2-14-5)
###### 2016-11-16
* Fixed control iteration bug where pce_fast (and possibly snes_faust) command line arguments were not being generated correctly
* Mednafen config backups are now stored in the MedLaunch folder (.\Data\MednafenCFGBackups\)
* Fixed some config controls not setting correctly
* Added messagebox on initial database creation - asking whether user wants to enable auto-check for updates on start

##### [0.2.14.2](http://medlaunch.asnitech.co.uk/releases/0-2-14-2)
###### 2016-11-16
* Fixed config import bug with non-valid values

##### [0.2.14.1](http://medlaunch.asnitech.co.uk/releases/0-2-14-1)
###### 2016-11-15
* Overhauled the database config tables (to more closely mirror Mednafen's config files)
* Implemented import-config-from-files functionality
* Fixed launch issue when using a fresh instance of MedLaunch 0.2.11.1
* Modified game launcher so that only system-specific command line arguments are used
* Added option (on by default) to backup Mednafen configuration on disk when MedLaunch first starts
* 'Base Configuration' config filter has been removed and the way MedLaunch handles Mednafen config arguments has been completely overhauled (if updating rather than new install of MedLaunch please ensure that you 'Import All Configs' once before launching a game
* Added option to automatically save system.cfg file in the Mednafen directory on game launch (enabled by default). It is recommended that you leave this on to avoid any inconsistencies when importing configs into MedLaunch. As above definitely do an 'Import All Configs' on first run and backup your pre-existing system.cfg files if you don't want them tampered with

##### [0.2.11.1](http://medlaunch.asnitech.co.uk/releases/0-2-11-1)
###### 2016-11-10
* Bufix to update checker

##### [0.2.11.0](http://medlaunch.asnitech.co.uk/releases/0-2-11-0)
###### 2016-11-10
* Fixed major issues with auto-game scraper (it actually works now and content is re-downloaded if it is already present)
* Added application colour theme picker
* Added option to check for MedLaunch updates on startup (but not auto-install) - disabled by default

##### [0.2.8.0](http://medlaunch.asnitech.co.uk/releases/0-2-8-0)
###### 2016-11-09
* Implemented manual 'check for updates' button - if a new update to MedLaunch is found you can choose to automatically download, extract and upgrade your current MedLaunch install
* Changed release zip file (from next release) so that there is no sub-folder inside (so the built-in updater works properly and folks can manually upgrade a little more easily)
* Game scraping has been completed overhauled - scraped data is now completely database independent (so images/manuals can be placed directory inside the game data folder and be picked up in the games library)
* Modified scraping module to include other sources
* Implemented game manual/instructions (PDF) scraping
* Added option to show ALL possible settings on the 'Base Configuration'. This is similar to how the actual Mednafen mednafen-09x.cfg file works and means that you don't have to use MedLaunch system-specific configurations to access system-specific settings (if you don't want to)
* Dev-only 'Release Generator' application for generation of release info json files
* Modifications to httprequest code to deal with random timeouts (although thegamesdb.net is still slow)
* Fixed db upgrade DateTime? exception on null value
* Fixed some of the database and application update checking logic

##### [0.2.3.1](http://medlaunch.asnitech.co.uk/releases/0-2-3-1)
###### 2016-10-24
* More UI threading fixes
* Implemented basic games library sidebar
* Added setting to always hide the sidebar
* Added .md as an allowed Megagrive ROM extension (so it will be picked up by the RomScanner)
* Implemented thegamesdb scraping code
* Added scraped data display in the sidebar
* Auto-save games library expander states on application close
* Implemented database update code (so application can be unzipped over a previous version's folder)
* Added 'Unscraped Games' filter to games library
* Simplified the GUI zoom code so (hopefully) there will be less edge-case crashes

##### [0.1.16.2](http://medlaunch.asnitech.co.uk/releases/0-1-16-2)
###### 2016-10-12
* Fixed vb config option that was causing games not to launch
* Fixed crash on double-click of empty games library
* Fixed all context menu exceptions (null errors) on Games Library
* Fixed a master system config option (so games now run)
* Added 'Minimise to taskbar on Game Launch' settings option (default is ON)
* Some UI speed optimisations

##### [0.1.15.7](http://medlaunch.asnitech.co.uk/releases/0-1-15-7)
###### 2016-10-11
* Added system specific debug config controls
* Added WonderSwan specific config controls
* Moved 'Enable Netplay' checkbox to Settings tab
* Changed netplay.password and netplay.gamekey so that they can be applied to static servers in the list
* Added startup check - if screen resolution is less than standard MedLaunch resolution - maximise the window to fit
* Moved snes_faust and pce_fast launch toggles to the settings page
* Added games library context menu option 'copy launch string to clipboard'
* Modified all panel heights so that they are viewable on lower resolutions
* Added a 'Set GUI Zoom' selector on the settings tab. Useful for people with tiny resolutions who cant see all content (and people with large resolutions who want the GUI content bigger)
* Added Mednafen directory selectors (cheats, snaps, sav etc)
* Better whitespace validation when building launch config (values with spaces are now encapsulated with quotation marks)
* Added System BIOS/Rom path controls

##### [0.1.14.6](http://medlaunch.asnitech.co.uk/releases/0-1-14-6)
###### 2016-10-10
* Added 32000Hz audio sample rate option
* Games library case-insensitive search
* Game path handling overhaul (fixed pce CD games not launching)
* Error handling and fixes to missing game or ROM folders (library games are marked as hidden until found again)
* Moved to semantic versioning
* Threaded game library import (so application doesnt appear to 'hang')
* Added context menu options in the games library for deleting all games in the database

##### [0.1.14](http://medlaunch.asnitech.co.uk/releases/0-1-14)
###### 2016-10-06
* Minor dialog box visual changes
* Hide unimplemented menu items
* Remove arbitary wait commands when launching games (so games launch as fast as GUI drawing allows)
* Implemented Saturn-specific configuration controls
* Implemented PSX-specific configuration controls
* Moved static game system information out of the database (where it had no business being)
* Unified 'Configs' tab so similar layout to 'Games' library tab
* Unified 'Settings' tab visual layout
* Fixed (force-ignore) JavaScript errors within the web browser control

##### [0.1.13](http://medlaunch.asnitech.co.uk/releases/0-1-13)
###### 2016-10-04
* Exception handling when corrupt Zip archive interogated
* Removed .smd as a supported Megadrive/Gensis ROM format (to re-implement once .smd to .bin conversion code is in place)
* Removed 'Rescan All Disks' option for the time being (as not yet implemented)
* Added option to manually add disk-based games (context menu right-click on System filter in 'Games' tab.
* Implemented m3u auto-generation logic when adding multi-disk games 
* Enabled double-click to launch game in Games library
* Added 'Delete From Games Library' context menu option (removes from the database, not disk - roms will be re-added on next scan)
* Modified games library refresh code so that data is updated after a change

##### [0.1.12](http://medlaunch.asnitech.co.uk/releases/0-1-12)
###### 2016-10-03
* Fixed broken URLs on 'help' tab
* Removed 'About' tab (as it is currently empty)

##### [0.1.11](http://medlaunch.asnitech.co.uk/releases/0-1-11)
###### 2016-10-03
* Updated first time initialisation routines
* Removed game info sidebar (to be re-added once scraping is implemented)
* Disabled disk-scanning function for the time being (until it is written properly)
* Implemented embedded browser control for 'Help' tab
* Modified WrapPanel and ScrollViewer code along with Expander controls so launcher can be used on smaller resolution systems


