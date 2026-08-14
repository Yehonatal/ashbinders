using Godot;
using Ashbinders.Gameplay.Damage;

namespace Ashbinders.Combat.Weapons;

[GlobalClass]
public partial class SpearTip : WeaponHead
{
    public SpearTip()
    {
        HeadId = "spear_tip";
        DisplayName = "Ashbinder Spear Tip";
        BaseDamage = 22;
        AttackCooldown = 0.40f;
        ReachDistance = 130.0f;
        BreaksArmor = false;
        StaggerForce = 180.0f;
        DamageType = DamageType.Piercing;
    }

    public override void OnAttackTriggered(Node2D attacker, Vector2 direction)
    {
        // Long-range piercing thrust
    }
}
