﻿using SampleApp.Model;
using SampleApp.Services;
using SampleApp.Views;

namespace SampleApp;

public partial class App : Application
{
    readonly IThemeService themeService;

    public static new App Current => (App)Application.Current;
    public App(IThemeService themeService)
    {
        this.themeService = themeService;
        InitializeComponent();

        MainPage = new AppShell();
    }


    protected override void OnStart()
    {
        themeService.SetTheme(AppTheme.Unspecified);
    }

    protected override void OnResume()
    {

    }

}

