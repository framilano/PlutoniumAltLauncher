using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PlutoniumAltLauncher.Views;

public partial class ErrorWindow : Window
{
    public ErrorWindow(string title, string message)
    {
        InitializeComponent();
        Title = title;
        ErrorMessage.Text = message;
        
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }

    private void Quit(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}