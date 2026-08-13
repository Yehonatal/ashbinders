using Godot;
using Ashbinders.Gameplay.Damage;

namespace Ashbinders.Combat.Weapons;

[GlobalClass]
public partial class BladeHead : WeaponHead
{
    public BladeHead()
    {
        HeadId = "blade_head_01";
        DisplayName = "Ashbinder Blade Head";
        BaseDamage = 15;
        AttackCooldown = 0.35f;
        ReachDistance = 72.0f;
        DamageType = DamageType.Physical;
    }

    public override void OnAttackTriggered(Node2D attacker, Vector2 direction)
    {
        // Executes standard arc slash logic
    }
}
