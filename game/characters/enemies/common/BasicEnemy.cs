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
    [Export] public float DetectionRadius { get; set; } = 180.0f;
    [Export] public float AttackRange { get; set; } = 36.0f;
    [Export] public int BaseAttackDamage { get; set; } = 10;
    [Export] public float AttackCooldown { get; set; } = 1.2f;

    [Export] public HealthComponent? Health { get; set; }
    [Export] public Hitbox? AttackHitbox { get; set; }
    [Export] public Ember? DroppedEmberOnDeath { get; set; }

    public EnemyState State { get; private set; } = EnemyState.Idle;
    public Node2D? Target { get; set; }

    private double _attackTimer;
    private double _hitstunTimer;

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
                ProcessAttack();
                break;
            case EnemyState.Hurt:
                ProcessHurt();
                break;
        }

        MoveAndSlide();
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
            return;
        }

        var direction = (Target.GlobalPosition - GlobalPosition).Normalized();
        Velocity = direction * MoveSpeed;
    }

    private void ProcessAttack()
    {
        Velocity = Vector2.Zero;
        _attackTimer = AttackCooldown;

        // Trigger attack animation / hitbox active frames
        State = EnemyState.Chase;
    }

    private void ProcessHurt()
    {
        // Decelerate during hitstun
        Velocity = Velocity.MoveToward(Vector2.Zero, 800.0f * (float)GetPhysicsProcessDeltaTime());
    }

    public void TakeDamage(DamageInfo damage)
    {
        if (State == EnemyState.Dead || Health == null) return;

        Health.ApplyDamage(damage.Amount);
        Velocity = damage.Knockback;
        State = EnemyState.Hurt;
        _hitstunTimer = 0.15f;
    }

    private void OnDied()
    {
        State = EnemyState.Dead;
        Velocity = Vector2.Zero;
        EmitSignal(SignalName.Defeated, this);

        // Spawn Ember drop or fade out
        QueueFree();
    }
}
