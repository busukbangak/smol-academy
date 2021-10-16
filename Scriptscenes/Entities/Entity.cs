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
    public float LevelMultiplicator = 2f;

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

    [Export]
    public bool IsRespawnActivated;

    public override void _Ready()
    {
        _animationPlayer = GetNode<Spatial>("Model").GetChild(0).GetNode<AnimationPlayer>("AnimationPlayer");


        _detectionAreaSphere = (SphereShape)GetNode<CollisionShape>("DetectionArea/CollisionShape").Shape;
        // TODO: Radius wont be set for some objects
        _detectionAreaSphere.Radius = AttackRange / Scale.x;
        Health = MaxHealth;
    }
    public void OnEntityMouseEntered()
    {
        // TODO: Bug, where it still shows sword on yourself
        if (Health > 0)
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
        GD.Print(AttackTarget.Health);
        if (AttackTarget.Health <= 0)
        {
            Kills++;
            AttackTarget = null;
            return;
        }
    }

    public virtual void OnDead()
    {
        Deaths++;
        if (IsRespawnActivated)
        {
            Respawn();
        }
    }

    public virtual void OnRespawn()
    {
        Health = MaxHealth;
    }

    public async void Respawn()
    {
        await ToSignal(GetTree().CreateTimer(RespawnTime), "timeout");
        OnRespawn();
    }

    public void OnDetectionAreaBodyEntered(Entity body)
    {
        if (body == this)
        {
            return;
        }

        EntitiesInDetectionArea.Add(body);
    }

    public void OnDetectionAreaBodyExited(Entity body)
    {
        EntitiesInDetectionArea.Remove(body);
    }
}
