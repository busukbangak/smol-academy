using Godot;
using System;

public class TransitionScreen : CanvasLayer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    public void OnAnimationPlayerAnimationFinished(string animation)
    {
        Visible = false;
    }

    public void OnAnimationPlayerAnimationStarted(string animation)
    {
        Visible = true;
    }
}
