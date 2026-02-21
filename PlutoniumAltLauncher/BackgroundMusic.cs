using System;
using System.IO;
using System.Threading.Tasks;
using LibVLCSharp.Shared;

namespace PlutoniumAltLauncher;

public class BackgroundMusic
{
    
    //Defines if music should be playing or not
    public bool IsActive { get; set; } = true;

    private string SongToPlay { get; set; } = "damned.mp3";

    private MediaPlayer? _player;
    
    private bool _shouldBeKilled;

    public void StopBackgroundMusicHandling()
    {
        _shouldBeKilled = true;
    }
    
    public void ChangeSong(string songName)
    {
        SongToPlay = songName;
        IsActive = false;
        _player?.Stop();
        IsActive = true;
    }
    
    public void StartBackgroundMusicHandling()
    {
        _shouldBeKilled = false;
        Task.Run(async () =>
        {
            Core.Initialize(); // loads native VLC
            
            while (true)
            {
                if (_shouldBeKilled) return;
                while (!IsActive) await Task.Delay(100); //Window isn't active, stop playing
                await PlayMusic();
            }
        });
    }

    private async Task PlayMusic()
    {
        using var libVlc = new LibVLC();
        var path = Path.Combine(AppContext.BaseDirectory, "Assets/music", SongToPlay);
        using var media = new Media(libVlc, path);
        _player = new MediaPlayer(media);

        _player.Play();
        _player.Volume = 35;
            
        //If this is too fast, player.IsPlaying would be false...
        await Task.Delay(500);

        //Console.WriteLine("Playing...");

        while (IsActive && _player.IsPlaying  && !_shouldBeKilled) await Task.Delay(100);
        _player.Stop();
    }
}