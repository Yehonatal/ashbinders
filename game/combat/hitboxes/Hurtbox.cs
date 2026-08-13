using Godot;
using Ashbinders.Gameplay.Damage;

namespace Ashbinders.Combat.Hitboxes;

[GlobalClass]
public partial class Hurtbox : Area2D
{
    [Export] public Node? DamageableTarget { get; set; }

    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
    }

    public override void _ExitTree()
    {
        AreaEntered -= OnAreaEntered;
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is Hitbox hitbox)
        {
            var target = DamageableTarget as IDamageable ?? Owner as IDamageable;
            if (target != null)
            {
                var direction = (GlobalPosition - hitbox.GlobalPosition).Normalized();
                target.TakeDamage(hitbox.CreateDamageInfo(direction));
            }
        }
    }
}
