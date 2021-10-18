using Godot;
using System;

public class Projectile : Hitbox
{
    [Export]
    private float _steerForce = 1000f;

    [Export]
    private float _speed = 25f;

    private Vector3 _velocity;

    private Entity _target;

    public void Fire(Entity target)
    {
        _target = target;
        SetAsToplevel(true);
        _velocity = -(Transform.basis.z) * 0.2f * _speed;
    }

    public override void _Process(float delta)
    {
        if (_target == null)
        {
            return;
        }

        var targetLocation = _target.GlobalTransform.origin;
        var desiredVelocity = (targetLocation - Transform.origin).Normalized() * _speed;
        var steer = (desiredVelocity - _velocity).Normalized() * _steerForce;
        _velocity += steer * delta;
        LookAt(Transform.origin + _velocity, Vector3.Up);
        Translation += _velocity * delta;
    }

    public void OnMissileBodyEntered(Entity body)
    {
        if (body == _target)
        {
            QueueFree();
        }
    }
}
