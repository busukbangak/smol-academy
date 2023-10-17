using Godot;
using System;

public enum AbilityIndicators
{
    Arrow,
    Cone,
    Heal,
    Range
}

public enum AbilityStates
{
    Disabled,
    Available,
    Selected,
    Casting,
    Active,
    Cooldown
}

public class Ability : Node
{
    public Smol Smol;

    [Signal]
    public delegate void AbilitySelected(Ability selectedAbility);

    [Export]
    public Texture Icon;

    [Export]
    public string Title;

    [Export]
    public string Description;

    [Export]
    public int Level = 1;

    public Timer CastTimer;

    public Timer ActiveTimer;

    public Timer CooldownTimer;

    public AbilityStates State;

    public string AbilityButtonMappingString;

    public override void _Ready()
    {
        State = AbilityStates.Available;
        CastTimer = GetNode<Timer>("CastTimer");
        ActiveTimer = GetNode<Timer>("ActiveTimer");
        CooldownTimer = GetNode<Timer>("CooldownTimer");
        DebugManager.Add(AbilityButtonMappingString, this, nameof(StateToString), true);
    }

    public override void _Process(float delta)
    {
        switch (State)
        {
            case AbilityStates.Disabled: break;
            case AbilityStates.Available:
                if (Input.IsActionJustPressed(AbilityButtonMappingString))
                {
                    ChangeState(AbilityStates.Selected);
                    return;
                }
                break;
            case AbilityStates.Selected:
                if (Input.IsActionJustPressed("select"))
                {
                    ChangeState(AbilityStates.Casting);
                }

                if (Input.IsActionJustPressed("target"))
                {
                    ChangeState(AbilityStates.Available);
                }

                UpdateIndicator();
                break;
            case AbilityStates.Casting: break;
            case AbilityStates.Active: break;
            case AbilityStates.Cooldown: break;

        }
    }

    public override void _Notification(int what)
    {
        // Handle notifications
        if (what == NotificationPredelete)
        {
            DebugManager.Remove(AbilityButtonMappingString);
        }
    }

    public void ChangeState(AbilityStates state)
    {
        ExitState(State);
        State = state;
        EnterState(State);
    }

    public virtual void EnterState(AbilityStates state)
    {
        switch (State)
        {
            case AbilityStates.Disabled: break;
            case AbilityStates.Available: break;
            case AbilityStates.Selected:
                ShowIndicator();
                EmitSignal(nameof(AbilitySelected), this);
                break;
            case AbilityStates.Casting:
                CastTimer.Start();
                break;
            case AbilityStates.Active:
                ActiveTimer.Start();
                break;
            case AbilityStates.Cooldown:
                CooldownTimer.Start();
                break;

        }
    }

    public virtual void ExitState(AbilityStates state)
    {
        switch (State)
        {
            case AbilityStates.Disabled: break;
            case AbilityStates.Available: break;
            case AbilityStates.Selected:
                HideIndicator();
                break;
            case AbilityStates.Casting: break;
            case AbilityStates.Active: break;
            case AbilityStates.Cooldown: break;

        }
    }

    public virtual void Select()
    {
        ChangeState(AbilityStates.Selected);
    }

    public void OnCastTimerTimeout()
    {
        ChangeState(AbilityStates.Active);
    }

    public void OnActiveTimerTimeout()
    {
        ChangeState(AbilityStates.Cooldown);
    }

    public void OnCooldownTimerTimeout()
    {
        ChangeState(AbilityStates.Available);
    }

    public virtual void UpdateIndicator()
    {
        var abilityIndicator = GetNode<Spatial>("AbilityIndicatorContainer");
        abilityIndicator.Translation = Smol.Translation;
    }

    public void ShowIndicator()
    {
        GetNode<Spatial>("AbilityIndicatorContainer").Visible = true;
    }

    public void HideIndicator()
    {
        GetNode<Spatial>("AbilityIndicatorContainer").Visible = false;
    }

    public string StateToString()
    {
        return State.ToString();
    }
}
