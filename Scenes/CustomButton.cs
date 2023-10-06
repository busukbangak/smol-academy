using Godot;
using System;

public class CustomButton : TextureRect
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Modulate = Colors.Black;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventScreenDrag screenDrag)
        {
            /* GD.Print("Drag", screenDrag.Position); */
            /* GD.Print(GetRect().HasPoint(screenDrag.Position)); */
            CursorManager.Instance.IsHoveringUI = GetRect().HasPoint(screenDrag.Position);
        }

        if (@event is InputEventScreenTouch screenTouch)
        {

            if (GetRect().HasPoint(screenTouch.Position))
            {
                if (Input.IsActionPressed("select"))
                {
                    GD.Print("Left Button Down");
                    Modulate = Colors.WhiteSmoke;
                }

                if (Input.IsActionJustReleased("select"))
                {
                    GD.Print("Left Button Up");
                    Modulate = Colors.Black;
                }

                if (Input.IsActionPressed("target"))
                {
                    Modulate = Colors.WhiteSmoke;
                    GD.Print("Right Button Down");
                }

                if (Input.IsActionJustReleased("target"))
                {
                    GD.Print("Right Button Up");
                    Modulate = Colors.Black;
                }
            }
        }
    }
}

