using Godot;
using Ashbinders.Core.Events;
using Ashbinders.Core.Save;
using Ashbinders.Gameplay.Interaction;
using Ashbinders.Characters.Player;

namespace Ashbinders.World.Interactables;

[GlobalClass]
public partial class AncientSaveAnchor : Area2D, IInteractable
{
    private readonly SaveManager _saveManager = new();
    private float _pulseTimer;
    private bool _isHighlighted;

    public string InteractionPrompt => "Save Game at Ancient Anchor";
    public bool CanInteract(Node2D interactor) => true;

    public override void _Process(double delta)
    {
        _pulseTimer += (float)delta * 2.5f;
        QueueRedraw();
    }

    public void Interact(Node2D interactor)
    {
        var savePath = ProjectSettings.GlobalizePath("user://savegame.json");
        
        if (interactor is PlayerController player)
        {
            _saveManager.CurrentSaveData.Player.PositionX = player.GlobalPosition.X;
            _saveManager.CurrentSaveData.Player.PositionY = player.GlobalPosition.Y;
            if (player.Health != null)
            {
                _saveManager.CurrentSaveData.Player.CurrentHealth = player.Health.CurrentHealth;
                _saveManager.CurrentSaveData.Player.MaxHealth = player.Health.MaxHealth;
            }
        }

        var success = _saveManager.SaveToFile(savePath);
        if (success)
        {
            EventBus.Publish(new GameSavedEvent("Ancient Save Anchor (Vault Chamber)"));
            EventBus.Publish(new ToastNotificationEvent("GAME SAVED AT ANCIENT ANCHOR!"));
        }
    }

    public void Highlight(bool isHighlighted)
    {
        _isHighlighted = isHighlighted;
    }

    public override void _Draw()
    {
        var pulse = (Mathf.Sin(_pulseTimer) + 1.0f) * 0.5f;
        var goldColor = new Color(1.0f, 0.85f, 0.2f);

        DrawRect(new Rect2(-24, -24, 48, 48), new Color(0.15f, 0.15f, 0.18f));
        DrawRect(new Rect2(-20, -20, 40, 40), new Color(0.25f, 0.22f, 0.15f));
        
        DrawCircle(Vector2.Zero, 12.0f + pulse * 4.0f, new Color(1.0f, 0.85f, 0.2f, 0.4f));
        DrawCircle(Vector2.Zero, 8.0f, goldColor);
        DrawCircle(Vector2.Zero, 4.0f, Colors.White);

        if (_isHighlighted)
        {
            DrawArc(Vector2.Zero, 30.0f, 0, Mathf.Tau, 20, Colors.Yellow, 2.0f);
        }
    }
}
