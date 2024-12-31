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
using Avalonia.Threading;


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
    private Dispatcher dispatcher;
    public event void GameStarted;
    public GameView()
    {
        InitializeComponent();
    }

    public GameView(int difficulty) : this()
    {
        dispatcher = Dispatcher.UIThread;
        this.Difficulty = difficulty;
        Progress = LowerLim;
            for (int i = 0; i < 16; i++)
            {
                using Stream stream = File.Open($"C:\\Users\\Bumchic\\Documents\\GitHub\\ProjectGymSim\\GymSim\\GymSim\\res\\Frame{i}.jpg", FileMode.Open);
                Image img = new Image()
                {
                    Source = new Bitmap(stream),
                    IsVisible = false
                };
                GameFrame.Add(img);
                this.ThisPanel.Children.Add(img);
            }
            GameFrame[Progress].IsVisible = true;
    }

    public async Task StartGame()
    {
        await Task.Run(() =>
        {
            dispatcher.Invoke(() =>
            {
                GameFrame[Progress].IsVisible = true;
            });
            while (Progress > LowerLim)
            {
                Progress--;
                dispatcher.Invoke(() =>
                {
                    GameFrame[Progress + 1].IsVisible = false;
                    GameFrame[Progress].IsVisible = true;
                });
                Task.Delay(300).Wait();
            }
             Console.WriteLine("Game started");
        });
    }


    public void SpaceButton_OnKeyDown(object? sender, KeyEventArgs e)
    {
        Task GameState = StartGame();
        if (e.Key != Key.Space)
        {
            return;
        }
        Progress += 1;
        GameFrame[Progress - 1].IsVisible = false;
        GameFrame[Progress].IsVisible = true;
        if (GameState.Status != TaskStatus.Created)
        {
            GameState.Start();
        }
    }
}