using Godot;
using Ashbinders.Gameplay.Damage;

namespace Ashbinders.Combat.Weapons;

public abstract partial class WeaponHead : Resource
{
    [Export] public string HeadId { get; set; } = string.Empty;
    [Export] public string DisplayName { get; set; } = string.Empty;
    [Export] public int BaseDamage { get; set; } = 15;
    [Export] public float AttackCooldown { get; set; } = 0.35f;
    [Export] public float ReachDistance { get; set; } = 72.0f;
    [Export] public DamageType DamageType { get; set; } = DamageType.Physical;

    public abstract void OnAttackTriggered(Node2D attacker, Vector2 direction);
}
