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

    [Export]
    public TeamColor AssignedTeam;

    [Export]
    public int Level = 1;

    [Export(PropertyHint.Range, "0,1")]
    public float LevelMultiplicator = 0.1f;

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
    public bool IsMelee = true;

    protected Entity AttackTarget;

    private AnimationPlayer _animationPlayer;

    public int Kills = 0;

    public int Deaths = 0;

    public int Assists = 0;

    public int Money = 0;

    public float RespawnTime = 5f;

    private SphereShape _detectionAreaSphere;

    protected List<Entity> EntitiesInDetectionArea = new List<Entity>();

    protected Spatial Model;

    [Signal]
    public delegate void UpdateHealthbar(float health, float maxHealth);

    [Export]
    public bool IsRespawnActivated;

    public override void _Ready()
    {
        _animationPlayer = GetNode<Spatial>("Model").GetChild(0).GetNode<AnimationPlayer>("AnimationPlayer");
        _detectionAreaSphere = (SphereShape)GetNode<CollisionShape>("DetectionArea/CollisionShape").Shape;
        // TODO: Radius wont be set for some objects
        _detectionAreaSphere.Radius = AttackRange / Scale.x;
        Model = GetNode<Spatial>("Model");
        Health = MaxHealth;

        EmitSignal(nameof(UpdateHealthbar), Health, MaxHealth);
    }
    public void OnEntityMouseEntered()
    {
        if (Health > 0 && PlayerData.AssignedTeam != AssignedTeam)
        {
            CursorManager.ChangeCursor(CursorType.Attack);
        }
    }

    public void OnEntityMouseExited()
    {
        CursorManager.ChangeCursor(CursorType.Default);
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
        AttackTarget.EmitSignal(nameof(UpdateHealthbar), AttackTarget.Health, MaxHealth);
        if (AttackTarget.Health <= 0)
        {
            Kills++;
            AttackTarget = null;
            return;
        }
    }

    public virtual void OnDead()
    {

        CollisionLayer = 0b00000000000000000001;
        Deaths++;

        if (IsRespawnActivated)
        {
            Respawn();
        }
    }

    public virtual void OnRespawn()
    {
        CollisionLayer = 0b00000000000000000010;
        Health = MaxHealth;
    }

    public async void Respawn()
    {
        await ToSignal(GetTree().CreateTimer(RespawnTime), "timeout");
        OnRespawn();
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

            if (GlobalTransform.origin.DistanceTo(closestEntity.GlobalTransform.origin) <
                GlobalTransform.origin.DistanceTo(entity.GlobalTransform.origin))
            {
                closestEntity = entity;
            }
        }
        return closestEntity;
    }

}
