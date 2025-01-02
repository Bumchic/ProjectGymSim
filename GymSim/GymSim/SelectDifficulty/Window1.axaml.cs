using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using GymSim.GamePartView;
namespace GymSim.SelectDifficulty;

public partial class Window1 : Window
{
    public Window1()
    {
        InitializeComponent();
        EasyButton.Click += EasyButton_Click;
        NormalButton.Click += NormalButton_Click;
        HardButton.Click += HardButton_Click;
        this.Icon = new WindowIcon("res/app.ico");
    }

    private void EasyButton_Click(object? sender, RoutedEventArgs e)
    {
        OpenGameView(1000);
    }

    private void NormalButton_Click(object? sender, RoutedEventArgs e)
    {
        OpenGameView(500);
    }

    private void HardButton_Click(object? sender, RoutedEventArgs e)
    {
        OpenGameView(200);
    }

    private void OpenGameView(int delay)
    {
        
        var gameView = new GameView();
        gameView.Show();
    }
}
