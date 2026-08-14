using Godot;
using Ashbinders.Core.Events;

namespace Ashbinders.World.Interactables;

[GlobalClass]
public partial class EmberGate : StaticBody2D
{
    [Export] public string GateId { get; set; } = "underlevels_vault_gate";
    [Export] public bool IsUnlocked { get; set; } = false;

    private CollisionShape2D? _collision;
    private ColorRect? _visualRect;
    private float _openProgress = 0.0f;

    public override void _Ready()
    {
        _collision = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
        _visualRect = GetNodeOrNull<ColorRect>("VisualRect");
        UpdateGateState();
    }

    public override void _Process(double delta)
    {
        if (IsUnlocked && _openProgress < 1.0f)
        {
            _openProgress = Mathf.Min(1.0f, _openProgress + (float)delta * 2.0f);
            if (_visualRect != null)
            {
                _visualRect.Color = new Color(0.2f, 0.8f, 0.4f, 1.0f - _openProgress * 0.7f);
                _visualRect.Scale = new Vector2(1.0f, 1.0f - _openProgress * 0.9f);
            }
        }
    }

    public void Unlock()
    {
        if (IsUnlocked) return;
        IsUnlocked = true;
        if (_collision != null)
        {
            _collision.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        }
        EventBus.Publish(new GateOpenedEvent(GateId));
        EventBus.Publish(new ToastNotificationEvent("GATE UNLOCKED — Vault Chamber Open!"));
    }

    private void UpdateGateState()
    {
        if (_collision != null)
        {
            _collision.Disabled = IsUnlocked;
        }
    }
}
