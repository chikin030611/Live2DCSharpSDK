using Avalonia.Controls;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Live2DCSharpSDK.App;
using System;
using System.IO;
using System.Collections.Generic;
using Live2DCSharpSDK.Framework.Motion;

namespace Live2DCSharpSDK.Avalonia.Avatar;

/// <summary>
/// A class responsible for rendering Live2D models using OpenGL in Avalonia.
/// </summary>
public class Live2dRender : OpenGlControlBase
{
    private LAppDelegate _lapp;
    private readonly LAppModel _model;

    private string _info = string.Empty;
    private DateTime time;

    /// <summary>
    /// Initializes a new instance of the <see cref="Live2dRender"/> class.
    /// </summary>
    public Live2dRender()
    {

    }

    /// <summary>
    /// Gets a value indicating whether a model is loaded.
    /// </summary>
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

    /// <summary>
    /// Checks for OpenGL errors and logs them to the console.
    /// </summary>
    /// <param name="gl">The OpenGL interface.</param>
    private static void CheckError(GlInterface gl)
    {
        int err;
        while ((err = gl.GetError()) != GlConsts.GL_NO_ERROR)
            Console.WriteLine(err);
    }

    /// <summary>
    /// Initializes OpenGL resources and loads the Live2D model.
    /// </summary>
    /// <param name="gl">The OpenGL interface.</param>
    protected override unsafe void OnOpenGlInit(GlInterface gl)
    {
        CheckError(gl);

        try
        {
            _lapp = new(new AvaloniaApi(this, gl), Console.WriteLine)
            {
                BGColor = new(0, 0, 0, 0)
            };
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

        // Load the model
        const string modelName = "Haru";
        string relativePath = $@"..\..\..\..\resources\models\{modelName}";
        string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
        var model = _lapp.Live2dManager.LoadModel(fullPath, modelName);

        CheckError(gl);
    }

    /// <summary>
    /// Deinitializes OpenGL resources.
    /// </summary>
    /// <param name="GL">The OpenGL interface.</param>
    protected override void OnOpenGlDeinit(GlInterface GL)
    {
        _lapp?.Dispose();
        _lapp = null!;
    }

    /// <summary>
    /// Renders the Live2D model using OpenGL.
    /// </summary>
    /// <param name="gl">The OpenGL interface.</param>
    /// <param name="fb">The framebuffer.</param>
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
        _lapp.Run(span);
        CheckError(gl);
    }

    /// <summary>
    /// Gets the list of available motions for the model.
    /// </summary>
    /// <returns>A list of motion names.</returns>
    public List<string> GetMotions()
    {
        return _model.Motions;
    }

    /// <summary>
    /// Gets the list of available expressions for the model.
    /// </summary>
    /// <returns>A list of expression names.</returns>
    public List<string> GetExpressions()
    {
        return _model.Expressions;
    }

    /// <summary>
    /// Plays the specified motion on the model.
    /// </summary>
    /// <param name="name">The name of the motion to play.</param>
    public void PlayMotion(string name)
    {
        _model.StartMotion(name, MotionPriority.PriorityForce);
    }

    /// <summary>
    /// Plays the specified expression on the model.
    /// </summary>
    /// <param name="name">The name of the expression to play.</param>
    public void PlayExpression(string name)
    {
        _model.SetExpression(name);
    }

    /// <summary>
    /// Starts playing the audio associated with the specified question ID.
    /// </summary>
    /// <param name="id">The question ID.</param>
    public void StartSpeaking(int id)
    {
        string filePath = QnaAudioManager.GetAudioPath(id);
        QnaAudioManager.PlayAudio(filePath);
        _lapp.StartSpeaking(filePath);
    }

}
