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
using System.Reflection;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Remote.Protocol;
using Avalonia.Threading;
using Microsoft.VisualBasic;


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
    private int weight;
    private bool GameOver;
    public int Weight
    {
        get
        {
            if (weight < 0)
            {
                return 0;
            }
            return weight;
        }
        set
        {
            if (weight < 0)
            {
                weight = 0;
            }
            weight = value;
        }
    }
    private int Time;
    private int RepCount = 0;
    private int strength;
    public int Strength
    {
        get
        {
            return strength;
        }
        set
        {
            if (strength > 100)
            {
                strength = 100;
            }
            else
            {
                strength = value;
            }
            
        }
    }

    private int PushStrength;
    private bool PushOver50;
    public GameView()
    {
        InitializeComponent();
        Icon = new WindowIcon("res/app.ico");
    }

    public GameView(int difficulty, double Width, double Height, WindowState windowState) : this()
    {
        dispatcher = Dispatcher.UIThread;
        this.Difficulty = difficulty;
        Progress = LowerLim;
        LiftStarted += OnLiftStarted;
        liftStarted = false;
        Strength = 100;
        GameOver = false;
        PushOver50 = false;
        this.Width = Width;
        this.Height = Height;
        this.WindowState = windowState;
        SetupImage();
        GameFrame[Progress].IsVisible = true;
        SetWeight(Difficulty);
        RegainStrength();
    }
    public void SetupImage()
    {
        string? assembly = Assembly.GetExecutingAssembly().GetName().Name;
        for (int i = 0; i < 16; i++)
        {
            using Stream stream = AssetLoader.Open(new Uri($"avares://{assembly}/res/Frame{i}.jpg"));
            Image img = new Image()
            {
                Source = new Bitmap(stream),
                IsVisible = false
            };
            GameFrame.Add(img);
            this.ThisPanel.Children.Add(img);
        }
    }
    public void SetWeight(int Difficulty)
    {
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
    public async Task RegainStrength()
    {
        await Task.Run(() =>
        {
            while (true)
            {
                Task.Delay(100).Wait();
                Strength += 2;
                dispatcher.Invoke(() =>
                {
                    this.StrengthCounter.Text = $"Strength:{Strength}";
                });
            }
        });
    }
    public async Task StartGame()
    {
        liftStarted = true;
        await Task.Run(() =>
        {
            while (Progress > LowerLim)
            {
                if (!liftReset)
                {
                    PushStrength = Math.Clamp(Time - Math.Abs(Strength), 0, Weight);
                }
                Progress--;
                dispatcher.Invoke(() =>
                {
                    GameFrame[Progress + 1].IsVisible = false;
                    GameFrame[Progress].IsVisible = true;
                });
                Console.WriteLine(PushStrength);
                Task.Delay(PushStrength).Wait();
            }

            if (!liftReset && PushOver50)
            {
                GameOver = true;
                dispatcher.Invoke(() =>
                {
                    this.GameOverScreen.IsVisible = true;
                });
                SaveGame();
            }
            liftStarted = false;
            liftReset = false;
            Time = Weight;
            PushOver50 = false;
        });
    }

    public void SaveGame()
    {
        
    }
    public async void OnLiftStarted()
    {
        await StartGame();
    }
    public void SpaceButton_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Space || liftReset || GameOver)
        {
            return;
        }
        Progress += 1;
        Strength -= 10;
        if (strength < -weight)
        {
            strength = -weight;
        }
        GameFrame[Progress - 1].IsVisible = false;
        GameFrame[Progress].IsVisible = true;
        if (Progress >= UpperLim / 2)
        {
            PushOver50 = true;
        }
        if (Progress == UpperLim - 1)
        {
            RepCount++;
            Time = 0;
            PushStrength = 20;
            this.RepCounter.Text = $"Rep:{RepCount}";
            liftReset = true;
        }
        if (liftStarted == false)
        {
            LiftStarted?.Invoke();
        }
    }

    public void RetryButton_OnClick(object? sender, RoutedEventArgs e)
    {
        GameView gameView = new GameView(this.Difficulty, this.Width, this.Height, this.WindowState);
        gameView.Show();
        this.Close();
    }
}