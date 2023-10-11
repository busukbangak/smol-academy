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

        UIManager.Add(nameof(Globals.Constants.UI.MAIN_SCREEN), Globals.Constants.UI.MAIN_SCREEN);
        UIManager.Add(nameof(Globals.Constants.UI.TRANSITION_SCREEN), Globals.Constants.UI.TRANSITION_SCREEN);
    }

    public override void _Process(float delta)
    {

    }
}
