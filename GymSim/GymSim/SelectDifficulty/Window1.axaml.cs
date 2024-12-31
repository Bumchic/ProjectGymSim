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

        // Gán sự kiện Click cho các nút
        EasyButton.Click += EasyButton_Click;
        NormalButton.Click += NormalButton_Click;
        HardButton.Click += HardButton_Click;
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
        GameView.DelayTime = delay; 
        var gameView = new GameView();
        gameView.Show();
    }
}
