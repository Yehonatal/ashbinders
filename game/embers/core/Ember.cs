using Godot;
using Ashbinders.Combat.Hitboxes;

namespace Ashbinders.Embers.Core;

public abstract partial class Ember : Resource
{
    [Export] public string EmberId { get; set; } = string.Empty;
    [Export] public EmberType Type { get; set; }
    [Export] public string DisplayName { get; set; } = string.Empty;
    [Export] public string Description { get; set; } = string.Empty;
    [Export] public Color GlowColor { get; set; } = Colors.Orange;

    public virtual void ApplyToWeapon(Hitbox hitbox) { }
    public virtual void RemoveFromWeapon(Hitbox hitbox) { }
}
