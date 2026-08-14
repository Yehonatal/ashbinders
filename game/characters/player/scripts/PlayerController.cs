using System;
using Godot;
using Ashbinders.Combat.Weapons;
using Ashbinders.Core.Events;
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
    [Export] public float DashSpeed { get; set; } = 560.0f;
    [Export] public float DashDuration { get; set; } = 0.22f;
    [Export] public float DashCooldown { get; set; } = 0.45f;
    [Export] public float DashPeakElevation { get; set; } = 12.0f;

    [Export] public HealthComponent? Health { get; set; }
    [Export] public InteractionDetector? Interactor { get; set; }
    [Export] public AshbinderChain? Chain { get; set; }
    [Export] public EmberSocket? ChainSocket { get; set; }

    [Export] public Node2D? VisualsNode { get; set; }
    [Export] public CanvasItem? DropShadowNode { get; set; }

    public Vector2 FacingDirection { get; private set; } = new Vector2(1, 0.5f).Normalized(); // Isometric SE default
    public bool IsDashing { get; private set; }
    public bool IsInvulnerable => IsDashing;
    public float CurrentElevationZ { get; private set; } = 0.0f;

    private double _dashTimer;
    private double _dashCooldownTimer;
    private Vector2 _dashDirection;
    private double _attackVisualTimer;
    private Vector2 _initialVisualPosition = Vector2.Zero;

    // 8 Isometric Standard Facing Vectors (2:1 dimetric projection)
    private static readonly Vector2 IsoNorth     = new(0.0f, -1.0f);
    private static readonly Vector2 IsoNorthEast = new(1.0f, -0.5f).Normalized();
    private static readonly Vector2 IsoEast      = new(1.0f, 0.0f);
    private static readonly Vector2 IsoSouthEast = new(1.0f, 0.5f).Normalized();
    private static readonly Vector2 IsoSouth     = new(0.0f, 1.0f);
    private static readonly Vector2 IsoSouthWest = new(-1.0f, 0.5f).Normalized();
    private static readonly Vector2 IsoWest      = new(-1.0f, 0.0f);
    private static readonly Vector2 IsoNorthWest = new(-1.0f, -0.5f).Normalized();

    private static readonly Vector2[] IsoDirections = new[]
    {
        IsoEast, IsoSouthEast, IsoSouth, IsoSouthWest,
        IsoWest, IsoNorthWest, IsoNorth, IsoNorthEast
    };

    public override void _Ready()
    {
        AddToGroup("player");

        Health ??= GetNodeOrNull<HealthComponent>("HealthComponent");
        Interactor ??= GetNodeOrNull<InteractionDetector>("InteractionDetector");
        Chain ??= GetNodeOrNull<AshbinderChain>("AshbinderChain");
        ChainSocket ??= GetNodeOrNull<EmberSocket>("EmberSocket");
        VisualsNode ??= GetNodeOrNull<Node2D>("Visuals");
        DropShadowNode ??= GetNodeOrNull<CanvasItem>("DropShadow");

        if (VisualsNode != null)
        {
            _initialVisualPosition = VisualsNode.Position;
        }

        if (Health != null)
        {
            Health.HealthChanged += (curr, max) => EventBus.Publish(new HealthChangedEvent(curr, max));
        }

        if (ChainSocket != null && ChainSocket.CurrentEmber == null)
        {
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
            UpdateDashElevation();
            MoveAndSlide();
            QueueRedraw();
            return;
        }
        else
        {
            ResetElevation();
        }

        var input = GetMovementInput();
        if (input.LengthSquared() > 0.01f)
        {
            FacingDirection = QuantizeToIsometricDirection(input);
            var targetSpeed = CalculateCurrentMoveSpeed();
            Velocity = Velocity.MoveToward(input * targetSpeed, (float)(Acceleration * delta));
        }
        else
        {
            Velocity = Velocity.MoveToward(Vector2.Zero, (float)(Friction * delta));
        }

        MoveAndSlide();

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

    public static Vector2 QuantizeToIsometricDirection(Vector2 rawDir)
    {
        if (rawDir.LengthSquared() < 0.001f) return IsoSouthEast;

        var bestDir = IsoSouthEast;
        var maxDot = -2.0f;

        foreach (var isoDir in IsoDirections)
        {
            var dot = rawDir.Dot(isoDir);
            if (dot > maxDot)
            {
                maxDot = dot;
                bestDir = isoDir;
            }
        }

        return bestDir;
    }

    private void UpdateDashElevation()
    {
        var progress = 1.0f - (float)(_dashTimer / DashDuration); // 0.0 to 1.0
        CurrentElevationZ = Mathf.Sin(progress * Mathf.Pi) * DashPeakElevation;

        if (VisualsNode != null)
        {
            VisualsNode.Position = _initialVisualPosition + new Vector2(0, -CurrentElevationZ);
        }

        if (DropShadowNode != null)
        {
            var shadowScale = 1.0f - (CurrentElevationZ / DashPeakElevation) * 0.25f;
            DropShadowNode.Scale = new Vector2(shadowScale, shadowScale * 0.5f);
        }
    }

    private void ResetElevation()
    {
        CurrentElevationZ = 0.0f;
        if (VisualsNode != null)
        {
            VisualsNode.Position = _initialVisualPosition;
        }
        if (DropShadowNode != null)
        {
            DropShadowNode.Scale = new Vector2(1.0f, 0.5f);
        }
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

        // Weapon Head Switching
        if (Input.IsKeyPressed(Key.Key1)) Chain?.SwitchHead(0); // Blade Head
        if (Input.IsKeyPressed(Key.Key2)) Chain?.SwitchHead(1); // Hammer Head
        if (Input.IsKeyPressed(Key.Key3)) Chain?.SwitchHead(2); // Twin Sickles
        if (Input.IsKeyPressed(Key.Key4)) Chain?.SwitchHead(3); // Spear Tip
        if (Input.IsKeyPressed(Key.Q)) Chain?.NextHead();
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
            _attackVisualTimer = 0.18;
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
        // 2D Isometric Elliptical Attack Sweep on Ground Plane
        if (_attackVisualTimer > 0.0)
        {
            var attackOffset = FacingDirection * 38.0f;
            var color = new Color(1.0f, 0.75f, 0.2f, (float)(_attackVisualTimer / 0.18) * 0.7f);
            
            // Draw 2:1 isometric arc
            DrawArc(attackOffset, 28.0f, FacingDirection.Angle() - 1.0f, FacingDirection.Angle() + 1.0f, 16, color, 4.0f);
            DrawCircle(attackOffset + FacingDirection * 8.0f, 8.0f, new Color(1.0f, 0.9f, 0.4f, (float)(_attackVisualTimer / 0.18)));
        }

        // Kinetic Dash Ghost Silhouette
        if (IsDashing)
        {
            var ghostColor = new Color(0.3f, 0.7f, 1.0f, 0.45f);
            DrawArc(Vector2.Zero, 16.0f, 0, Mathf.Tau, 16, ghostColor, 2.5f);
        }
    }
}
