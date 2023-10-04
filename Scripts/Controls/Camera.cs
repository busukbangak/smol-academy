using Godot;
using System;

// TODO: Corner pan movement
public class Camera : Spatial
{

    [Export]
    public float MoveMargin = 20f;

    [Export]
    public float MoveSpeed = 130f;

    [Export]
    public float MinZoom = 0.5f;

    [Export]
    public float MaxZoom = 1.2f;

    [Export]
    public float ZoomSpeed = 0.08f;

    private float _currentZoom = 1f;

    private bool _isCameraLocked = false;

    private float _minX = -100f;
    private float _maxX = 100f;
    private float _minZ = -100f;
    private float _maxZ = 100f;

    public override void _Process(float delta)
    {
        Vector2 mousePosition = GetViewport().GetMousePosition();
        Vector2 viewportSize = GetViewport().Size;
        Vector3 moveVector = new Vector3();

        // LOCKED CAMERA
        if (_isCameraLocked)
        {
            var playerPosition = GetNode<Smol>("../Navigation/Smol").GlobalTranslation;
            GlobalTranslation = new Vector3(playerPosition.x, GlobalTranslation.y, playerPosition.z - 10);
        }

        // CAMERA MOVEMENT
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
            Mathf.Clamp(newCameraPosition.x, _minX / _currentZoom, _maxX / _currentZoom),
            newCameraPosition.y,
            Mathf.Clamp(newCameraPosition.z, _minZ / _currentZoom, _maxZ / _currentZoom)
        );

        // ZOOM
        Scale = Scale.LinearInterpolate(Vector3.One * _currentZoom, ZoomSpeed);

    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("zoom_in"))
        {
            _currentZoom -= ZoomSpeed;
        }

        if (@event.IsActionPressed("zoom_out"))
        {
            _currentZoom += ZoomSpeed;
        }

        if (@event.IsActionPressed("ui_accept"))
        {
            _isCameraLocked = true;
        }

        if (@event.IsActionReleased("ui_accept"))
        {
            _isCameraLocked = false;
        }

        _currentZoom = Mathf.Clamp(_currentZoom, MinZoom, MaxZoom);
    }
}
