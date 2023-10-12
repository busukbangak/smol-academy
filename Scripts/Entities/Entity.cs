using Godot;
using System;
using System.Collections.Generic;

public enum TeamColor
{
    Blue,
    Red
}

public abstract class Entity : KinematicBody
{

    [Signal]
    public delegate void Die(Entity entity);

    [Signal]
    public delegate void Respawned(Entity entity);

    [Signal]
    public delegate void Kill(Entity entity, Entity killedEntity);

    [Signal]
    public delegate void XPGained(Entity entity);

    [Signal]
    public delegate void LevelIncreased(Entity entity);

    [Export]
    public TeamColor AssignedTeam;

    [Export]
    public int Level = 1;

    [Export(PropertyHint.Range, "0,1")]
    public float LevelMultiplicator = 0.05f;

    public float Health;

    [Export]
    public float MaxHealth = 100f;

    [Export]
    public float AttackDamage = 10f;

    [Export]
    public float AttackRange = 5f;

    [Export]
    public float AttackSpeed = 1f;

    [Export]
    public PackedScene Projectile;

    protected Entity AttackTarget;

    private AnimationPlayer _animationPlayer;

    public int Kills = 0;

    public int Deaths = 0;

    public int Assists = 0;

    public int MinionKills = 0;

    public int Gold = 0;

    public float RespawnTime = 5f;

    public Timer RespawnTimer;

    private SphereShape _detectionAreaSphere;

    protected List<Entity> EntitiesInDetectionArea = new List<Entity>();

    private SphereShape _experienceAreaSphere;

    protected List<Entity> EntitiesInExperienceArea = new List<Entity>();

    public Spatial Model;

    public float Experience = 0;

    private float _experienceTotal = 0;

    [Signal]
    public delegate void UpdateHealthbar(float maxHealth, float health);

    [Export]
    public bool IsRespawnActivated;

    public Timer AttackTimer;

    public override void _Ready()
    {
        _animationPlayer = GetNode<Spatial>("Model").GetChild(0).GetNode<AnimationPlayer>("AnimationPlayer");

        var area = (Area)GetNode<Area>("DetectionArea");
        var sphereShape = new SphereShape();
        sphereShape.Radius = AttackRange / Scale.x;
        area.ShapeOwnerAddShape(0, sphereShape);

        AttackTimer = GetNode<Timer>("AttackTimer");
        AttackTimer.WaitTime = 1 / AttackSpeed;

        Model = GetNode<Spatial>("Model");
        Health = MaxHealth;

        RespawnTimer = GetNode<Timer>("RespawnTimer");
        RespawnTimer.WaitTime = RespawnTime;

        UpdateHealth();
    }

    public void OnEntityMouseEntered()
    {
        if (Health > 0 && Globals.PlayerData.AssignedTeam != AssignedTeam)
        {
            CursorManager.Instance.ChangeCursor(CursorType.Attack);
        }
    }

    public void OnEntityMouseExited()
    {
        CursorManager.Instance.ChangeCursor(CursorType.Default);
    }

    public void PlayAnimation(string animationName)
    {
        _animationPlayer.Play(animationName);
    }

    public void StopAnimation()
    {
        _animationPlayer.Stop();
    }

    public void UpdateAnimationPlaybackSpeed(float playbackSpeed)
    {
        _animationPlayer.PlaybackSpeed = playbackSpeed;
    }

    public void OnAttackHit()
    {
        if (AttackTarget == null)
        {
            return;
        }

        AttackTarget.Health -= AttackDamage;
        AttackTarget.UpdateHealth();

        if (AttackTarget.Health <= 0)
        {
            if (AttackTarget is Minion)
            {
                MinionKills++;
                Gold += 21;
            }
            else if (AttackTarget is CannonMinion)
            {
                MinionKills++;
                Gold += 60;
            }
            else if (AttackTarget is Smol)
            {
                Kills++;
                Gold += 300;
            }

            EmitSignal(nameof(Kill), this, AttackTarget);

            AttackTarget = null;
            return;
        }
    }

    public virtual void OnDead()
    {
        AttackTarget = null;
        CollisionLayer = 0b00000000000000000001;
        Deaths++;



        EmitSignal(nameof(Die), this);

        if (IsRespawnActivated)
        {
            Respawn();
        }
    }

    public virtual void OnRespawn()
    {
        CollisionLayer = 0b00000000000000000010;
        Health = MaxHealth;
        EmitSignal(nameof(Respawned), this);
    }

    public void Respawn()
    {
        RespawnTimer.Start();
    }

    public void OnExperienceAreaBodyEntered(Entity body)
    {
        if (body == this || IsQueuedForDeletion() || body.Health <= 0)
        {
            return;
        }

        EntitiesInExperienceArea.Add(body);
    }

    public void OnExperienceAreaBodyExited(Entity body)
    {
        if (body.AssignedTeam != AssignedTeam && body.Health <= 0)
        {
            if (body is Minion)
            {
                AddExperience(60);
            }
            else if (body is CannonMinion)
            {
                AddExperience(93);
            }
            else if (body is Smol)
            {
                AddExperience(55.76f * AttackTarget.Level - 13.76f);
            }
        }
        EntitiesInExperienceArea.Remove(body);
    }

    public void OnDetectionAreaBodyEntered(Entity body)
    {
        if (body == this || IsQueuedForDeletion() || body.Health <= 0)
        {
            return;
        }

        EntitiesInDetectionArea.Add(body);
    }

    public void OnDetectionAreaBodyExited(Entity body)
    {
        EntitiesInDetectionArea.Remove(body);
    }

    public List<Entity> EnemyEntitiesInDetectionArea()
    {
        List<Entity> enemyEntitiesInDetectionArea = new List<Entity>();
        foreach (var entity in EntitiesInDetectionArea)
        {
            if (entity.AssignedTeam != AssignedTeam)
            {
                enemyEntitiesInDetectionArea.Add(entity);
            }
        }
        return enemyEntitiesInDetectionArea;
    }

    public Entity GetClosestEnemyEntityInDetectionArea()
    {
        Entity closestEntity = null;
        foreach (var entity in EnemyEntitiesInDetectionArea())
        {
            if (closestEntity == null)
            {
                closestEntity = entity;
                continue;
            }

            if (GlobalTransform.origin.DistanceTo(entity.GlobalTransform.origin) <
                GlobalTransform.origin.DistanceTo(closestEntity.GlobalTransform.origin))
            {
                closestEntity = entity;
            }
        }
        return closestEntity;
    }

    public void OnHurtboxAreaEntered(Hitbox area)
    {
        if (area is Projectile projectile && projectile.Target != this)
        {
            return;
        }

        Health -= area.Damage;
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        EmitSignal(nameof(UpdateHealthbar), MaxHealth, Health);
    }

    public Projectile FireProjectile()
    {
        Projectile projectile = (Projectile)Projectile.Instance();
        AddChild(projectile);
        projectile.Translate(new Vector3(0, 9, 0));
        projectile.Damage = AttackDamage;
        projectile.Fire(AttackTarget);
        return projectile;
    }

    public float GetRequiredExperienceForLevel(int level)
    {
        return 280 + 100 * (level - 2);
    }

    public void AddExperience(float amount)
    {
        _experienceTotal += amount;
        Experience += amount;

        while (Experience >= GetRequiredExperienceForLevel(Level + 1))
        {
            Experience -= GetRequiredExperienceForLevel(Level + 1);
            LevelUp();
        }

        EmitSignal(nameof(XPGained), this);
    }

    public void LevelUp()
    {
        Level++;
        Health *= LevelMultiplicator + 1;
        AttackDamage *= LevelMultiplicator + 1;
        AttackSpeed *= LevelMultiplicator + 1;
        EmitSignal(nameof(LevelIncreased));
    }
}
