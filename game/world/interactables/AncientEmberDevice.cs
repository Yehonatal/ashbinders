using Godot;
using Ashbinders.Characters.Player;
using Ashbinders.Core.Events;
using Ashbinders.Embers.Core;
using Ashbinders.Embers.Types;
using Ashbinders.Gameplay.Interaction;

namespace Ashbinders.World.Interactables;

[GlobalClass]
public partial class AncientEmberDevice : Area2D, IInteractable
{
    [Signal]
    public delegate void DeviceActivatedEventHandler();

    [Signal]
    public delegate void DeviceDeactivatedEventHandler();

    [Export] public string DeviceId { get; set; } = "ancient_ember_device_01";
    [Export] public EmberType RequiredEmberType { get; set; } = EmberType.Motion;
    [Export] public EmberSocket? Socket { get; set; }
    [Export] public EmberGate? TargetGate { get; set; }
    [Export] public bool IsActivated { get; private set; }

    public string InteractionPrompt => IsActivated
        ? "Extract Ember from Ancient Device"
        : $"Insert {RequiredEmberType} Ember into Device";

    private float _glowTimer;
    private bool _isHighlighted;

    public override void _Ready()
    {
        Socket ??= GetNodeOrNull<EmberSocket>("EmberSocket");
        if (Socket != null)
        {
            Socket.AllowedType = RequiredEmberType;
            Socket.AcceptAnyType = false;
        }
    }

    public override void _Process(double delta)
    {
        _glowTimer += (float)delta * 3.0f;
        QueueRedraw();
    }

    public bool CanInteract(Node2D interactor)
    {
        if (interactor is PlayerController player)
        {
            if (IsActivated) return true;
            return player.ChainSocket?.CurrentEmber?.Type == RequiredEmberType;
        }
        return true;
    }

    public void Interact(Node2D interactor)
    {
        if (interactor is not PlayerController player) return;

        if (IsActivated)
        {
            var ember = Socket?.TryExtractEmber();
            if (ember != null)
            {
                player.ChainSocket?.TryInsertEmber(ember);
                IsActivated = false;
                EmitSignal(SignalName.DeviceDeactivated);
                EventBus.Publish(new ToastNotificationEvent("Ember Extracted from Ancient Device"));
            }
        }
        else
        {
            var ember = player.ChainSocket?.TryExtractEmber();
            if (ember != null && Socket != null && Socket.TryInsertEmber(ember))
            {
                IsActivated = true;
                EmitSignal(SignalName.DeviceActivated);
                EventBus.Publish(new DeviceActivatedEvent(DeviceId));
                EventBus.Publish(new ToastNotificationEvent("ANCIENT DEVICE ACTIVATED — Power Flowing!"));
                TargetGate?.Unlock();
            }
        }
    }

    public void Highlight(bool isHighlighted)
    {
        _isHighlighted = isHighlighted;
    }

    public override void _Draw()
    {
        var activeColor = IsActivated ? new Color(1.0f, 0.75f, 0.2f) : new Color(0.3f, 0.35f, 0.45f);
        DrawCircle(Vector2.Zero, 24.0f, activeColor);
        DrawCircle(Vector2.Zero, 12.0f, IsActivated ? Colors.White : new Color(0.2f, 0.2f, 0.2f));

        if (IsActivated)
        {
            var pulse = (Mathf.Sin(_glowTimer) + 1.0f) * 0.5f;
            DrawArc(Vector2.Zero, 30.0f + pulse * 6.0f, 0, Mathf.Tau, 24, new Color(1.0f, 0.75f, 0.2f, 0.5f), 2.5f);
        }

        if (_isHighlighted)
        {
            DrawArc(Vector2.Zero, 32.0f, 0, Mathf.Tau, 24, Colors.Yellow, 2.0f);
        }
    }
}
