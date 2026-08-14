using Godot;
using Ashbinders.Gameplay.Damage;

namespace Ashbinders.Combat.Weapons;

[GlobalClass]
public partial class BladeHead : WeaponHead
{
    public BladeHead()
    {
        HeadId = "blade_head";
        DisplayName = "Ashbinder Blade Head";
        BaseDamage = 18;
        AttackCooldown = 0.30f;
        ReachDistance = 80.0f;
        BreaksArmor = false;
        StaggerForce = 120.0f;
        DamageType = DamageType.Physical;
    }

    public override void OnAttackTriggered(Node2D attacker, Vector2 direction)
    {
        // Slashing arc attack
    }
}
