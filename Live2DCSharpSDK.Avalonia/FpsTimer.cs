using Avalonia.OpenGL.Controls;
using Avalonia.Threading;
using System;
using System.Threading;

namespace Live2DCSharpSDK.Avalonia;

/// <summary>
/// A timer class to manage frames per second (FPS) for rendering.
/// </summary>
public class FpsTimer
{
    private readonly OpenGlControlBase _render;
    private readonly Timer _timer;

    /// <summary>
    /// Gets or sets the target frames per second (FPS).
    /// </summary>
    public int Fps { get; set; } = 60;

    /// <summary>
    /// Gets or sets the action to be invoked on each FPS tick.
    /// </summary>
    public Action<int>? FpsTick { private get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the timer is paused.
    /// </summary>
    public bool Pause { get; set; }

    /// <summary>
    /// Gets the current FPS.
    /// </summary>
    public int NowFps { get; private set; }

    private int _time;
    private bool _run;

    /// <summary>
    /// Initializes a new instance of the <see cref="FpsTimer"/> class.
    /// </summary>
    /// <param name="render">The OpenGL control base for rendering.</param>
    public FpsTimer(OpenGlControlBase render)
    {
        _render = render;
        _run = true;
        _time = (int)((double)1000 / Fps);
        _timer = new(Tick);
        _timer.Change(0, 1000);
        new Thread(() =>
        {
            while (_run)
            {
                if (Pause)
                {
                    Thread.Sleep(100);
                    continue;
                }
                Dispatcher.UIThread.Post(() =>
                {
                    _render.RequestNextFrameRendering();
                });
                NowFps++;
                Thread.Sleep(_time);
            }
        })
        {
            Name = "Live2DDotNet_Render_Timer"
        }.Start();
    }

    /// <summary>
    /// The method called on each timer tick.
    /// </summary>
    /// <param name="state">The state object.</param>
    private void Tick(object? state)
    {
        if (!Pause)
        {
            if (NowFps != Fps && _time > 1)
            {
                if (NowFps > Fps)
                {
                    _time++;
                }
                else
                {
                    _time--;
                }
            }
        }
        FpsTick?.Invoke(NowFps);
        NowFps = 0;
    }

    /// <summary>
    /// Closes the timer and stops the rendering loop.
    /// </summary>
    public void Close()
    {
        _run = false;
        _timer.Dispose();
    }
}
