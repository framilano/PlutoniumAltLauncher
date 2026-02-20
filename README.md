# PlutoniumAltLauncher 
#### *Plutonium Alternative Launcher*

A [Plutonium](https://plutonium.pw/) launcher that allows you to play offline, mainly developed with Steam Deck in mind and powered by [Avalonia](https://avaloniaui.net/).

## Building
```
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishTrimmed=true -p:TrimMode=partial
robocopy PlutoniumAltLauncher/Native/libvlc PlutoniumAltLauncher/bin/Release/net9.0/win-x64/publish/libvlc /MIR
```
## Todo
- [X] Add Controller support
- [X] Play background music
- [X] Set IW5SP image
- [X] Replace images with higher resolution ones? Meh
- [X] Stop music when launching Plutonium
- [X] Option to close Launcher automatically when launching Plutonium
- [ ] Change music (and loop it) when changing game in main menu
- [ ] Better UI in settings, controller navigation?
