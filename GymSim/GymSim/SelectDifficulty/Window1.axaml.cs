using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ProjectGymSim.GamePartView;

namespace ProjectGymSim.SelectDifficulty;

public partial class Window1 : Window
{
    public Window1()
    {
        InitializeComponent();
        Icon = new WindowIcon("res/app.ico");
        
        EasyButton.Click += EasyButton_Click;
        NormalButton.Click += NormalButton_Click;
        HardButton.Click += HardButton_Click;
    }

    private void EasyButton_Click(object? sender, RoutedEventArgs e)
    {
        OpenGameView(1);
    }

    private void NormalButton_Click(object? sender, RoutedEventArgs e)
    {
        OpenGameView(2);
    }

    private void HardButton_Click(object? sender, RoutedEventArgs e)
    {
        OpenGameView(3);
    }
    private void OpenGameView(int difficulty)
    {
        var gameView = new GameView(difficulty, this.Width, this.Height, this.WindowState);
        gameView.Show();
    }
}
