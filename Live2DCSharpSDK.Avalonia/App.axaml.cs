using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using System;

namespace Live2DCSharpSDK.Avalonia;

public partial class App : Application
{
    public static event Action? OnClose;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
    public static void Close()
    {
        OnClose?.Invoke();
        Environment.Exit(Environment.ExitCode);
    }
}