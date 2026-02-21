using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Serilog;

namespace PlutoniumAltLauncher.Views;

public partial class MainWindow
{
    //Init method invoked in MainWindow.axaml.cs
    private void InitGamepadHandling()
    {
        //New instance of gamepad, all handling is declared here
        _gamepad = new Gamepad();
        _gamepad.StartGamepadHandling();
        _gamepad.GamepadButtonPressed += OnGamepadButtonPressedOnMainWindow;
        
        //When the window goes out of focus, tell the gamepad handler to stop polling for events
        //Activated and Deactivated are events fire by this Window
        Activated += (_, _) => IsWindowGamepadEnabled = true;
        Deactivated += (_, _) => IsWindowGamepadEnabled = false;

        AddHighlight(OnlineBtn);
    }

    private Gamepad _gamepad;

    private bool IsWindowGamepadEnabled { get; set; } = false;
    private int[] CurrentElementSelected { get; set; } = [0, 0];
    private readonly Dictionary<string, string> _elementsDict = new Dictionary<string, string>
    {
        ["0;0"] = "OnlineBtn",
        ["1;0"] = "OnlineBtn",
        ["0;1"] = "T4SpBtn",
        ["1;1"] = "T4MpBtn",
        ["0;2"] = "T5SpBtn",
        ["1;2"] = "T5MpBtn",
        ["0;3"] = "T6ZmBtn",
        ["1;3"] = "T6MpBtn",
        ["0;4"] = "Iw5SpBtn",
        ["1;4"] = "Iw5MpBtn",
    };

    private void OnPointerEntered(object? sender, RoutedEventArgs _)
    {
        var btn = (Button)sender!;
        var name = btn.Name!;
        
        var enteredButton = this.FindControl<Button>(name)!;
        AddHighlight(enteredButton);

        foreach (var element in _elementsDict)
        {
            if (element.Value != name) continue;
            CurrentElementSelected[0] = int.Parse(element.Key.Split(";")[0]);
            CurrentElementSelected[1] = int.Parse(element.Key.Split(";")[1]);
        }
    }
    
    private void OnPointerExited(object? sender, RoutedEventArgs _)
    {
        var btn = (Button)sender!;
        var name = btn.Name!;
        
        var enteredButton = this.FindControl<Button>(name)!;
        RemoveHighlight(enteredButton);
    }


    private void AddHighlight(Button element)
    {
        
        //Telling the backgroundmusic the change song
        ChangeMusic(element.Name!.ToLower().Replace("btn", ""));
        
        foreach (var keyPair in _elementsDict)
        {   
            //Remove highlight from every element when adding a new highlight (could be useful is a cursor is in window and we're
            //moving with a gamepad
            RemoveHighlight(this.FindControl<Button>(_elementsDict[keyPair.Key])!);
        }
        
        element.Opacity = 1;
    }

    private static void RemoveHighlight(Button element)
    {
        element.Opacity = 0.3;
    }

    private void OnGamepadButtonPressedOnMainWindow(object? sender, string message)
    {
        if (!IsWindowGamepadEnabled)
        {
            Console.WriteLine("Ignoring gamepad input on MainWindow");
            return;
        }

        var number = int.Parse(message);
        int[] newElementSelected = number switch
        {
            0 => [CurrentElementSelected[0], CurrentElementSelected[1] - 1],
            1 => [CurrentElementSelected[0], CurrentElementSelected[1] + 1],
            2 => [CurrentElementSelected[0] - 1, CurrentElementSelected[1]],
            3 => [CurrentElementSelected[0] + 1, CurrentElementSelected[1]],
            4 => [-1, -1], //South
            5 => [-1, -2], //East
            _ => [-2, -2]
        };
        
        //Pressed Unhandled button
        if (newElementSelected[0] == -2 && newElementSelected[1] == -2) return;
        
        //Pressed South
        if (newElementSelected[0] == -1 && newElementSelected[1] == -1)
        {
            Dispatcher.UIThread.Post(() =>
            {
                //Emulating click event
                var currentHighlightedButton =
                    this.FindControl<Button>(
                        _elementsDict[$"{CurrentElementSelected[0]};{CurrentElementSelected[1]}"])!;
                currentHighlightedButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            });
            Log.Information("Clicked on button!");
            return;
        }
        //Pressed East
        if (newElementSelected[0] == -1 && newElementSelected[1] == -2) Dispatcher.UIThread.Post(() => Close());


        if (newElementSelected[0] < 0 ||
            newElementSelected[0] > 1 ||
            newElementSelected[1] < 0 ||
            newElementSelected[1] > 4)
        {
            Log.Information("Value outside of bounds, ignoring");
            return;
        }

        Dispatcher.UIThread.Post(() =>
        {
            var currentHighlightedButton =
                this.FindControl<Button>(_elementsDict[$"{CurrentElementSelected[0]};{CurrentElementSelected[1]}"])!;
            RemoveHighlight(currentHighlightedButton);

            CurrentElementSelected = newElementSelected;

            var buttonToHighlight =
                this.FindControl<Button>(_elementsDict[$"{CurrentElementSelected[0]};{CurrentElementSelected[1]}"])!;
            AddHighlight(buttonToHighlight);
            //ButtonViewer.Text = message;
        });
    }
    
    //Stop gamepad thread when closing
    protected override void OnClosed(EventArgs e)
    {
        _gamepad.StopGamepadHandling();
        base.OnClosed(e);
    }
}