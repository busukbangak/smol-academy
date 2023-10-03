using Godot;
using System;

// TODO: Make boundaries dependeten on Zoom Level
// BUG: CAMERA LOCK
// BUG: ZOOM
public class Camera : Godot.Camera
{

    [Export]
    public float MoveMargin = 20f;

    [Export]
    public float MoveSpeed = 130f;

    [Export]
    public float MinZoom = 1;

    [Export]
    public float MaxZoom = 25f;

    [Export]
    public float ZoomSpeed = 200f;

    [Export]
    public float ZoomSpeedDamping = 0.9f;

    private float _currentZoom = 10;

    /* private bool _isCameraLocked = false; */

    private float _minX = -50f;
    private float _maxX = 50f;
    private float _minZ = -50f;
    private float _maxZ = 100f;

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


        var newCameraPosition = GlobalTranslation + (moveVector * delta * MoveSpeed);
        GlobalTranslation = new Vector3(
            Mathf.Clamp(newCameraPosition.x, _minX, _maxX),
            newCameraPosition.y,
            Mathf.Clamp(newCameraPosition.z, _minZ, _maxZ)
        );

        /* if (_isCameraLocked)
        {
            var playerPosition = GetNode<Spatial>("../Navigation/Smol").GlobalTranslation;
            GlobalTranslation = new Vector3(playerPosition.x, GlobalTranslation.y, playerPosition.z + 15);
        } */
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

       /*  if (@event.IsActionPressed("ui_accept"))
        {
            _isCameraLocked = true;
        }

        if (@event.IsActionReleased("ui_accept"))
        {
            _isCameraLocked = false;
        } */
    }

    public void Zoom(int zoomDirection)
    {
        if (_currentZoom + zoomDirection > MaxZoom || _currentZoom + zoomDirection < MinZoom) return;

        _currentZoom += zoomDirection;
        var newZoom = Translation.z + (zoomDirection * GetProcessDeltaTime() * ZoomSpeed);

        Translation = new Vector3(
            Translation.x,
            newZoom,
            newZoom
        );
    }
}
