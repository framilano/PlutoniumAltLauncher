using System;
using System.IO;
using System.Threading.Tasks;
using LibVLCSharp.Shared;

namespace PlutoniumAltLauncher;

public class BackgroundMusic
{
    
    public void StartBackgroundMusicHandling()
    {
        Task.Run(async () =>
        {
            Core.Initialize(); // loads native VLC

            using var libVLC = new LibVLC();
            
            var path = Path.Combine(AppContext.BaseDirectory, "Assets/music", "adrenaline.mp3");
            using var media = new Media(libVLC, path, FromType.FromPath);
            using var player = new MediaPlayer(media);

            player.Play();
            player.Volume = 35;
            
            //If this is too fast, player.IsPlaying would be false...
            await Task.Delay(500);

            Console.WriteLine("Playing...");

            // Keep alive until playback ends
            while (player.IsPlaying)
            {
                await Task.Delay(100);
            }

        });
    }
}