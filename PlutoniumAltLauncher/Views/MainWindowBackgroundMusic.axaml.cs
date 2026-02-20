using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Serilog;

namespace PlutoniumAltLauncher.Views;

public partial class MainWindow
{
    //Init method invoked in MainWindow.axaml.cs
    public void InitBackgroundMusicHandling()
    {
        //New instance of background music, all handling is declared here
        BackgroundMusic backgroundMusic = new BackgroundMusic();
        backgroundMusic.StartBackgroundMusicHandling();
        
        //When the window goes out of focus, tell the gamepad handler to stop polling for events
        //Activated and Deactivated are events fire by this Window
        Activated += (_, _) => backgroundMusic.IsActive = true;
        Deactivated += (_, _) => backgroundMusic.IsActive = false;
    }
}