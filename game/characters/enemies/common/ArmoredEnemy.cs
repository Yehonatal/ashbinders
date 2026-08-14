using System;
using Godot;
using Ashbinders.Combat.Hitboxes;
using Ashbinders.Core.Events;
using Ashbinders.Embers.Core;
using Ashbinders.Embers.Types;
using Ashbinders.Gameplay.Damage;
using Ashbinders.Gameplay.Health;

namespace Ashbinders.Characters.Enemies.Common;

[GlobalClass]
public partial class ArmoredEnemy : CharacterBody2D, IDamageable
{
    [Signal]
    public delegate void DefeatedEventHandler(ArmoredEnemy enemy);

    [Export] public float MoveSpeed { get; set; } = 70.0f;
    [Export] public float DetectionRadius { get; set; } = 240.0f;
    [Export] public float AttackRange { get; set; } = 50.0f;
    [Export] public int BaseAttackDamage { get; set; } = 22;
    [Export] public float AttackCooldown { get; set; } = 1.8f;
    [Export] public bool HasArmor { get; set; } = true;

    [Export] public HealthComponent? Health { get; set; }
    [Export] public Ember? DroppedEmberOnDeath { get; set; }

    public EnemyState State { get; private set; } = EnemyState.Idle;
    public Node2D? Target { get; set; }

    private double _attackTimer;
    private double _hitstunTimer;
    private double _attackWindupTimer;

    public override void _Ready()
    {
        Health ??= GetNodeOrNull<HealthComponent>("HealthComponent");
        if (Health != null)
        {
            Health.SetHealthDirectly(120, 120);
            Health.Died += OnDied;
        }

        DroppedEmberOnDeath ??= new GuardEmber();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (State == EnemyState.Dead) return;

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
            _attackWindupTimer = 0.5;
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
                if (Target != null && GlobalPosition.DistanceTo(Target.GlobalPosition) <= AttackRange * 1.2f)
                {
                    if (Target is IDamageable damageable)
                    {
                        var dir = (Target.GlobalPosition - GlobalPosition).Normalized();
                        damageable.TakeDamage(new DamageInfo(BaseAttackDamage, DamageType.Physical, dir * 250.0f, this));
                    }
                }
                _attackTimer = AttackCooldown;
                State = EnemyState.Chase;
            }
        }
    }

    private void ProcessHurt()
    {
        Velocity = Velocity.MoveToward(Vector2.Zero, 400.0f * (float)GetPhysicsProcessDeltaTime());
    }

    public void TakeDamage(DamageInfo damage)
    {
        if (State == EnemyState.Dead || Health == null) return;

        int finalDamage = damage.Amount;

        if (HasArmor)
        {
            if (damage.BreaksArmor)
            {
                HasArmor = false;
                EventBus.Publish(new ToastNotificationEvent("ARMOR SHATTERED! (Hammer Head Impact)"));
            }
            else
            {
                finalDamage = Math.Max(2, damage.Amount / 5);
                EventBus.Publish(new ToastNotificationEvent("Attack Deflected by Armor! (Use Hammer Head)"));
            }
        }

        Health.ApplyDamage(finalDamage);
        Velocity = damage.Knockback * (HasArmor ? 0.3f : 1.0f);
        State = EnemyState.Hurt;
        _hitstunTimer = 0.25f;
    }

    private void OnDied()
    {
        State = EnemyState.Dead;
        Velocity = Vector2.Zero;

        var pickup = new EmberPickup();
        if (DroppedEmberOnDeath != null)
        {
            pickup.ExtractedEmber = DroppedEmberOnDeath;
        }
        pickup.GlobalPosition = GlobalPosition;
        GetParent()?.CallDeferred("add_child", pickup);

        EmitSignal(SignalName.Defeated, this);
        QueueFree();
    }

    public override void _Draw()
    {
        var color = HasArmor ? new Color(0.4f, 0.45f, 0.55f) : new Color(0.7f, 0.2f, 0.2f);
        if (State == EnemyState.Hurt) color = Colors.White;

        DrawCircle(Vector2.Zero, 22.0f, color);

        if (HasArmor)
        {
            DrawArc(Vector2.Zero, 26.0f, 0, Mathf.Tau, 24, new Color(0.9f, 0.95f, 1.0f), 3.0f);
        }

        if (Health != null && Health.CurrentHealth < Health.MaxHealth)
        {
            var hpRatio = (float)Health.CurrentHealth / Health.MaxHealth;
            DrawRect(new Rect2(-20, -32, 40, 5), new Color(0.2f, 0.2f, 0.2f));
            DrawRect(new Rect2(-20, -32, 40 * hpRatio, 5), HasArmor ? new Color(0.3f, 0.6f, 0.9f) : new Color(0.9f, 0.2f, 0.2f));
        }
    }
}
