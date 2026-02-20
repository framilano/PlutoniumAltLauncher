namespace PlutoniumAltLauncher.Views;

public partial class MainWindow
{
    private BackgroundMusic? _backgroundMusic;
    
    //Init method invoked in MainWindow.axaml.cs
    private void InitBackgroundMusicHandling()
    {
        //New instance of background music, all handling is declared here
        _backgroundMusic = new BackgroundMusic();
        _backgroundMusic.StartBackgroundMusicHandling();
        
        //When the window goes out of focus, tell the gamepad handler to stop polling for events
        //Activated and Deactivated are events fire by this Window
        Activated += (_, _) => _backgroundMusic.IsActive = true;
        Deactivated += (_, _) => _backgroundMusic.IsActive = false;
    }

    private void ChangeMusic(string elementName)
    {
        _backgroundMusic?.ChangeSong($"{elementName}.mp3");
    }
}