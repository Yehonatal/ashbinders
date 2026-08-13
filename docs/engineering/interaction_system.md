# Engineering Guide: Generic Interaction System

## 1. Overview
Rather than hardcoding interaction prompts for every NPC, chest, door, and machine inside `PlayerController.cs`, we use an open interface architecture (`IInteractable`).

---

## 2. Interaction Contract

```csharp
namespace Ashbinders.Gameplay.Interaction;

public interface IInteractable
{
    string InteractionPrompt { get; }
    bool CanInteract(Godot.Node2D interactor);
    void Interact(Godot.Node2D interactor);
    void Highlight(bool isHighlighted);
}
```

---

## 3. Interaction Detector Component

```csharp
using Godot;

namespace Ashbinders.Gameplay.Interaction;

[GlobalClass]
public partial class InteractionDetector : Area2D
{
    private readonly List<IInteractable> _nearbyInteractables = new();
    public IInteractable? BestInteractable => _nearbyInteractables.FirstOrDefault();

    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
        AreaExited += OnAreaExited;
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is IInteractable interactable)
        {
            _nearbyInteractables.Add(interactable);
            interactable.Highlight(true);
        }
    }

    private void OnAreaExited(Area2D area)
    {
        if (area is IInteractable interactable)
        {
            interactable.Highlight(false);
            _nearbyInteractables.Remove(interactable);
        }
    }

    public void TriggerInteraction(Node2D interactor)
    {
        var target = BestInteractable;
        if (target != null && target.CanInteract(interactor))
        {
            target.Interact(interactor);
        }
    }
}
```
