using Godot;
using System;

public abstract class Ability : Node
{
    public Smol Smol;
    
    [Export]
    public float Cooldown = 1f;
    
    // Called when the node enters the scene tree for the first time.
    public void Init(Smol smol) {
        Smol = smol;
    }

    public abstract void Cast();
}
