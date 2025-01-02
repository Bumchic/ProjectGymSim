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
using Avalonia.Remote.Protocol;
using Avalonia.Threading;
using ProjectGymSim.SelectDifficulty;

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
            else if (progress > UpperLim-1)
            {
                return UpperLim-1;
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
    private event Action LiftStarted;
    private bool liftStarted;
    private bool liftReset = false;
    private const int repResetTime = 100;
    private int Weight;
    private int Time;
    private int RepCount = 0;
    
    public GameView()
    {
        InitializeComponent();
    }

    public GameView(int difficulty) : this()
    {
        dispatcher = Dispatcher.UIThread;
        this.Difficulty = difficulty;
        Progress = LowerLim;
        LiftStarted += OnLiftStarted;
        liftStarted = false;
            for (int i = 0; i < 16; i++)
            {
                using Stream stream = File.Open($"C:\\Users\\hoang\\Desktop\\ProjectGymSim\\GymSim\\GymSim\\res\\Frame{i}.jpg", FileMode.Open);
                Image img = new Image()
                {
                    Source = new Bitmap(stream),
                    IsVisible = false
                };
                GameFrame.Add(img);
                this.ThisPanel.Children.Add(img);
            }
            GameFrame[Progress].IsVisible = true;
            switch (Difficulty)
            {
                case 1: Weight = 3000;
                    break;
                case 2: Weight = 2500;
                    break;
                case 3: Weight = 2000;
                    break;
                default: Weight = 3000;
                    break;
            }
    }

    public async Task StartGame()
    {
        liftStarted = true;
        await Task.Run(() =>
        {
            while (Progress > LowerLim)
            {
                Progress--;
                dispatcher.Invoke(() =>
                {
                    GameFrame[Progress + 1].IsVisible = false;
                    GameFrame[Progress].IsVisible = true;
                });
                Task.Delay(Time).Wait();
            }
            liftStarted = false;
            liftReset = false;
            Time = Weight;
        });
    }

    public void OnLiftStarted()
    {
        StartGame();
    }
    public void SpaceButton_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Space || liftReset)
        {
            return;
        }
        Progress += 1;
        GameFrame[Progress - 1].IsVisible = false;
        GameFrame[Progress].IsVisible = true;
        if (Progress == UpperLim - 1)
        {
            RepCount++;
            Time = repResetTime;
            this.RepCounter.Text = $"Rep:{RepCount}";
            liftReset = true;
            Weight -= 150;
        }
        if (liftStarted == false)
        {
            LiftStarted?.Invoke();
        }
    }
}