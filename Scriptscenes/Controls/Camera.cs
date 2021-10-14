using Godot;
using System;

public class Camera : Godot.Camera
{

    [Export]
    public float MoveMargin = 20f;

    [Export]
    public float MoveSpeed = 30f;

    [Export]
    public float MinZoom = 5f;

    [Export]
    public float MaxZoom = 50f;

    [Export]
    public float ZoomSpeed = 100f;

    [Export]
    public float ZoomSpeedDamping = 0.9f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    public override void _Process(float delta)
    {
        Vector2 mousePosition = GetViewport().GetMousePosition();
        Vector2 viewportSize = GetViewport().Size;
        Vector3 moveVector = new Vector3();

        // TODO: Corner pan
        if (mousePosition.x < MoveMargin)
        {
            moveVector.x--;
        }
        else if (mousePosition.y < MoveMargin)
        {
            moveVector.z--;
        }
        else if (mousePosition.x > viewportSize.x - MoveMargin)
        {
            moveVector.x++;
        }
        else if (mousePosition.y > viewportSize.y - MoveMargin)
        {
            moveVector.z++;
        }

        GlobalTranslate(moveVector * delta * MoveSpeed);


    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("zoom_in"))
        {
            Zoom(-1);
        }

        if (@event.IsActionPressed("zoom_out"))
        {
            Zoom(1);
        }
    }

    public void Zoom(int zoomDirection)
    {
        // TODO: Fix
        var newZoom = Mathf.Clamp(Translation.z + ZoomSpeed * GetProcessDeltaTime() * zoomDirection, MinZoom, MaxZoom);
        Translation = new Vector3(Translation.x, newZoom, newZoom);
    }
}
