using Godot;
using Ashbinders.Combat.Hitboxes;
using Ashbinders.Embers.Core;
using Ashbinders.Gameplay.Damage;

namespace Ashbinders.Embers.Types;

[GlobalClass]
public partial class MotionEmber : Ember
{
    [Export] public float SpeedMultiplier { get; set; } = 1.3f;
    [Export] public float DashCooldownReduction { get; set; } = 0.2f;

    public MotionEmber()
    {
        EmberId = "ember_motion_01";
        Type = EmberType.Motion;
        DisplayName = "Motion Ember";
        Description = "A radiant golden ember pulsing with kinetic urgency. Enhances mobility and adds dash-strike capabilities.";
        GlowColor = new Color(1.0f, 0.75f, 0.1f);
    }

    public override void ApplyToWeapon(Hitbox hitbox)
    {
        hitbox.DamageType = DamageType.MotionImpact;
        hitbox.KnockbackForce *= 1.4f;
    }
}
