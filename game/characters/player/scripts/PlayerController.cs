using System;
using Godot;
using Ashbinders.Combat.Weapons;
using Ashbinders.Embers.Core;
using Ashbinders.Embers.Types;
using Ashbinders.Gameplay.Damage;
using Ashbinders.Gameplay.Health;
using Ashbinders.Gameplay.Interaction;

namespace Ashbinders.Characters.Player;

[GlobalClass]
public partial class PlayerController : CharacterBody2D, IDamageable
{
    [Export] public float BaseMoveSpeed { get; set; } = 180.0f;
    [Export] public float Acceleration { get; set; } = 1400.0f;
    [Export] public float Friction { get; set; } = 1200.0f;
    [Export] public float DashSpeed { get; set; } = 480.0f;
    [Export] public float DashDuration { get; set; } = 0.18f;
    [Export] public float DashCooldown { get; set; } = 0.6f;

    [Export] public HealthComponent? Health { get; set; }
    [Export] public InteractionDetector? Interactor { get; set; }
    [Export] public AshbinderChain? Chain { get; set; }
    [Export] public EmberSocket? ChainSocket { get; set; }

    public Vector2 FacingDirection { get; private set; } = Vector2.Down;
    public bool IsDashing { get; private set; }
    public bool IsInvulnerable => IsDashing;

    private double _dashTimer;
    private double _dashCooldownTimer;
    private Vector2 _dashDirection;

    public override void _Ready()
    {
        Health ??= GetNodeOrNull<HealthComponent>("HealthComponent");
        Interactor ??= GetNodeOrNull<InteractionDetector>("InteractionDetector");
        Chain ??= GetNodeOrNull<AshbinderChain>("AshbinderChain");
        ChainSocket ??= GetNodeOrNull<EmberSocket>("EmberSocket");
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("attack"))
        {
            PerformAttack();
        }
        else if (@event.IsActionPressed("dash"))
        {
            PerformDash();
        }
        else if (@event.IsActionPressed("interact"))
        {
            Interactor?.TryInteract(this);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        UpdateTimers(delta);

        if (IsDashing)
        {
            Velocity = _dashDirection * DashSpeed;
            MoveAndSlide();
            return;
        }

        var input = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        if (input.LengthSquared() > 0.01f)
        {
            FacingDirection = input.Normalized();
            var targetSpeed = CalculateCurrentMoveSpeed();
            Velocity = Velocity.MoveToward(input * targetSpeed, (float)(Acceleration * delta));
        }
        else
        {
            Velocity = Velocity.MoveToward(Vector2.Zero, (float)(Friction * delta));
        }

        MoveAndSlide();
    }

    private float CalculateCurrentMoveSpeed()
    {
        var speed = BaseMoveSpeed;
        if (ChainSocket?.CurrentEmber is MotionEmber motion)
        {
            speed *= motion.SpeedMultiplier;
        }
        return speed;
    }

    private void UpdateTimers(double delta)
    {
        if (_dashTimer > 0.0)
        {
            _dashTimer -= delta;
            if (_dashTimer <= 0.0)
            {
                IsDashing = false;
            }
        }

        if (_dashCooldownTimer > 0.0)
        {
            _dashCooldownTimer = Math.Max(0.0, _dashCooldownTimer - delta);
        }
    }

    public bool PerformAttack()
    {
        if (IsDashing || Chain == null) return false;
        return Chain.TryAttack(FacingDirection);
    }

    public bool PerformDash()
    {
        if (IsDashing || _dashCooldownTimer > 0.0) return false;

        IsDashing = true;
        _dashTimer = DashDuration;
        _dashCooldownTimer = DashCooldown;
        _dashDirection = Velocity.LengthSquared() > 0.01f ? Velocity.Normalized() : FacingDirection;
        return true;
    }

    public void TakeDamage(DamageInfo damage)
    {
        if (IsInvulnerable || Health == null) return;
        Health.ApplyDamage(damage.Amount);
        Velocity += damage.Knockback;
    }
}
