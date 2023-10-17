using Godot;
using System;

public class ShotAbility : Ability
{

    [Export]
    public float MaxRange = 25f;

    [Export]
    public PackedScene Projectile;

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
                var newMousePosition = (Vector3)CursorManager.Instance.MouseRaycast()["position"];

                var shot = Projectile.Instance<Shot>();
                Smol.GetParent().AddChild(shot);
                shot.Attacker = Smol;
                shot.GlobalTranslation = new Vector3(Smol.Translation.x, Smol.Translation.y + 2, Smol.Translation.z);
                shot.LookAt(new Vector3(newMousePosition.x, Smol.Translation.y + 2, newMousePosition.z), Vector3.Up);
                shot.InitalPosition = shot.GlobalTranslation;
                shot.MaxRange = MaxRange;
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

    public override void UpdateIndicator()
    {
        base.UpdateIndicator();

        var abilityIndicator = GetNode<Spatial>("AbilityIndicatorContainer");

        var newMousePosition = (Vector3)CursorManager.Instance.MouseRaycast()["position"];

        abilityIndicator.Translation = new Vector3(abilityIndicator.Translation.x, Smol.Translation.y + 0.1f, abilityIndicator.Translation.z);
        abilityIndicator.LookAt(new Vector3(newMousePosition.x, Smol.Translation.y + 0.1f, newMousePosition.z), Vector3.Up);
        abilityIndicator.Scale = new Vector3(abilityIndicator.Scale.x, abilityIndicator.Scale.y, MaxRange / 10);
    }
}
