using Godot;
using System;

public class Shot : Hitbox
{
    public Vector3 InitalPosition;

    public float MaxRange;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Translate(new Vector3(0, 0, -40f * delta));

        if (InitalPosition.DistanceTo(GlobalTranslation) >= MaxRange)
        {
            QueueFree();
        }
    }

    public override void OnHitboxAreaEntered(Area area)
    {
        var entity = area.GetParent<Entity>();
        if (entity.AssignedTeam != Attacker.AssignedTeam)
        {
            QueueFree();
        }
    }
}
