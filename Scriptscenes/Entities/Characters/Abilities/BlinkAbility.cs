using Godot;
using System;

public class BlinkAbility : Ability
{

    public float MaxRange = 15f;

    public override void Cast()
    {
        var mousePosition = (Vector3)Utilities.MouseRaycast(Smol.GetViewport().GetCamera())["position"];
        var flooredMousePosition = new Vector3(mousePosition.x, Smol.Translation.y, mousePosition.z);
        var direction = Smol.GlobalTransform.origin.DirectionTo(flooredMousePosition);
        var distanceToMousePosition = Smol.GlobalTransform.origin.DistanceTo(flooredMousePosition);
        Smol.Model.LookAt(Smol.GlobalTransform.origin + direction, Vector3.Up);
        Smol.GlobalTranslate(direction * (distanceToMousePosition <= MaxRange ? distanceToMousePosition : MaxRange ));
    }
}
