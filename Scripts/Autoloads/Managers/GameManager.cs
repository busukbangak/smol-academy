using Godot;
using System;

public class GameManager : Node
{
    public static GameManager Instance;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (Instance != null && Instance != this)
        {
            QueueFree();
        }
        else
        {
            Instance = this;
        }

    }

    public override void _Process(float delta)
    {
    }
}
