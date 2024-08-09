using Avalonia.Controls;
using Avalonia;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Avalonia.Rendering;
using Live2DCSharpSDK.App;
using System;
namespace Live2DCSharpSDK.Avalonia.Avatar;

internal class Live2dRender : OpenGlControlBase
{
    private LAppDelegate _lapp;
    private LAppModel _model;

    private LAppDelegate lapp;

    private string _info = string.Empty;
    private DateTime time;

    public Live2dRender() { }

    private static void CheckError(GlInterface gl)
    {
        int err;
        while ((err = gl.GetError()) != GlConsts.GL_NO_ERROR)
            Console.WriteLine(err);
    }

    protected override unsafe void OnOpenGlInit(GlInterface gl)
    {
        CheckError(gl);

        try
        {
            lapp = new(new AvaloniaApi(this, gl), Console.WriteLine)
            {
                BGColor = new(0, 0, 0, 0)
            };
        }
        catch (Exception e)
        {

        }
        var model = lapp.Live2dManager.LoadModel("C:\\Personal\\Kenneth\\Live2D-dotnet\\res\\live2d-model\\", "Haru");
        CheckError(gl);
    }

    protected override void OnOpenGlDeinit(GlInterface GL)
    {

    }

    protected override void OnOpenGlRender(GlInterface gl, int fb)
    {
        int x = (int)Bounds.Width;
        int y = (int)Bounds.Height;

        if (VisualRoot is TopLevel window)
        {
            var screen = window.RenderScaling;
            x = (int)(Bounds.Width * screen);
            y = (int)(Bounds.Height * screen);
        }
        gl.Viewport(0, 0, x, y);
        var now = DateTime.Now;
        float span = 0;
        if (time.Ticks == 0)
        {
            time = now;
        }
        else
        {
            span = (float)(now - time).TotalSeconds;
            time = now;
        }
        lapp.Run(span);
        CheckError(gl);
    }
}
