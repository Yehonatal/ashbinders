using Godot;
using Ashbinders.Gameplay.Damage;

namespace Ashbinders.Combat.Weapons;

[GlobalClass]
public partial class HammerHead : WeaponHead
{
    public HammerHead()
    {
        HeadId = "hammer_head";
        DisplayName = "Ashbinder Hammer Head";
        BaseDamage = 35;
        AttackCooldown = 0.65f;
        ReachDistance = 65.0f;
        BreaksArmor = true;
        StaggerForce = 350.0f;
        DamageType = DamageType.Physical;
    }

    public override void OnAttackTriggered(Node2D attacker, Vector2 direction)
    {
        // Slam armor-breaking attack
    }
}
