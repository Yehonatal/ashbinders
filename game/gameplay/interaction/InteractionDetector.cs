using System.Collections.Generic;
using System.Linq;
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

    public override void _ExitTree()
    {
        AreaEntered -= OnAreaEntered;
        AreaExited -= OnAreaExited;
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is IInteractable interactable)
        {
            if (!_nearbyInteractables.Contains(interactable))
            {
                _nearbyInteractables.Add(interactable);
                interactable.Highlight(true);
            }
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

    public bool TryInteract(Node2D interactor)
    {
        var target = BestInteractable;
        if (target != null && target.CanInteract(interactor))
        {
            target.Interact(interactor);
            return true;
        }

        return false;
    }
}
