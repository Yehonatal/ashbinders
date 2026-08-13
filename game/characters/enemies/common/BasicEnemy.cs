using System;
using Godot;
using Ashbinders.Combat.Hitboxes;
using Ashbinders.Embers.Core;
using Ashbinders.Embers.Types;
using Ashbinders.Gameplay.Damage;
using Ashbinders.Gameplay.Health;

namespace Ashbinders.Characters.Enemies.Common;

public enum EnemyState
{
    Idle,
    Chase,
    Attack,
    Hurt,
    Dead
}

[GlobalClass]
public partial class BasicEnemy : CharacterBody2D, IDamageable
{
    [Signal]
    public delegate void DefeatedEventHandler(BasicEnemy enemy);

    [Export] public float MoveSpeed { get; set; } = 110.0f;
    [Export] public float DetectionRadius { get; set; } = 280.0f;
    [Export] public float AttackRange { get; set; } = 40.0f;
    [Export] public int BaseAttackDamage { get; set; } = 10;
    [Export] public float AttackCooldown { get; set; } = 1.2f;

    [Export] public HealthComponent? Health { get; set; }
    [Export] public Hitbox? AttackHitbox { get; set; }
    [Export] public Ember? DroppedEmberOnDeath { get; set; }

    public EnemyState State { get; private set; } = EnemyState.Idle;
    public Node2D? Target { get; set; }

    private double _attackTimer;
    private double _hitstunTimer;
    private double _attackWindupTimer;

    public override void _Ready()
    {
        Health ??= GetNodeOrNull<HealthComponent>("HealthComponent");
        AttackHitbox ??= GetNodeOrNull<Hitbox>("Hitbox");
        DroppedEmberOnDeath ??= new MotionEmber();

        if (Health != null)
        {
            Health.Died += OnDied;
        }

        if (AttackHitbox != null)
        {
            AttackHitbox.AttackerNode = this;
            AttackHitbox.BaseDamage = BaseAttackDamage;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (State == EnemyState.Dead) return;

        // Auto-acquire player target if not assigned
        if (Target == null || !GodotObject.IsInstanceValid(Target))
        {
            Target = (Node2D?)GetTree().GetFirstNodeInGroup("player") ?? GetParent()?.GetNodeOrNull<Node2D>("Player");
        }

        UpdateTimers(delta);

        switch (State)
        {
            case EnemyState.Idle:
                ProcessIdle();
                break;
            case EnemyState.Chase:
                ProcessChase(delta);
                break;
            case EnemyState.Attack:
                ProcessAttack(delta);
                break;
            case EnemyState.Hurt:
                ProcessHurt();
                break;
        }

        MoveAndSlide();
        QueueRedraw();
    }

    private void UpdateTimers(double delta)
    {
        if (_attackTimer > 0.0)
            _attackTimer = Math.Max(0.0, _attackTimer - delta);

        if (_hitstunTimer > 0.0)
        {
            _hitstunTimer = Math.Max(0.0, _hitstunTimer - delta);
            if (_hitstunTimer <= 0.0 && State == EnemyState.Hurt)
            {
                State = Target != null ? EnemyState.Chase : EnemyState.Idle;
            }
        }
    }

    private void ProcessIdle()
    {
        Velocity = Vector2.Zero;
        if (Target != null && GlobalPosition.DistanceTo(Target.GlobalPosition) <= DetectionRadius)
        {
            State = EnemyState.Chase;
        }
    }

    private void ProcessChase(double delta)
    {
        if (Target == null || !GodotObject.IsInstanceValid(Target))
        {
            State = EnemyState.Idle;
            return;
        }

        var distance = GlobalPosition.DistanceTo(Target.GlobalPosition);
        if (distance > DetectionRadius * 1.5f)
        {
            State = EnemyState.Idle;
            return;
        }

        if (distance <= AttackRange && _attackTimer <= 0.0)
        {
            State = EnemyState.Attack;
            _attackWindupTimer = 0.3;
            return;
        }

        var direction = (Target.GlobalPosition - GlobalPosition).Normalized();
        Velocity = direction * MoveSpeed;
    }

    private void ProcessAttack(double delta)
    {
        Velocity = Vector2.Zero;
        if (_attackWindupTimer > 0.0)
        {
            _attackWindupTimer -= delta;
            if (_attackWindupTimer <= 0.0)
            {
                // Deal damage if player in range
                if (Target != null && GlobalPosition.DistanceTo(Target.GlobalPosition) <= AttackRange * 1.2f)
                {
                    if (Target is IDamageable damageable)
                    {
                        var dir = (Target.GlobalPosition - GlobalPosition).Normalized();
                        damageable.TakeDamage(new DamageInfo(BaseAttackDamage, DamageType.Physical, dir * 180.0f, this));
                    }
                }
                _attackTimer = AttackCooldown;
                State = EnemyState.Chase;
            }
        }
    }

    private void ProcessHurt()
    {
        Velocity = Velocity.MoveToward(Vector2.Zero, 600.0f * (float)GetPhysicsProcessDeltaTime());
    }

    public void TakeDamage(DamageInfo damage)
    {
        if (State == EnemyState.Dead || Health == null) return;

        Health.ApplyDamage(damage.Amount);
        Velocity = damage.Knockback;
        State = EnemyState.Hurt;
        _hitstunTimer = 0.2f;
    }

    private void OnDied()
    {
        State = EnemyState.Dead;
        Velocity = Vector2.Zero;
        EmitSignal(SignalName.Defeated, this);
        QueueFree();
    }

    public override void _Draw()
    {
        if (State == EnemyState.Hurt)
        {
            // Flash white on hit
            DrawCircle(Vector2.Zero, 16.0f, Colors.White);
        }
        else if (State == EnemyState.Attack)
        {
            // Attack windup ring
            DrawCircle(Vector2.Zero, 20.0f, new Color(1.0f, 0.2f, 0.2f, 0.5f));
        }

        // Draw small health indicator above enemy
        if (Health != null && Health.CurrentHealth < Health.MaxHealth)
        {
            var hpRatio = (float)Health.CurrentHealth / Health.MaxHealth;
            DrawRect(new Rect2(-16, -24, 32, 4), new Color(0.2f, 0.2f, 0.2f));
            DrawRect(new Rect2(-16, -24, 32 * hpRatio, 4), new Color(0.9f, 0.2f, 0.2f));
        }
    }
}
