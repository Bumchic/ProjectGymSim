using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using ProjectGymSim.Menu;
using ProjectGymSim.SelectDifficulty;

namespace GymSim;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        RequestedThemeVariant = ThemeVariant.Dark;
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MenuControl();
        }

        base.OnFrameworkInitializationCompleted();
    }
}