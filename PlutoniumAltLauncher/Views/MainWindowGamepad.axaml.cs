using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using Serilog;

namespace PlutoniumAltLauncher.Views;

public partial class MainWindow
{
    
    //Init method invoked in MainWindow.axaml.cs
    public void InitGamepadHandling()
    {
        //New instance of gamepad, all handling is declared here
        Gamepad gamepad = new Gamepad();
        gamepad.StartGamepadHandling();
        gamepad.GamepadButtonPressed += OnGamepadButtonPressed;
        
        //When the window goes out of focus, tell the gamepad handler to stop polling for events
        //Activated and Deactivated are events fire by this Window
        Activated += (_, _) => gamepad.IsActive = true;
        Deactivated += (_, _) => gamepad.IsActive = false;

        AddHighlight(PlutoniumOnline);
    }

    private void AddHighlight(Button element)
    {
        element.BorderBrush = new SolidColorBrush(Colors.Blue);
        element.BorderThickness = new Thickness(2);
    }

    private void RemoveHighlight(Button element)
    {
        element.BorderBrush = null;
        element.BorderThickness = new Thickness(0);
    }
    
    private int[] CurrentElementSelected { get; set; } = [0, 0];
    private Dictionary<string, string> ElementsDict = new Dictionary<string, string>
    {
        ["0;0"] = "PlutoniumOnline",
        ["1;0"] = "PlutoniumOnline",
        ["0;1"] = "PlutoniumT4SPZM",
        ["1;1"] = "PlutoniumT4MP",
        ["0;2"] = "PlutoniumT5SPZM",
        ["1;2"] = "PlutoniumT5MP",
        ["0;3"] = "PlutoniumT6ZM",
        ["1;3"] = "PlutoniumT6MP",
        ["0;4"] = "PlutoniumIW5SP",
        ["1;4"] = "PlutoniumIW5MP",
    };
    
    private void OnGamepadButtonPressed(object? sender, string message)
    {
        int number = int.Parse(message);
        int[] newElementSelected = number switch
        {
            0 => [CurrentElementSelected[0], CurrentElementSelected[1] - 1],
            1 => [CurrentElementSelected[0], CurrentElementSelected[1] + 1],
            2 => [CurrentElementSelected[0] - 1, CurrentElementSelected[1]],
            3 => [CurrentElementSelected[0] + 1, CurrentElementSelected[1]],
            _ => [-1, -1]
        };

        if (newElementSelected[0] == -1 && newElementSelected[1] == -1)
        {
            Dispatcher.UIThread.Post(() =>
            {
                //Emulating click event
                Button currentHighlightedButton = this.FindControl<Button>(ElementsDict[$"{CurrentElementSelected[0]};{CurrentElementSelected[1]}"])!;
                currentHighlightedButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            });
            Log.Information("Clicked on button!");
            return;
        }

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
            Button currentHighlightedButton = this.FindControl<Button>(ElementsDict[$"{CurrentElementSelected[0]};{CurrentElementSelected[1]}"])!;
            RemoveHighlight(currentHighlightedButton);
            
            CurrentElementSelected = newElementSelected;
            
            Button buttonToHighlight = this.FindControl<Button>(ElementsDict[$"{CurrentElementSelected[0]};{CurrentElementSelected[1]}"])!;
            AddHighlight(buttonToHighlight);
            ButtonViewer.Text = message;
        });
    }
}