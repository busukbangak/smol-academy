using Godot;
using System;

public class GameManager : Node
{
    public static GameManager Instance;

    public bool IsGameStarted = false;

    private float elapsedTime = 0f;

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


        DebugManager.Add("GameTime", this, nameof(ElapsedTimeToString), true);
    }

    public override void _Process(float delta)
    {
        if (!IsGameStarted) return;

        elapsedTime += delta;
    }

    public void StartGame()
    {
        IsGameStarted = true;
    }

    public void ExitGame()
    {
        IsGameStarted = false;
    }

    public string ElapsedTimeToString()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
}
