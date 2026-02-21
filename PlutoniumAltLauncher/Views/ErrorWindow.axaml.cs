using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace PlutoniumAltLauncher.Views;

public partial class ErrorWindow : Window
{
    public ErrorWindow(string title, string message)
    {
        InitializeComponent();
        Title = title;
        ErrorMessage.Text = message;
        
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        
        //Init ErrorWindowGamepad
        InitGamepadHandling();
        
        Opened += (_, _) => Dispatcher.UIThread.Post(InvalidateVisual, DispatcherPriority.Render);
    }

    private void Quit(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}