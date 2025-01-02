using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
namespace GymSim.Menu;

public partial class MenuControl : Window
{
    public MenuControl()
    {
        InitializeComponent();
        this.Icon = new WindowIcon("res/app.ico");
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

    public void OnIconClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (MainMenu.IsVisible)
        {
            MainMenu.IsVisible = false;
            /*((Button)sender!).IsVisible = true;*/
        }
        else
        { 
            MainMenu.IsVisible = true;
            ((Button)sender!).IsVisible = false;
        }
    }
    private void OnMenuClick(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        // Hide the menu and show the icon
        MainMenu.IsVisible = false;

        // Find the button (icon) and make it visible again
        var iconButton = this.FindControl<Button>("IconButton");
        if (iconButton != null)
        {
            iconButton.IsVisible = true;
        }
    }
    private void OnFilePointerEnter(object? sender, PointerEventArgs e)
    {
        if (sender is MenuItem menuItem)
        {
            menuItem.IsSubMenuOpen = true; // Buộc hiển thị menu con
        }
    }
    
}