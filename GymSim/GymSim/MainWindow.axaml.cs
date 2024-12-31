using Avalonia.Controls;
using ProjectGymSim.GamePartView;

namespace GymSim;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        GameView gameView = new GameView(1);
        gameView.Show();
    }
}