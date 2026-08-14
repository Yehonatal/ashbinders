using Godot;

namespace Ashbinders.Gameplay.Damage;

public readonly struct DamageInfo
{
    public int Amount { get; }
    public DamageType Type { get; }
    public Vector2 Knockback { get; }
    public Node2D? Source { get; }
    public bool BreaksArmor { get; }
    public float StaggerForce { get; }

    public DamageInfo(int amount, DamageType type, Vector2 knockback, Node2D? source = null, bool breaksArmor = false, float staggerForce = 0.0f)
    {
        Amount = amount;
        Type = type;
        Knockback = knockback;
        Source = source;
        BreaksArmor = breaksArmor;
        StaggerForce = staggerForce;
    }
}

public interface IDamageable
{
    void TakeDamage(DamageInfo damage);
}
