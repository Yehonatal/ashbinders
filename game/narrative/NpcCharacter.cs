using Godot;
using Ashbinders.Core.Events;
using Ashbinders.Gameplay.Interaction;

namespace Ashbinders.Narrative;

[GlobalClass]
public partial class NpcCharacter : Area2D, IInteractable
{
    [Export] public string NpcName { get; set; } = "Archivist Vael";
    [Export] public string DialogueText { get; set; } = "Greetings Ashbinder Kael. The Underlevels are shifting. Power the Ancient Ember Device to unlock the Vault Chamber ahead!";

    public string InteractionPrompt => $"Talk to {NpcName}";
    public bool CanInteract(Node2D interactor) => true;

    private float _glowTimer;
    private bool _isHighlighted;

    public override void _Process(double delta)
    {
        _glowTimer += (float)delta * 2.0f;
        QueueRedraw();
    }

    public void Interact(Node2D interactor)
    {
        var choices = new[]
        {
            new DialogueChoiceOption(
                "What is this Ancient Device?",
                "Vael: It is a remnant of Veyr's hydraulic routing network. Inserting a Motion Ember supplies kinetic power to retract the Ember Gate."
            ),
            new DialogueChoiceOption(
                "Where can I find an Ember?",
                "Vael: The Scavenger Prowlers in the Western Chamber carry Motion Embers within their core. Defeat them to extract their flame!"
            ),
            new DialogueChoiceOption(
                "How do I break the Armored Enemy?",
                "Vael: The Hollow Brute's steel plating deflects light slashes. Switch to your Hammer Head [Key 2] to shatter its armor!"
            ),
            new DialogueChoiceOption(
                "I will proceed.",
                "Vael: May the hearth flame guide your chain, Ashbinder."
            )
        };

        EventBus.Publish(new DialogueTriggerEvent(NpcName, DialogueText, choices));
    }

    public void Highlight(bool isHighlighted)
    {
        _isHighlighted = isHighlighted;
    }

    public override void _Draw()
    {
        var pulse = (Mathf.Sin(_glowTimer) + 1.0f) * 0.5f;
        DrawCircle(Vector2.Zero, 16.0f, new Color(0.2f, 0.75f, 0.85f));
        DrawCircle(Vector2.Zero, 8.0f, Colors.White);
        DrawArc(Vector2.Zero, 22.0f + pulse * 4.0f, 0, Mathf.Tau, 20, new Color(0.2f, 0.75f, 0.85f, 0.6f), 2.0f);

        if (_isHighlighted)
        {
            DrawArc(Vector2.Zero, 28.0f, 0, Mathf.Tau, 20, Colors.Yellow, 2.0f);
        }
    }
}
