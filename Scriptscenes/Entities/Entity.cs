using Godot;
using System;

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

    [Export]
    public bool IsRespawnActivated;

    public override void _Ready()
    {
        _animationPlayer = GetNode("Model").GetChild(0).GetNode<AnimationPlayer>("AnimationPlayer");
        Health = MaxHealth;
    }

    public void OnEntityMouseEntered()
    {
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

    public void OnDead()
    {
        Deaths++;

        if (IsRespawnActivated)
        {
            Respawn();
        }
    }

    public async void Respawn()
    {
        await ToSignal(GetTree().CreateTimer(RespawnTime), "timeout");
        Health = MaxHealth;
        GD.Print("RESPAWNED");
    }

}
