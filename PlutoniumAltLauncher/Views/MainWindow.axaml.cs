using System;
using System.Diagnostics;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Serilog;

namespace PlutoniumAltLauncher.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        AppConfigManager.Load();

        //Init MainWindowBackgroundMusic
        InitBackgroundMusicHandling();
        
        //Init MainWindowGamepad
        InitGamepadHandling();
        
    }
    
    private void OpenSettings_OnClick(object? sender, RoutedEventArgs e)
    {
        var win = new MainSettings();
        win.ShowDialog(this);
    }

    private void ShowMessage(string title, string message)
    {
        var win = new ErrorWindow(title, message);
        win.ShowDialog(this);
    }

    private void LaunchGame_OnClick(object? sender, RoutedEventArgs e)
    {
        var btn = (Button)sender!;
        var gameName = btn.Name!.ToLower().Replace("btn", "");
        
        var user = Environment.UserName;
        var plutoniumAppDataPath = $@"C:\Users\{user}\AppData\Local\Plutonium";
        var bootstrapperExecutable = $@"{plutoniumAppDataPath}\bin\plutonium-bootstrapper-win32.exe";
        var arguments = "";
        var exe = "";
        var gamePath = "";
        
        //Creating AppData dir if not present
        if (!Directory.Exists(plutoniumAppDataPath))
        {
            Directory.CreateDirectory(plutoniumAppDataPath);
            Log.Information("Created directory {PlutoniumAppDataPath}", plutoniumAppDataPath);
        }

        exe = bootstrapperExecutable;
        if (gameName == "online") exe = AppConfigManager.Current.PlutoniumExecutablePath;
        else if (gameName.Contains("t4")) gamePath = AppConfigManager.Current.T4FolderPath;
        else if (gameName.Contains("t5")) gamePath = AppConfigManager.Current.T5FolderPath;
        else if (gameName.Contains("t6")) gamePath = AppConfigManager.Current.T6FolderPath;
        else arguments = gamePath = AppConfigManager.Current.IW5FolderPath;

        if (!string.IsNullOrEmpty(gamePath)) arguments = $"{gameName} {gamePath} +name {AppConfigManager.Current.IngameUsername} -lan"; 
        
        Log.Information("Executable {Executable}", exe);
        Log.Information("Arguments {Arguments}", arguments);
        Log.Information("WorkingDir {PlutoniumAppDataPath}", plutoniumAppDataPath);

        if (!ValidateButtonInput(gameName, gamePath, exe)) return;

        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = exe,
                Arguments = arguments,
                WorkingDirectory = plutoniumAppDataPath
            });
        }
        catch (Exception ex) { Log.Error(ex, "Process crashed :\\"); }
        
        Log.Information("Game launched successfully");
        if (AppConfigManager.Current.CloseAtLaunch) Close();
        
    }

    private bool ValidateButtonInput(string gameName, string gamePath, string exe)
    {
        if (gameName != "online" && string.IsNullOrEmpty(gamePath))
        {
            ShowMessage("⚠️ Invalid gamepath selected", "Check your game paths in settings");
            return false;
        }
        
        if (string.IsNullOrEmpty(exe))
        {
            ShowMessage("⚠️ Invalid executable selected", "Check your plutonium.exe path in settings");
            return false;
        }

        if (!File.Exists(exe))
        {
            ShowMessage("⚠️ Plutonium installation not found", "I can't find your Plutonium installation\nDid you boot the official launcher at least once?");
            return false;
        }
        
        Log.Debug("Validation completed");
        return true;
    }
}