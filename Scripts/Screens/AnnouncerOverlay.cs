using Godot;
using System;

public class AnnouncerOverlay : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

    }

    public void ShowMessage(string message, float time = 2)
    {
        GetNode<Label>("MarginContainer/MarginContainer/Message").Text = message;

        var messageTimer = GetNode<Timer>("MessageTimer");
        messageTimer.WaitTime = time;
        messageTimer.Start();

        Visible = true;
    }

    public void OnMessageTimerTimeout()
    {

        Visible = false;
    }
}
