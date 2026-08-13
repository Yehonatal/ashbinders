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
    [Export] public float BaseMoveSpeed { get; set; } = 220.0f;
    [Export] public float Acceleration { get; set; } = 1600.0f;
    [Export] public float Friction { get; set; } = 1400.0f;
    [Export] public float DashSpeed { get; set; } = 550.0f;
    [Export] public float DashDuration { get; set; } = 0.2f;
    [Export] public float DashCooldown { get; set; } = 0.5f;

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
    private Node2D? _visualNode;
    private double _attackVisualTimer;

    public override void _Ready()
    {
        AddToGroup("player");

        Health ??= GetNodeOrNull<HealthComponent>("HealthComponent");
        Interactor ??= GetNodeOrNull<InteractionDetector>("InteractionDetector");
        Chain ??= GetNodeOrNull<AshbinderChain>("AshbinderChain");
        ChainSocket ??= GetNodeOrNull<EmberSocket>("EmberSocket");
        _visualNode = GetNodeOrNull<Node2D>("VisualPlaceholder");

        if (ChainSocket != null && ChainSocket.CurrentEmber == null)
        {
            // Give Kael 1 Motion Ember to start Phase 1 prototype
            ChainSocket.TryInsertEmber(new MotionEmber());
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        UpdateTimers(delta);
        HandleActionInputs();

        if (IsDashing)
        {
            Velocity = _dashDirection * DashSpeed;
            MoveAndSlide();
            return;
        }

        var input = GetMovementInput();
        if (input.LengthSquared() > 0.01f)
        {
            FacingDirection = input;
            var targetSpeed = CalculateCurrentMoveSpeed();
            Velocity = Velocity.MoveToward(input * targetSpeed, (float)(Acceleration * delta));
        }
        else
        {
            Velocity = Velocity.MoveToward(Vector2.Zero, (float)(Friction * delta));
        }

        MoveAndSlide();

        // Update Chain orientation
        if (Chain != null)
        {
            Chain.Rotation = FacingDirection.Angle();
        }

        QueueRedraw();
    }

    private Vector2 GetMovementInput()
    {
        var dir = Vector2.Zero;

        if (Input.IsActionPressed("move_right") || Input.IsKeyPressed(Key.D) || Input.IsKeyPressed(Key.Right))
            dir.X += 1.0f;
        if (Input.IsActionPressed("move_left") || Input.IsKeyPressed(Key.A) || Input.IsKeyPressed(Key.Left))
            dir.X -= 1.0f;
        if (Input.IsActionPressed("move_down") || Input.IsKeyPressed(Key.S) || Input.IsKeyPressed(Key.Down))
            dir.Y += 1.0f;
        if (Input.IsActionPressed("move_up") || Input.IsKeyPressed(Key.W) || Input.IsKeyPressed(Key.Up))
            dir.Y -= 1.0f;

        return dir.Normalized();
    }

    private void HandleActionInputs()
    {
        if (Input.IsActionJustPressed("attack") || Input.IsKeyPressed(Key.J) || Input.IsMouseButtonPressed(MouseButton.Left))
        {
            PerformAttack();
        }

        if (Input.IsActionJustPressed("dash") || Input.IsKeyPressed(Key.K) || Input.IsKeyPressed(Key.Space))
        {
            PerformDash();
        }

        if (Input.IsActionJustPressed("interact") || Input.IsKeyPressed(Key.E))
        {
            Interactor?.TryInteract(this);
        }
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

        if (_attackVisualTimer > 0.0)
        {
            _attackVisualTimer = Math.Max(0.0, _attackVisualTimer - delta);
        }
    }

    public bool PerformAttack()
    {
        if (IsDashing || Chain == null) return false;
        var success = Chain.TryAttack(FacingDirection);
        if (success)
        {
            _attackVisualTimer = 0.15;
        }
        return success;
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

    public override void _Draw()
    {
        if (_attackVisualTimer > 0.0)
        {
            // Visual slash arc for Ashbinder Chain attack
            var attackOffset = FacingDirection * 40.0f;
            DrawCircle(attackOffset, 24.0f, new Color(1.0f, 0.8f, 0.2f, 0.6f));
        }

        if (IsDashing)
        {
            // Dash ghost ring
            DrawCircle(Vector2.Zero, 18.0f, new Color(0.4f, 0.7f, 1.0f, 0.4f));
        }
    }
}
