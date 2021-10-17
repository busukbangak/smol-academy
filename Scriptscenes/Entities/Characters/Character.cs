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

    [Export]
    private NodePath _customPathNodePath;
    private Path _customPath;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        _characterNavigation = GetParent<Navigation>();
        _customPath = GetNodeOrNull<Path>(_customPathNodePath);
        if (_customPath != null)
        {
            UpdateNavigationPath(_customPath.Curve.GetBakedPoints());
        }
        Mana = MaxMana;
    }

    public void UpdateNavigationPath(Vector3 movementTarget)
    {
        _path = _characterNavigation.GetSimplePath(GlobalTransform.origin, movementTarget);
        _pathIndex = 0;
    }

    public void UpdateNavigationPath(Vector3[] path)
    {
        _path = path;
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
        // TODO: HACK, look for another solution
        if (moveVector.Length() < 1 || _customPath != null && moveVector.Length() < 7)
        {
            _pathIndex++;
        }
        else
        {
            Model.LookAt(_path[_pathIndex], Vector3.Up);
            MoveAndSlide(moveVector.Normalized() * MoveSpeed);
        }
    }

    public void ResumeCustomPathNavigation()
    {
        _path = GetNode<Path>(_customPathNodePath).Curve.GetBakedPoints();
        _pathIndex = GetClosestPathIndex();

    }

    public override void OnDead()
    {
        base.OnDead();
        GetNode<CollisionShape>("CollisionShape").Disabled = true;

        if (!IsRespawnActivated)
        {
            GetNode<Timer>("NavigationTimer").Disconnect("timeout", this, "OnNavigationTimerTimeout");
        }
    }

    public override void OnRespawn()
    {
        base.OnRespawn();
        GetNode<CollisionShape>("CollisionShape").Disabled = false;
    }

    public void OnNavigationTimerTimeout()
    {
        if (AttackTarget == null)
        {
            return;
        }

        UpdateNavigationPath(AttackTarget.GlobalTransform.origin);
    }


    public int GetClosestPathIndex()
    {
        int closestPathIndex = 0;

        for (int i = 0; i < _path.Length; i++)
        {
            if (GlobalTransform.origin.DistanceTo(_path[i]) <
                GlobalTransform.origin.DistanceTo(_path[closestPathIndex]))
            {
                closestPathIndex = i;
            }
        }
        return closestPathIndex;
    }

    
}
