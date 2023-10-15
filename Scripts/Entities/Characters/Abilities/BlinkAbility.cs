using Godot;
using System;

public class BlinkAbility : Ability
{

    public float MaxRange = 15f;

    public override void _Process(float delta)
    {
        base._Process(delta);
        switch (State)
        {
            case AbilityStates.Disabled: break;
            case AbilityStates.Available: break;
            case AbilityStates.Selected: break;
            case AbilityStates.Casting: break;
            case AbilityStates.Active: break;
            case AbilityStates.Cooldown: break;
        }
    }

    public override void EnterState(AbilityStates state)
    {
        base.EnterState(state);

        switch (State)
        {
            case AbilityStates.Disabled: break;
            case AbilityStates.Available: break;
            case AbilityStates.Selected: break;
            case AbilityStates.Casting: break;
            case AbilityStates.Active:
                var mousePosition = (Vector3)CursorManager.Instance.MouseRaycast()["position"];
                var flooredMousePosition = new Vector3(mousePosition.x, Smol.Translation.y, mousePosition.z);
                var direction = Smol.GlobalTransform.origin.DirectionTo(flooredMousePosition);
                var distanceToMousePosition = Smol.GlobalTransform.origin.DistanceTo(mousePosition);
                Smol.Model.LookAt(Smol.GlobalTransform.origin + direction, Vector3.Up);
                Smol.GlobalTranslate(direction * (distanceToMousePosition <= MaxRange ? distanceToMousePosition : MaxRange));
                Smol.UpdateNavigationPath();
                break;
            case AbilityStates.Cooldown: break;
        }
    }

    public override void ExitState(AbilityStates state)
    {
        base.ExitState(state);
        switch (State)
        {
            case AbilityStates.Disabled: break;
            case AbilityStates.Available: break;
            case AbilityStates.Selected: break;
            case AbilityStates.Casting: break;
            case AbilityStates.Active: break;
            case AbilityStates.Cooldown: break;
        }
    }
}
