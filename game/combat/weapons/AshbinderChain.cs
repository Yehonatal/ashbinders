using System;
using System.Collections.Generic;
using Godot;
using Ashbinders.Combat.Hitboxes;
using Ashbinders.Core.Events;
using Ashbinders.Embers.Core;

namespace Ashbinders.Combat.Weapons;

[GlobalClass]
public partial class AshbinderChain : Node2D
{
    [Export] public WeaponHead? CurrentHead { get; set; }
    [Export] public Hitbox? AttackHitbox { get; set; }
    [Export] public EmberSocket? Socket { get; set; }

    private double _attackCooldownTimer;
    private readonly List<WeaponHead> _availableHeads = new()
    {
        new BladeHead(),
        new HammerHead(),
        new TwinSickles(),
        new SpearTip()
    };
    private int _currentHeadIndex = 0;

    public bool CanAttack => _attackCooldownTimer <= 0.0;
    public WeaponHead ActiveHead => CurrentHead ?? _availableHeads[0];

    public override void _Ready()
    {
        CurrentHead ??= _availableHeads[0];
        UpdateHitboxConfig();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_attackCooldownTimer > 0.0)
        {
            _attackCooldownTimer = Math.Max(0.0, _attackCooldownTimer - delta);
        }
    }

    public void SwitchHead(int index)
    {
        if (index < 0 || index >= _availableHeads.Count) return;
        _currentHeadIndex = index;
        CurrentHead = _availableHeads[_currentHeadIndex];
        UpdateHitboxConfig();
        EventBus.Publish(new WeaponHeadChangedEvent(CurrentHead.DisplayName));
        EventBus.Publish(new ToastNotificationEvent($"Switched Weapon Head: {CurrentHead.DisplayName}"));
    }

    public void NextHead()
    {
        SwitchHead((_currentHeadIndex + 1) % _availableHeads.Count);
    }

    private void UpdateHitboxConfig()
    {
        if (AttackHitbox != null && CurrentHead != null)
        {
            AttackHitbox.AttackerNode = this;
            AttackHitbox.BaseDamage = CurrentHead.BaseDamage;
        }
    }

    public bool TryAttack(Vector2 attackDirection)
    {
        if (!CanAttack || CurrentHead == null) return false;

        _attackCooldownTimer = CurrentHead.AttackCooldown;
        CurrentHead.OnAttackTriggered(this, attackDirection);

        if (Socket?.CurrentEmber != null && AttackHitbox != null)
        {
            Socket.CurrentEmber.ApplyToWeapon(AttackHitbox);
        }

        return true;
    }
}
