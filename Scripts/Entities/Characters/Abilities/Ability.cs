using Godot;
using System;

public class Ability : Node
{
    public Smol Smol;

    [Export]
    public string Title;

    [Export]
    public Texture Icon;

    [Export]
    public int Level = 1;

    [Export]
    public float Cooldown = 1f;

    public Timer CooldownTimer;

    // TODO: Rework Ability System
    public void Init(Smol smol)
    {
        Smol = smol;
        CooldownTimer = new Timer();
        CooldownTimer.WaitTime = Cooldown;
        CooldownTimer.OneShot = true;
        Smol.AddChild(CooldownTimer);
    }

    public virtual void Cast()
    {
        CooldownTimer.Start();
    }
}
