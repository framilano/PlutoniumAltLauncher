# PlutoniumAltLauncher 
#### *Plutonium Alternative Launcher*

A [Plutonium](https://plutonium.pw/) launcher that allows you to play offline, mainly developed with Steam Deck in mind and powered by [Avalonia](https://avaloniaui.net/).
It has controller, touchscreen, mouse/kb support and works on both Linux (through Wine/Proton) and Windows.

The main objective is giving a shortcut to play offline Call of Duty Plutonium-supported titles.

Steam Deck LCDs seem to get banned immediately while playing online, due to the HWID being shared and in some way triggering the anticheat. OLEDs models are not affected by this issue

**Oh btw, I'm not affiliated with the Plutonium in any capability, show some gratitude to the devs!**

![immagine](https://github.com/framilano/PlutoniumDeckLauncher/assets/28491164/345a3045-9b24-45e0-8c6f-35f3f5710c11)


## Requirements
- Call of Duty game files
- [Plutonium](https://cdn.plutonium.pw/updater/plutonium.exe)

## How to install
1. Download and extract the `PlutoniumAltLauncher.zip` from [Releases](https://github.com/framilano/PlutoniumDeckLauncher/releases)
2. Here's my folder structure in `/home/deck/NoLauncherGames/Plutonium`:

```
├── PlutoniumWAW    # Contains World at War game files
├── PlutoniumBO2    # Contains Black Ops 1 game files
├── PlutoniumBO1    # Contains Black Ops 2 game files
├── PlutoniumIW5    # Contains MW3 game files
├── plutonium.exe
└── PlutoniumAltLauncher
    ├── Assets
    ├── config.json
    └── PlutoniumAltLauncher.exe
```
You can change this folder structure editing the `config.json` inside the `configs` folder, maybe you need to point to a different folder or drive to retrieve your game files.

3. Add `PlutoniumAltLauncher.exe` as non-steam game in `Desktop Mode`.
4. Set `Proton Experimental` or `Proton-GE` in compatibility settings, `Proton GE` is recommended.
5. Add the following line (if you're on kernel < 6.14 or without the NTSync module loaded) on command launch arguments (fsync and esync are known to cause issues with BO1/2):

   `PROTON_NO_ESYNC=1 PROTON_NO_FSYNC=1 %command%`

6. With NTSYNC on kernel >= 6.14 and `Proton-GE` integrating it by default there's no need for these launch arguments anymore. You still can't use NTSYNC on Steam Deck until Valve updates the kernel.
7. Start the program, you need to first boot the classic plutonium launcher (so it can download the prefix all the necessary files). Close everything and reboot the launcher, you're done!

## Extra
You need to install through `Protontricks` the `xact` audio library to have all sound fx on BO1 and WAW

## Configuration

There are some editable fields in config.json:
- `PlutoniumExecutablePath` set your plutonium.exe path
- `IngameUsername` set your ingame username while playing offline
- `T4/5/6/IW5 FolderPath` set your game files path
- `CloseAtLaunch` exit the launcher when booting the game, or leave it in background so you can open it back later
- `DisableBackgroundMusic` disable the background music immediately

## Building
```
dotnet publish -c Release -r win-x64
robocopy PlutoniumAltLauncher/Native/libvlc PlutoniumAltLauncher/bin/Release/net10.0/win-x64/publish/libvlc /MIR
```

## Todo
- [X] Add Controller support
- [X] Play background music
- [X] Set IW5SP image
- [X] Replace images with higher resolution ones? Meh
- [X] Stop music when launching Plutonium
- [X] Option to close Launcher automatically when launching Plutonium
- [X] Change music (and loop it) when changing game in main menu
- [X] Check if player started Plutonium at least once
- [ ] Better UI in settings, controller navigation?
- [ ] Reduce RAM usage
