using System;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;
using Avalonia.Input;
using Avalonia.Interactivity;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Avalonia.Media.Imaging;
using System.Drawing.Imaging;


namespace ProjectGymSim.GamePartView;

public partial class GameView : Window
{
    private int progress;
    private const int UpperLim = 16;
    private const int LowerLim = 0;
    public int Progress
    {
        get
        {
            if (progress < LowerLim)
            {
                return LowerLim;
            }
            else if (progress > UpperLim)
            {
                return UpperLim;
            }else
                return progress;
        }
        set
        {
            progress = value;
        }
    }
    
    public int Difficulty{get;set;}
    List<Image> GameFrame = new List<Image>();
    private Task BarTimer;
    private string Path;
    
    public GameView()
    {
        InitializeComponent();
    }

    public GameView(int difficulty) : this()
    {

        this.Difficulty = difficulty;
        Progress = UpperLim;
        Path = "res";
            for (int i = 0; i < 16; i++)
            {
                using Stream stream = File.Open($"{Path}/Frame{i}.jpg", FileMode.Open);
                Image img = new Image()
                {
                    Source = new Bitmap(stream),
                    IsVisible = false
                };
                GameFrame.Add(img);
                this.ThisPanel.Children.Add(img);
            }

            BarTimer = new Task(() =>
            {
                while (this.Progress > LowerLim)
                {
                    GameFrame[Progress].IsVisible = true;
                    Progress--;
                    GameFrame[Progress + 1].IsVisible = false;
                    Console.WriteLine(Progress);
                    Task.Delay(100).Wait();
                }
            });
        BarTimer.Start();
    }
    
    public void SpaceButton_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Space)
        {
            return;
        }
        Progress += 1;
        GameFrame[Progress-=1].IsVisible = false;
        GameFrame[Progress].IsVisible = true;
    }
}