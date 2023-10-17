using Godot;
using System;

public class Projectile : Hitbox
{
    [Export]
    private float _steerForce = 1000f;

    [Export]
    private float _speed = 25f;

    private Vector3 _velocity;

    public Entity Target;

    public void Fire(Entity target)
    {
        Target = target;
        SetAsToplevel(true);
        _velocity = -Transform.basis.z * 0.2f * _speed;
    }

    public override void _Process(float delta)
    {
        if (Target == null || IsQueuedForDeletion())
        {
            return;
        }

        // TODO BUGFIX: when projectile flies to minion which is already queued free, this is quickfix hack
        try
        {
            var targetLocation = Target.GlobalTransform.origin;
            var desiredVelocity = (targetLocation - Transform.origin).Normalized() * _speed;
            var steer = (desiredVelocity - _velocity).Normalized() * _steerForce;

            var velocity = steer * delta;
            _velocity += velocity;
            LookAt(Transform.origin + _velocity, Vector3.Up);
            Translation += _velocity * delta;
        }
        catch (Exception e)
        {
            GD.PrintErr(e);
            QueueFree();
        }

    }

    public override void OnHitboxAreaEntered(Area area)
    {
        if (area.GetParent<Entity>() == Target)
        {
            QueueFree();
        }
    }
}
