using Godot;
using Ashbinders.Combat.Hitboxes;
using Ashbinders.Embers.Core;
using Ashbinders.Gameplay.Damage;

namespace Ashbinders.Embers.Types;

[GlobalClass]
public partial class GuardEmber : Ember
{
    [Export] public float ParryWindowSeconds { get; set; } = 0.25f;

    public GuardEmber()
    {
        EmberId = "ember_guard_01";
        Type = EmberType.Guard;
        DisplayName = "Guard Ember";
        Description = "A deep cerulean ember radiating defensive density. Grants parry windows and powers protective conduits.";
        GlowColor = new Color(0.1f, 0.4f, 0.95f);
    }
}

[GlobalClass]
public partial class ForgeEmber : Ember
{
    [Export] public int FireDamageOverTime { get; set; } = 5;

    public ForgeEmber()
    {
        EmberId = "ember_forge_01";
        Type = EmberType.Forge;
        DisplayName = "Forge Ember";
        Description = "A searing crimson ember burning with ancient furnace heat. Adds fire damage and thaws frozen mechanisms.";
        GlowColor = new Color(0.95f, 0.2f, 0.1f);
    }

    public override void ApplyToWeapon(Hitbox hitbox)
    {
        hitbox.DamageType = DamageType.ForgeFire;
    }
}

[GlobalClass]
public partial class BonelightEmber : Ember
{
    [Export] public float LifestealRatio { get; set; } = 0.15f;

    public BonelightEmber()
    {
        EmberId = "ember_bonelight_01";
        Type = EmberType.Bonelight;
        DisplayName = "Bonelight Ember";
        Description = "A pale greenish flame harvested from dark caverns. Grants lifesteal and illuminates the Bonelight Warrens.";
        GlowColor = new Color(0.4f, 0.95f, 0.4f);
    }

    public override void ApplyToWeapon(Hitbox hitbox)
    {
        hitbox.DamageType = DamageType.BonelightDrain;
    }
}

[GlobalClass]
public partial class MemoryEmber : Ember
{
    public MemoryEmber()
    {
        EmberId = "ember_memory_01";
        Type = EmberType.Memory;
        DisplayName = "Memory Ember";
        Description = "An ethereal violet flame whispering ancestral recollections. Replays historical events and grants Insight.";
        GlowColor = new Color(0.7f, 0.2f, 0.95f);
    }
}
