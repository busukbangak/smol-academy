using Godot;
using System;

public abstract class Character : Entity
{


    public float Mana;

    [Export]
    public float MaxMana = 100f;

    [Export]
    public float MoveSpeed = 10f;

    private Navigation _characterNavigation;

    private Vector3[] _path = new Vector3[0];

    private int _pathIndex = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        _characterNavigation = GetParent<Navigation>();
        Mana = MaxMana;
    }

    public Vector3[] GetSimpleNavigationPath(Vector3 start, Vector3 end)
    {
        return _characterNavigation.GetSimplePath(start, end);
    }

    public void UpdateNavigationPath(Vector3 movementTarget)
    {
        LookAt(movementTarget, Vector3.Up);
        _path = GetSimpleNavigationPath(GlobalTransform.origin, movementTarget);
        _pathIndex = 0;
    }

    public void StopNavigation()
    {
        _pathIndex = _path.Length;
    }

    public bool IsNavigationComplete()
    {
        return _pathIndex >= _path.Length;
    }

    public void Navigate()
    {
        Vector3 moveVector = _path[_pathIndex] - GlobalTransform.origin;
        if (moveVector.Length() < 1)
        {
            _pathIndex++;
        }
        else
        {
            MoveAndSlide(moveVector.Normalized() * MoveSpeed);
        }
    }

    public override void OnDead()
    {
        base.OnDead();
        GetNode<CollisionShape>("CollisionShape").Disabled = true;
    }

    public override void OnRespawn()
    {
        base.OnRespawn();
        GetNode<CollisionShape>("CollisionShape").Disabled = false;
    }
}
