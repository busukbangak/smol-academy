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

    [Export]
    public Texture Icon;

    [Export]
    public string Title;

    [Export]
    public string Description;

    [Export]
    public int Level = 1;

    [Export]
    public float CastTime = 0.5f;

    [Export]
    public float ActiveTime = 0.1f;

    [Export]
    public float CooldownTime = 5f;

    public Timer CastTimer;

    public Timer ActiveTimer;

    public Timer CooldownTimer;

    public AbilityStates State = AbilityStates.Available;

    public string AbilityButtonMappingString;

    public override void _Ready()
    {
        CastTimer = new Timer();
        CastTimer.WaitTime = CastTime;
        CastTimer.OneShot = true;
        AddChild(CastTimer);

        ActiveTimer = new Timer();
        ActiveTimer.WaitTime = ActiveTime;
        ActiveTimer.OneShot = true;
        AddChild(ActiveTimer);

        CooldownTimer = new Timer();
        CooldownTimer.WaitTime = CooldownTime;
        CooldownTimer.OneShot = true;
        AddChild(CooldownTimer);

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
                break;
            case AbilityStates.Casting:
                break;
            case AbilityStates.Active: break;
            case AbilityStates.Cooldown: break;

        }
    }

    public override void _Notification(int what)
    {
        // Handle notifications
        if (what == NotificationPredelete)
        {
            DebugManager.Remove(nameof(AbilityButtonMappingString));
        }
    }

    public void ChangeState(AbilityStates state)
    {
        ExitState(State);
        State = state;
        EnterState(State);
    }

    public void EnterState(AbilityStates state)
    {
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

    public void ExitState(AbilityStates state)
    {
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

    public virtual void Select()
    {
        ChangeState(AbilityStates.Selected);
    }

    public virtual void Cast()
    {
        CooldownTimer.Start();
    }

    public void DisplayIndicator()
    {

    }

    public void ShowIndicator()
    {

    }

    public void HideIndicator()
    {

    }

    public string StateToString()
    {
        return State.ToString();
    }
}
