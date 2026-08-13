using Godot;

namespace Ashbinders.Gameplay.Damage;

public readonly struct DamageInfo
{
    public int Amount { get; }
    public DamageType Type { get; }
    public Vector2 Knockback { get; }
    public Node2D? Source { get; }

    public DamageInfo(int amount, DamageType type, Vector2 knockback, Node2D? source = null)
    {
        Amount = amount;
        Type = type;
        Knockback = knockback;
        Source = source;
    }
}

public interface IDamageable
{
    void TakeDamage(DamageInfo damage);
}
