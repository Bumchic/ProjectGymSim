using Avalonia.Controls;
using ProjectGymSim.GamePartView;

namespace ProjectGymSimWindow.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }
    public MainView(int Difficulty) : this()
    {
        GameView gameView = new GameView(Difficulty);
        gameView.Show();
    }
}
