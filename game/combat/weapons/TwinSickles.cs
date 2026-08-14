using Godot;
using Ashbinders.Gameplay.Damage;

namespace Ashbinders.Combat.Weapons;

[GlobalClass]
public partial class TwinSickles : WeaponHead
{
    public TwinSickles()
    {
        HeadId = "twin_sickles";
        DisplayName = "Ashbinder Twin Sickles";
        BaseDamage = 10;
        AttackCooldown = 0.15f;
        ReachDistance = 55.0f;
        BreaksArmor = false;
        StaggerForce = 60.0f;
        DamageType = DamageType.Physical;
    }

    public override void OnAttackTriggered(Node2D attacker, Vector2 direction)
    {
        // Rapid multi-slash attack
    }
}
