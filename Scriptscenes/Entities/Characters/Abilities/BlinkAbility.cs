using Godot;
using System;

public class BlinkAbility : Ability
{


    public override void Cast()
    {
        var hit = Utilities.MouseRaycast(Smol.GetViewport().GetCamera());
        var direction = Smol.GlobalTransform.origin.DirectionTo((Vector3)hit["position"]);
        Smol.Model.LookAt(direction, Vector3.Up);
        Smol.GlobalTranslate(direction * 15);
    }
}
