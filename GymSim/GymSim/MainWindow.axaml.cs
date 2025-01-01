using Avalonia.Controls;
using ProjectGymSim.GamePartView;

namespace GymSim;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        GameView gameView = new GameView(3);
        gameView.Show();
    }
}