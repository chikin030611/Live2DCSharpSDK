using Avalonia.Controls;
using Live2DCSharpSDK.Avalonia.Avatar;

namespace Live2DCSharpSDK.Avalonia.Avatar.UI;

public partial class Live2dControl : UserControl
{
    private readonly Live2dRender _render;
    private readonly FpsTimer _renderTimer;

    public Live2dControl()
    {
        InitializeComponent();

        _render = new();

        Live2D.Child = _render;

        _renderTimer = new(_render);
    }
}