using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace ProjectGymSim.Menu;

public partial class MenuControl : Window
{
    public MenuControl()
    {
        InitializeComponent();
        Icon = new WindowIcon("res/app.ico");
    }
    private async void OnKeyDown(object? sender, KeyEventArgs e)
    {
        var selectWindow = new SelectDifficulty.Window1();
        await selectWindow.ShowDialog(this);
        this.Close();
    }
    
    private void OnExitClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
    private void OnFullScreenClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.WindowState = WindowState.FullScreen;
    }

    private void OnResize800x600Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.WindowState = WindowState.Normal; 
        this.Width = 800;
        this.Height = 600;
    }

    private void OnResize1024x768Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.WindowState = WindowState.Normal; 
        this.Width = 1024;
        this.Height = 768;
    }
}