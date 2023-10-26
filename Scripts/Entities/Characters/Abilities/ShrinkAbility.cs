using Godot;
using System;

public class ShrinkAbility : Ability
{
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
            case AbilityStates.Selected:
                ChangeState(AbilityStates.Active);
                break;
            case AbilityStates.Casting: break;
            case AbilityStates.Active:
                Smol.Scale /= 2;
                Smol.AttackDamage /= 2;
                Smol.MoveSpeed *= 2;
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
            case AbilityStates.Active:
                Smol.Scale *= 2;
                Smol.AttackDamage *= 2;
                Smol.MoveSpeed /= 2;
                break;
            case AbilityStates.Cooldown: break;
        }
    }

    public override void UpdateIndicator()
    {
        base.UpdateIndicator();
    }
}
