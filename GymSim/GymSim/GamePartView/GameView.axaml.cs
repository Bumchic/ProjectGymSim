﻿using System;
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


namespace GymSim.GamePartView;

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
        Strength = 100;
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
            RegainStrength();
    }

    public async Task RegainStrength()
    {
        await Task.Run(() =>
        {
            while (true)
            {
                Task.Delay(100).Wait();
                Strength += 1;
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
                Progress--;
                dispatcher.Invoke(() =>
                {
                    GameFrame[Progress + 1].IsVisible = false;
                    GameFrame[Progress].IsVisible = true;
                });
                Task.Delay(Math.Clamp(Time - Math.Abs(Strength), 0, Weight)).Wait();
            }
            liftStarted = false;
            liftReset = false;
            Time = Weight;
        });
    }

    public async void OnLiftStarted()
    {
        await StartGame();
    }
    public void SpaceButton_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (liftReset)
        {
            return;
        }
        Progress += 1;
        Strength -= 3;
        GameFrame[Progress - 1].IsVisible = false;
        GameFrame[Progress].IsVisible = true;
        if (Progress == UpperLim - 1)
        {
            RepCount++;
            Time = 0;
            this.RepCounter.Text = $"Rep:{RepCount}";
            liftReset = true;
        }
        if (liftStarted == false)
        {
            LiftStarted?.Invoke();
        }
    }
}