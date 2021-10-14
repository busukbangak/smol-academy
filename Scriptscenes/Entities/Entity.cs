using Godot;
using System;

public enum TeamColor
{
    Blue,
    Red
}

public class Entity : KinematicBody
{

    [Export]
    public TeamColor AssignedTeam;

    [Export]
    public float Health = 100f;

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

    public int Kills;

    public int Deaths;

    public int Assists;

    public int Money;

    public override void _Ready()
    {
        _animationPlayer = GetNode("Model").GetChild(0).GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public void OnEntityMouseEntered()
    {
        CursorManager.ChangeCursor(CursorType.Attack);
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
        
    }

}
