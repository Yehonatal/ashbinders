using Godot;
using Ashbinders.Characters.Player;
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
    [Export] public bool IsActivated { get; private set; }

    public string InteractionPrompt => IsActivated
        ? "Extract Ember (Press E)"
        : $"Insert {RequiredEmberType} Ember (Press E)";

    public override void _Ready()
    {
        Socket ??= GetNodeOrNull<EmberSocket>("EmberSocket");
        if (Socket != null)
        {
            Socket.AllowedType = RequiredEmberType;
            Socket.AcceptAnyType = false;
        }
    }

    public bool CanInteract(Node2D interactor)
    {
        if (interactor is PlayerController player)
        {
            if (IsActivated) return true; // Can extract
            return player.ChainSocket?.CurrentEmber?.Type == RequiredEmberType;
        }
        return false;
    }

    public void Interact(Node2D interactor)
    {
        if (interactor is not PlayerController player) return;

        if (IsActivated)
        {
            // Extract from machine back into player
            var ember = Socket?.TryExtractEmber();
            if (ember != null)
            {
                player.ChainSocket?.TryInsertEmber(ember);
                IsActivated = false;
                EmitSignal(SignalName.DeviceDeactivated);
            }
        }
        else
        {
            // Insert from player into machine
            var ember = player.ChainSocket?.TryExtractEmber();
            if (ember != null && Socket != null && Socket.TryInsertEmber(ember))
            {
                IsActivated = true;
                EmitSignal(SignalName.DeviceActivated);
            }
        }
    }

    public void Highlight(bool isHighlighted)
    {
        // Visual indicator / highlight shader toggle
    }
}
