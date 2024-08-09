using Avalonia.Controls;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Live2DCSharpSDK.App;
using System;
using System.Collections.Generic;
using Live2DCSharpSDK.Framework.Motion;

namespace Live2DCSharpSDK.Avalonia.Avatar;

public class Live2dRender : OpenGlControlBase
{
    private LAppDelegate _lapp;
    private LAppModel _model;

    private LAppDelegate lapp;

    private string _info = string.Empty;
    private DateTime time;

    public Live2dRender()
    {

    }

    public bool HaveModel
    {
        get
        {
            if (_lapp == null)
            {
                return false;
            }
            return _lapp.Live2dManager.GetModelNum() != 0;
        }
    }

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
            throw new Exception(e.Message);
        }
        var model = lapp.Live2dManager.LoadModel("C:\\Personal\\Kenneth\\Live2D-dotnet\\res\\live2d-model\\", "Haru");
        CheckError(gl);
    }

    protected override void OnOpenGlDeinit(GlInterface GL)
    {
        _lapp?.Dispose();
        _lapp = null!;
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

    public List<string> GetMotions()
    {
        return _model.Motions;
    }

    public List<string> GetExpressions()
    {
        return _model.Expressions;
    }

    public void PlayMotion(string name)
    {
        _model.StartMotion(name, MotionPriority.PriorityForce);
    }

    public void PlayExpression(string name)
    {
        _model.SetExpression(name);
    }

    public void StartSpeaking(int id)
    {
        string filePath = QnaAudioManager.GetAudioPath(id);
        QnaAudioManager.PlayAudio(filePath);
        _lapp.StartSpeaking(filePath);
    }

}
