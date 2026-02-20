using System;
using System.IO;
using System.Threading.Tasks;
using LibVLCSharp.Shared;

namespace PlutoniumAltLauncher;

public class BackgroundMusic
{
    
    //Defines if music should be playing or not
    public bool IsActive { get; set; } = true;
    
    public void StartBackgroundMusicHandling()
    {
        Task.Run(async () =>
        {
            Core.Initialize(); // loads native VLC
            
            while (true)
            {
                while (!IsActive) await Task.Delay(100);
                await PlayMusic();
            }

        });
    }

    private async Task PlayMusic()
    {
        using var libVlc = new LibVLC();
        var path = Path.Combine(AppContext.BaseDirectory, "Assets/music", "adrenaline.mp3");
        using var media = new Media(libVlc, path);
        using var player = new MediaPlayer(media);

        player.Play();
        player.Volume = 35;
            
        //If this is too fast, player.IsPlaying would be false...
        await Task.Delay(500);

        Console.WriteLine("Playing...");

        while (IsActive && player.IsPlaying) await Task.Delay(100);
    }
}