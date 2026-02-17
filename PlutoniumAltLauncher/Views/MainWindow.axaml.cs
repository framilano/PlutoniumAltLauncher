using System;
using System.Diagnostics;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Serilog;

namespace PlutoniumAltLauncher.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        AppConfigManager.Load();
        
        //Init MainWindowGamepad
        InitGamepadHandling();
    }
    
    private void OpenSettings_OnClick(object? sender, RoutedEventArgs e)
    {
        var win = new MainSettings();
        win.ShowDialog(this);
    }

    private async void ShowMessage(string title, string message)
    {
        var box = MessageBoxManager.GetMessageBoxStandard(title,message, ButtonEnum.Ok);
        var result = await box.ShowAsync(); // Use await
    }

    private void LaunchGame_OnClick(object? sender, RoutedEventArgs e)
    {
        var btn = (Button)sender!;
        var tag = btn.Name;
        
        var user = Environment.UserName;
        var plutoniumAppDataPath = $@"C:\Users\{user}\AppData\Local\Plutonium";
        var bootstrapperExecutable = $@"{plutoniumAppDataPath}\bin\plutonium-bootstrapper-win32.exe";
        var arguments = "";
        var exe = "";
        
        //Creating AppData dir if not present
        if (!Directory.Exists(plutoniumAppDataPath))
        {
            Directory.CreateDirectory(plutoniumAppDataPath);
            Log.Information("Created directory {PlutoniumAppDataPath}", plutoniumAppDataPath);
        }
        
        switch (tag)
        {
            case "PlutoniumOnline":
            {
                exe = AppConfigManager.Current.PlutoniumPath;
                break;
            }
            case "PlutoniumT4SPZM" or "PlutoniumT4MP":
            {
                exe = bootstrapperExecutable;
                var gameName = tag.EndsWith("MP") ? "t4mp" : "t4sp";
                arguments = $"{gameName} {AppConfigManager.Current.T4FolderPath} +name {AppConfigManager.Current.IngameUsername} -lan";
                break;
            }
            case "PlutoniumT5SPZM" or "PlutoniumT5MP":
            {
                exe = bootstrapperExecutable;
                var gameName = tag.EndsWith("MP") ? "t5mp" : "t5sp";
                arguments = $"{gameName} {AppConfigManager.Current.T5FolderPath} +name {AppConfigManager.Current.IngameUsername} -lan";
                break;
            }
            case "PlutoniumT6ZM" or "PlutoniumT6MP":
            {
                exe = bootstrapperExecutable;
                var gameName = tag.EndsWith("MP") ? "t6mp" : "t6zm";
                arguments = $"{gameName} {AppConfigManager.Current.T6FolderPath} +name {AppConfigManager.Current.IngameUsername} -lan";
                break;
            }
            case "PlutoniumIW5SP" or "PlutoniumIW5MP":
            {
                exe = bootstrapperExecutable;
                var gameName = tag.EndsWith("MP") ? "iw5mp" : "iw5sp";
                arguments = $"{gameName} {AppConfigManager.Current.IW5FolderPath} +name {AppConfigManager.Current.IngameUsername} -lan";
                break;
            }
        }

        Log.Information("Executable {Executable}", exe);
        Log.Information("Arguments {Arguments}", arguments);
        Log.Information("WorkingDir {PlutoniumAppDataPath}", plutoniumAppDataPath);
        
        if (string.IsNullOrEmpty(exe))
        {
            ShowMessage("Invalid executable selected", "Check your executable paths in settings");
            return;
        }

        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = exe,
                Arguments = arguments,
                WorkingDirectory = plutoniumAppDataPath
            });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Process crashed :\\");
        }
    }

    private void CloseProgram_OnClick(object? sender, RoutedEventArgs e)
    {
        Log.Information("Application closing");
        Close();
    }
}