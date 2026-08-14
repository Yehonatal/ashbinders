using Godot;

namespace Ashbinders.UI.Modals;

[GlobalClass]
public partial class TestModeInfoWindow : Control
{
    private Button? _startButton;

    public override void _Ready()
    {
        _startButton = GetNodeOrNull<Button>("Panel/StartButton");
        if (_startButton != null)
        {
            _startButton.Pressed += OnStartPressed;
        }

        Visible = true;
        GetTree().Paused = true;
    }

    private void OnStartPressed()
    {
        Visible = false;
        if (GetParent() is CanvasLayer canvas)
        {
            canvas.Visible = false;
        }
        GetTree().Paused = false;
    }
}
