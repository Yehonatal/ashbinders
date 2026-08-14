using Godot;
using Ashbinders.Core.Events;
using Ashbinders.Gameplay.Interaction;
using Ashbinders.Embers.Types;

namespace Ashbinders.Embers.Core;

[GlobalClass]
public partial class EmberPickup : Area2D, IInteractable
{
    [Export] public Ember ExtractedEmber { get; set; } = new MotionEmber();

    public string InteractionPrompt => $"Extract {ExtractedEmber.DisplayName}";
    public bool CanInteract(Node2D interactor) => true;

    private float _bobTimer;
    private bool _isHighlighted;
    private Color _glowColor = new(1.0f, 0.7f, 0.2f);

    public override void _Ready()
    {
        Monitoring = true;
        _glowColor = ExtractedEmber.GlowColor;
    }

    public override void _Process(double delta)
    {
        _bobTimer += (float)delta * 3.0f;
        QueueRedraw();
    }

    public void Interact(Node2D interactor)
    {
        EventBus.Publish(new EmberExtractedEvent(ExtractedEmber.Type.ToString()));
        EventBus.Publish(new ToastNotificationEvent($"Extracted {ExtractedEmber.DisplayName}!"));
        QueueFree();
    }

    public void Highlight(bool isHighlighted)
    {
        _isHighlighted = isHighlighted;
    }

    public override void _Draw()
    {
        var offset = Mathf.Sin(_bobTimer) * 4.0f;
        DrawCircle(new Vector2(0, offset), 12.0f, _glowColor);
        DrawCircle(new Vector2(0, offset), 6.0f, Colors.White);
        DrawArc(new Vector2(0, offset), 18.0f, 0, Mathf.Tau, 16, new Color(_glowColor.R, _glowColor.G, _glowColor.B, 0.5f), 2.0f);

        if (_isHighlighted)
        {
            DrawArc(new Vector2(0, offset), 24.0f, 0, Mathf.Tau, 20, Colors.Yellow, 2.0f);
        }
    }
}
