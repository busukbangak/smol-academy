using Godot;
using System;

public enum CursorType
{
    Default,
    Attack
}

public class CursorManager : Node
{

    public static CursorManager Instance;

    [Export]
    private Texture _defaultCursorImage;

    [Export]
    private Texture _attackCursorImage;

    private Vector2 _accumulatedMouseMotion = Vector2.Zero;

    private bool _isInteracting = false;

    public Control Cursor;

    public bool IsHoveringUI = false;

    public bool IsHovereringEntity = false;

    public Godot.Collections.Dictionary HoveredObject = new Godot.Collections.Dictionary();

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

        Cursor = GetNode<Control>("Wrapper/Cursor");

        ChangeCursor(CursorType.Default);

        Cursor.Visible = false;
    }


    public override void _Process(float delta)
    {
        if (Input.MouseMode != Input.MouseModeEnum.Captured)
        {
            Cursor.Visible = false;
            _isInteracting = false;
            return;
        }

        Cursor.RectPosition = _accumulatedMouseMotion;

        var hoveredObject = MouseRaycast();

        if (hoveredObject.Count > 0)
        {
            if (hoveredObject["collider"] is Entity entity)
            {
                IsHovereringEntity = true;

                if (HoveredObject.Count == 0)
                {
                    // Enter Mouse on new Hovered Entity
                    entity.OnEntityMouseEntered();
                }
                else if (hoveredObject["collider"] != HoveredObject["collider"])
                {
                    if (HoveredObject["collider"] is Entity)
                    {
                        (HoveredObject["collider"] as Entity).OnEntityMouseExited();
                    }

                    // Enter Mouse on new Hovered Entity
                    entity.OnEntityMouseEntered();
                }

            }
            else
            {
                IsHovereringEntity = false;
                if (HoveredObject.Count > 0)
                {
                    if (HoveredObject["collider"] is Entity)
                    {
                        (HoveredObject["collider"] as Entity).OnEntityMouseExited();
                    }
                }
            }
        }
        else
        {
            IsHovereringEntity = false;
            if (HoveredObject.Count > 0)
            {
                if (HoveredObject["collider"] is Entity)
                {
                    (HoveredObject["collider"] as Entity).OnEntityMouseExited();
                }
            }
        }

        HoveredObject = hoveredObject;
    }

    public override void _Input(InputEvent @event)
    {
        // Check if the input event is a mouse motion event
        if (@event is InputEventMouseMotion mouseMotion && _isInteracting)
        {
            // Get the relative mouse motion from the event
            Vector2 rawMouseMotion = mouseMotion.Relative;

            // Accumulate the mouse motion
            _accumulatedMouseMotion += rawMouseMotion;

            _accumulatedMouseMotion = new Vector2(
                Mathf.Clamp(_accumulatedMouseMotion.x, 0, Cursor.GetViewportRect().Size.x - Cursor.RectSize.x),
                Mathf.Clamp(_accumulatedMouseMotion.y, 0, Cursor.GetViewportRect().Size.y - Cursor.RectSize.y)
            );

            var touch = new InputEventScreenDrag();
            touch.Position = _accumulatedMouseMotion;
            Input.ParseInputEvent(touch);
        }


        if (@event is InputEventMouseButton click)
        {
            if (!_isInteracting)
            {
                _isInteracting = true;
                Input.MouseMode = Input.MouseModeEnum.Captured;
                Cursor.Visible = true;
                // Center first position
                _accumulatedMouseMotion = Cursor.GetViewportRect().Size / 2;
            }

            var touch = new InputEventScreenTouch();
            touch.Position = _accumulatedMouseMotion;
            touch.Pressed = click.Pressed;

            Input.ParseInputEvent(touch);
        }

    }

    public Godot.Collections.Dictionary MouseRaycast()
    {
        if (IsHoveringUI || GetViewport().GetCamera() == null) return new Godot.Collections.Dictionary();

        Vector2 cursorPosition = Cursor.RectPosition;
        var camera = Cursor.GetViewport().GetCamera();
        var from = camera.ProjectRayOrigin(cursorPosition);
        var to = from + camera.ProjectRayNormal(cursorPosition) * 1000;

        var spaceState = camera.GetWorld().DirectSpaceState;
        var result = spaceState.IntersectRay(from, to);

        return result;
    }

    public void ChangeCursor(CursorType cursorType)
    {
        switch (cursorType)
        {
            case CursorType.Default:
                Cursor.GetNode<TextureRect>("TextureRect").Texture = _defaultCursorImage;
                break;
            case CursorType.Attack:
                Cursor.GetNode<TextureRect>("TextureRect").Texture = _attackCursorImage;
                break;
        }
    }


}
