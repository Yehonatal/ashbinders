using Godot;

namespace Ashbinders.Gameplay.Interaction;

public interface IInteractable
{
    string InteractionPrompt { get; }
    bool CanInteract(Node2D interactor);
    void Interact(Node2D interactor);
    void Highlight(bool isHighlighted);
}
