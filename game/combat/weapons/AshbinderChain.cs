using System;
using Godot;
using Ashbinders.Combat.Hitboxes;
using Ashbinders.Embers.Core;

namespace Ashbinders.Combat.Weapons;

[GlobalClass]
public partial class AshbinderChain : Node2D
{
    [Export] public WeaponHead? CurrentHead { get; set; }
    [Export] public Hitbox? AttackHitbox { get; set; }
    [Export] public EmberSocket? Socket { get; set; }

    private double _attackCooldownTimer;

    public bool CanAttack => _attackCooldownTimer <= 0.0;

    public override void _Ready()
    {
        CurrentHead ??= new BladeHead();
        if (AttackHitbox != null)
        {
            AttackHitbox.AttackerNode = this;
            AttackHitbox.BaseDamage = CurrentHead.BaseDamage;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_attackCooldownTimer > 0.0)
        {
            _attackCooldownTimer = Math.Max(0.0, _attackCooldownTimer - delta);
        }
    }

    public bool TryAttack(Vector2 attackDirection)
    {
        if (!CanAttack || CurrentHead == null) return false;

        _attackCooldownTimer = CurrentHead.AttackCooldown;
        CurrentHead.OnAttackTriggered(this, attackDirection);

        // Apply socket modifications (e.g. Forge adds fire damage, Motion adds speed)
        if (Socket?.CurrentEmber != null && AttackHitbox != null)
        {
            Socket.CurrentEmber.ApplyToWeapon(AttackHitbox);
        }

        return true;
    }
}
