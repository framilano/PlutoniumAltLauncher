using System;
using Avalonia.Threading;

namespace PlutoniumAltLauncher.Views;

public partial class ErrorWindow
{
    //Init method invoked in MainWindow.axaml.cs
    private void InitGamepadHandling()
    {
        //New instance of gamepad, all handling is declared here
        _gamepad = new Gamepad();
        _gamepad.StartGamepadHandling();
        _gamepad.GamepadButtonPressed += OnGamepadButtonPressedOnMainSettings;
        
        //When the window goes out of focus, tell the gamepad handler to stop polling for events
        //Activated and Deactivated are events fire by this Window
        Activated += (_, _) => IsWindowGamepadEnabled = true;
        Deactivated += (_, _) => IsWindowGamepadEnabled = false;
    }

    private Gamepad _gamepad;

    private bool IsWindowGamepadEnabled { get; set; } = false;
    
    private void OnGamepadButtonPressedOnMainSettings(object? sender, string message)
    {
        if (!IsWindowGamepadEnabled)
        {
            Console.WriteLine("Ignoring gamepad input on MainSettings");
            return;
        }
        var number = int.Parse(message);
        int[] newElementSelected = number switch
        {
            4 => [0, 0], //South
            5 => [0, 1], //East
            _ => [-1, -1]
        };
        
        //Pressed Unhandled button
        if (newElementSelected[0] == -1 && newElementSelected[1] == -1) return;
        
        //Pressed South or East
        if (newElementSelected[0] == -0 && (newElementSelected[1] == 0 || newElementSelected[1] == 1)) Dispatcher.UIThread.Post(() => Close());
    }
    
    //Stop gamepad thread when closing
    protected override void OnClosed(EventArgs e)
    {
        _gamepad.StopGamepadHandling();
        base.OnClosed(e);
    }
}