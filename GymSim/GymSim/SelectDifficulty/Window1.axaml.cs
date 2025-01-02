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
        var gameView = new GameView(difficulty);
        gameView.Show();
    }
}
