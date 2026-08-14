using Godot;
using Ashbinders.Gameplay.Damage;

namespace Ashbinders.Combat.Hitboxes;

[GlobalClass]
public partial class Hitbox : Area2D
{
    [Export] public int BaseDamage { get; set; } = 10;
    [Export] public DamageType DamageType { get; set; } = DamageType.Physical;
    [Export] public float KnockbackForce { get; set; } = 150.0f;
    [Export] public bool BreaksArmor { get; set; } = false;
    [Export] public float StaggerForce { get; set; } = 100.0f;

    public Node2D? AttackerNode { get; set; }

    public override void _Ready()
    {
        Monitoring = false;
    }

    public DamageInfo CreateDamageInfo(Vector2 direction)
    {
        return new DamageInfo(BaseDamage, DamageType, direction * KnockbackForce, AttackerNode, BreaksArmor, StaggerForce);
    }
}
