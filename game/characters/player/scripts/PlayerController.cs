using System;
using Godot;
using Ashbinders.Combat.Weapons;
using Ashbinders.Core.Events;
using Ashbinders.Embers.Core;
using Ashbinders.Embers.Types;
using Ashbinders.Gameplay.Damage;
using Ashbinders.Gameplay.Health;
using Ashbinders.Gameplay.Interaction;
using Ashbinders.World.Traversal;

namespace Ashbinders.Characters.Player;

[GlobalClass]
public partial class PlayerController : CharacterBody2D, IDamageable
{
    [Export] public float BaseMoveSpeed { get; set; } = 220.0f;
    [Export] public float Acceleration { get; set; } = 1600.0f;
    [Export] public float Friction { get; set; } = 1400.0f;

    // Dash
    [Export] public float DashSpeed { get; set; } = 560.0f;
    [Export] public float DashDuration { get; set; } = 0.22f;
    [Export] public float DashCooldown { get; set; } = 0.45f;
    [Export] public float DashPeakElevation { get; set; } = 10.0f;

    // Jump
    [Export] public float JumpPeakElevation { get; set; } = 28.0f;
    [Export] public float JumpDuration { get; set; } = 0.40f;
    [Export] public float JumpHorizontalSpeed { get; set; } = 240.0f;

    // Nodes
    [Export] public HealthComponent? Health { get; set; }
    [Export] public InteractionDetector? Interactor { get; set; }
    [Export] public AshbinderChain? Chain { get; set; }
    [Export] public EmberSocket? ChainSocket { get; set; }
    [Export] public Node2D? VisualsNode { get; set; }
    [Export] public Control? DropShadowNode { get; set; }

    public PlayerState State { get; private set; } = PlayerState.Grounded;
    public Vector2 FacingDirection { get; private set; } = new Vector2(1, 0.5f).Normalized();
    public bool IsInvulnerable => State == PlayerState.Dashing;
    public float CurrentElevationZ { get; private set; } = 0.0f;

    private double _dashTimer;
    private double _dashCooldownTimer;
    private Vector2 _dashDirection;

    private double _jumpTimer;
    private Vector2 _jumpVelocity;

    private ClimbableVolume? _activeLadder;
    private WaterVolume? _activeWater;

    private double _attackVisualTimer;
    private Vector2 _initialVisualPosition = Vector2.Zero;

    // 8 Isometric Standard Facing Vectors (2:1 dimetric projection)
    private static readonly Vector2 IsoNorth     = new Vector2(0.0f, -1.0f);
    private static readonly Vector2 IsoNorthEast = new Vector2(1.0f, -0.5f).Normalized();
    private static readonly Vector2 IsoEast      = new Vector2(1.0f, 0.0f);
    private static readonly Vector2 IsoSouthEast = new Vector2(1.0f, 0.5f).Normalized();
    private static readonly Vector2 IsoSouth     = new Vector2(0.0f, 1.0f);
    private static readonly Vector2 IsoSouthWest = new Vector2(-1.0f, 0.5f).Normalized();
    private static readonly Vector2 IsoWest      = new Vector2(-1.0f, 0.0f);
    private static readonly Vector2 IsoNorthWest = new Vector2(-1.0f, -0.5f).Normalized();

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
        DropShadowNode ??= GetNodeOrNull<Control>("DropShadow");

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
        HandleGlobalActionInputs();

        switch (State)
        {
            case PlayerState.Grounded:
                ProcessGrounded(delta);
                break;
            case PlayerState.Dashing:
                ProcessDashing(delta);
                break;
            case PlayerState.Jumping:
                ProcessJumping(delta);
                break;
            case PlayerState.Climbing:
                ProcessClimbing(delta);
                break;
            case PlayerState.Swimming:
                ProcessSwimming(delta);
                break;
            case PlayerState.Hurt:
                ProcessHurt(delta);
                break;
        }

        if (Chain != null)
        {
            Chain.Rotation = FacingDirection.Angle();
        }

        QueueRedraw();
    }

    private void ProcessGrounded(double delta)
    {
        ResetElevation();

        // Check for climbing trigger
        if (_activeLadder != null && (Input.IsKeyPressed(Key.W) || Input.IsKeyPressed(Key.Up) || Input.IsKeyPressed(Key.S) || Input.IsKeyPressed(Key.Down)))
        {
            StartClimbing();
            return;
        }

        // Check for jump input (Space)
        if (Input.IsKeyPressed(Key.Space) || Input.IsActionJustPressed("jump"))
        {
            PerformJump();
            return;
        }

        // Check for dash input (K or Right Click or Shift)
        if (Input.IsActionJustPressed("dash") || Input.IsKeyPressed(Key.K) || Input.IsKeyPressed(Key.Shift))
        {
            PerformDash();
            return;
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
    }

    private void ProcessDashing(double delta)
    {
        Velocity = _dashDirection * DashSpeed;

        var progress = 1.0f - (float)(_dashTimer / DashDuration);
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

        MoveAndSlide();
    }

    private void ProcessJumping(double delta)
    {
        Velocity = _jumpVelocity;

        var progress = 1.0f - (float)(_jumpTimer / JumpDuration); // 0.0 to 1.0
        CurrentElevationZ = 4.0f * JumpPeakElevation * progress * (1.0f - progress);

        if (VisualsNode != null)
        {
            VisualsNode.Position = _initialVisualPosition + new Vector2(0, -CurrentElevationZ);
        }

        if (DropShadowNode != null)
        {
            var shadowScale = 1.0f - (CurrentElevationZ / JumpPeakElevation) * 0.45f;
            DropShadowNode.Scale = new Vector2(shadowScale, shadowScale * 0.5f);
        }

        MoveAndSlide();

        if (_jumpTimer <= 0.0)
        {
            ResetElevation();
            State = _activeWater != null ? PlayerState.Swimming : PlayerState.Grounded;
        }
    }

    private void ProcessClimbing(double delta)
    {
        ResetElevation();

        if (_activeLadder == null)
        {
            State = PlayerState.Grounded;
            return;
        }

        // Lock X-position to ladder center
        GlobalPosition = new Vector2(_activeLadder.GlobalPosition.X, GlobalPosition.Y);

        float climbInputY = 0.0f;
        if (Input.IsKeyPressed(Key.W) || Input.IsKeyPressed(Key.Up)) climbInputY -= 1.0f;
        if (Input.IsKeyPressed(Key.S) || Input.IsKeyPressed(Key.Down)) climbInputY += 1.0f;

        Velocity = new Vector2(0, climbInputY * _activeLadder.ClimbSpeed);
        MoveAndSlide();

        // Check top ledge dismount
        if (GlobalPosition.Y <= _activeLadder.TopLedgeGlobalPosition.Y)
        {
            GlobalPosition = _activeLadder.TopLedgeGlobalPosition;
            State = PlayerState.Grounded;
            EventBus.Publish(new ToastNotificationEvent("Ascended to High Ledge"));
            return;
        }

        // Check bottom floor dismount
        if (GlobalPosition.Y >= _activeLadder.BottomFloorGlobalPosition.Y)
        {
            GlobalPosition = _activeLadder.BottomFloorGlobalPosition;
            State = PlayerState.Grounded;
            return;
        }

        // Jump dismount
        if (Input.IsKeyPressed(Key.Space) || Input.IsActionJustPressed("dash"))
        {
            State = PlayerState.Grounded;
            Velocity = new Vector2(0, 100.0f);
        }
    }

    private void ProcessSwimming(double delta)
    {
        ResetElevation();

        // Slightly submerge visual torso in water
        if (VisualsNode != null)
        {
            VisualsNode.Position = _initialVisualPosition + new Vector2(0, 6.0f);
        }

        if (Input.IsActionJustPressed("dash") || Input.IsKeyPressed(Key.K) || Input.IsKeyPressed(Key.Shift))
        {
            PerformDash();
            return;
        }

        var input = GetMovementInput();
        var swimSpeed = CalculateCurrentMoveSpeed() * (_activeWater?.WaterSpeedMultiplier ?? 0.6f);

        if (input.LengthSquared() > 0.01f)
        {
            FacingDirection = QuantizeToIsometricDirection(input);
            Velocity = Velocity.MoveToward(input * swimSpeed + (_activeWater?.WaterCurrent ?? Vector2.Zero), (float)(Acceleration * 0.8f * delta));
        }
        else
        {
            Velocity = Velocity.MoveToward(_activeWater?.WaterCurrent ?? Vector2.Zero, (float)(Friction * 0.8f * delta));
        }

        MoveAndSlide();
    }

    private void ProcessHurt(double delta)
    {
        ResetElevation();
        Velocity = Velocity.MoveToward(Vector2.Zero, (float)(Friction * delta));
        MoveAndSlide();
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

    private void HandleGlobalActionInputs()
    {
        if (Input.IsActionJustPressed("attack") || Input.IsKeyPressed(Key.J) || Input.IsMouseButtonPressed(MouseButton.Left))
        {
            PerformAttack();
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
                State = _activeWater != null ? PlayerState.Swimming : PlayerState.Grounded;
            }
        }

        if (_jumpTimer > 0.0)
        {
            _jumpTimer = Math.Max(0.0, _jumpTimer - delta);
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
        if (State == PlayerState.Climbing || Chain == null) return false;
        var success = Chain.TryAttack(FacingDirection);
        if (success)
        {
            _attackVisualTimer = 0.18;
        }
        return success;
    }

    public bool PerformDash()
    {
        if (State == PlayerState.Climbing || _dashCooldownTimer > 0.0) return false;

        State = PlayerState.Dashing;
        _dashTimer = DashDuration;
        _dashCooldownTimer = DashCooldown;
        _dashDirection = Velocity.LengthSquared() > 0.01f ? Velocity.Normalized() : FacingDirection;
        return true;
    }

    public bool PerformJump()
    {
        if (State != PlayerState.Grounded) return false;

        State = PlayerState.Jumping;
        _jumpTimer = JumpDuration;
        var input = GetMovementInput();
        _jumpVelocity = input.LengthSquared() > 0.01f ? input * JumpHorizontalSpeed : FacingDirection * (JumpHorizontalSpeed * 0.5f);
        return true;
    }

    public void StartClimbing()
    {
        if (_activeLadder == null) return;
        State = PlayerState.Climbing;
        Velocity = Vector2.Zero;
    }

    public void OnAreaEntered(Area2D area)
    {
        if (area is ClimbableVolume ladder)
        {
            _activeLadder = ladder;
        }
        else if (area is WaterVolume water)
        {
            _activeWater = water;
            if (State == PlayerState.Grounded)
            {
                State = PlayerState.Swimming;
                EventBus.Publish(new ToastNotificationEvent("Entered Flooded Basin (Swimming)"));
            }
        }
    }

    public void OnAreaExited(Area2D area)
    {
        if (area == _activeLadder)
        {
            _activeLadder = null;
            if (State == PlayerState.Climbing)
            {
                State = PlayerState.Grounded;
            }
        }
        else if (area == _activeWater)
        {
            _activeWater = null;
            if (State == PlayerState.Swimming)
            {
                State = PlayerState.Grounded;
                EventBus.Publish(new ToastNotificationEvent("Exited Water Basin"));
            }
        }
    }

    public void TakeDamage(DamageInfo damage)
    {
        if (IsInvulnerable || Health == null) return;
        Health.ApplyDamage(damage.Amount);
        Velocity += damage.Knockback;
        State = PlayerState.Hurt;
    }

    public override void _Draw()
    {
        // 2D Isometric Elliptical Attack Sweep on Ground Plane
        if (_attackVisualTimer > 0.0)
        {
            var attackOffset = FacingDirection * 38.0f;
            var color = new Color(1.0f, 0.75f, 0.2f, (float)(_attackVisualTimer / 0.18) * 0.7f);
            
            DrawArc(attackOffset, 28.0f, FacingDirection.Angle() - 1.0f, FacingDirection.Angle() + 1.0f, 16, color, 4.0f);
            DrawCircle(attackOffset + FacingDirection * 8.0f, 8.0f, new Color(1.0f, 0.9f, 0.4f, (float)(_attackVisualTimer / 0.18)));
        }

        // Kinetic Dash Ghost Silhouette
        if (State == PlayerState.Dashing)
        {
            var ghostColor = new Color(0.3f, 0.7f, 1.0f, 0.45f);
            DrawArc(Vector2.Zero, 16.0f, 0, Mathf.Tau, 16, ghostColor, 2.5f);
        }

        // Swimming Surface Ripple Rings
        if (State == PlayerState.Swimming)
        {
            var rippleAlpha = (float)(Math.Sin(Time.GetTicksMsec() * 0.006) * 0.25 + 0.5);
            DrawArc(Vector2.Zero, 18.0f, 0, Mathf.Tau, 16, new Color(0.4f, 0.85f, 1.0f, rippleAlpha), 2.0f);
            DrawArc(Vector2.Zero, 26.0f, 0, Mathf.Tau, 16, new Color(0.3f, 0.7f, 0.9f, rippleAlpha * 0.5f), 1.5f);
        }
    }
}
