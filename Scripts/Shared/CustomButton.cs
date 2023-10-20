using Godot;
using System;

public class CustomButton : TextureRect
{
    [Signal]
    public delegate void LeftButtonPressed();

    [Signal]
    public delegate void RightButtonPressed();

    [Signal]
    public delegate void IsToggled(bool isActivated);

    [Export]
    public Texture DefaultTexture;

    [Export]
    public Texture PressedTexture;

    [Export]
    public Texture HoveredTexture;

    [Export]
    public bool IsToggleMode = false;

    public bool IsActivated { get; private set; }

    [Export]
    public bool IsDisabled;

    private bool _wasLeftClickPressedInside = false;

    private bool _wasLeftClickPressedOutside = false;

    private bool _wasRightClickPressedInside = false;

    private bool _wasRightClickPressedOutside = false;

    private bool _isHovered = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        IsActivated = false;
        IsDisabled = false;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (IsDisabled)
        {
            Modulate = new Color(0.5f, 0.5f, 0.5f);
        }
        else
        {
            Modulate = new Color(1, 1, 1);
        }
    }


    public override void _Input(InputEvent @event)
    {
        if (IsDisabled) return;

        if (@event is InputEventScreenDrag screenDrag)
        {
            _isHovered = GetGlobalRect().HasPoint(screenDrag.Position);
            CursorManager.Instance.IsHoveringUI = _isHovered;

            if (!_wasLeftClickPressedInside && !IsActivated)
            {
                if (_isHovered && Texture != HoveredTexture && !(_wasRightClickPressedInside || _wasRightClickPressedOutside))
                {
                    Texture = HoveredTexture;
                }

                if (!_isHovered && Texture == HoveredTexture)
                {
                    Texture = DefaultTexture;
                }
            }


        }

        if (@event is InputEventScreenTouch screenTouch)
        {

            if (_isHovered)
            {
                if (Input.IsActionPressed("select"))
                {
                    _wasLeftClickPressedInside = true;
                    Texture = PressedTexture;
                    /* GD.Print("Left Button Down"); */
                }

                if (Input.IsActionJustReleased("select"))
                {
                    if (_wasLeftClickPressedInside)
                    {
                        EmitSignal(nameof(LeftButtonPressed));
                        if (IsToggleMode)
                        {
                            SetIsActivated(!IsActivated);
                            EmitSignal(nameof(IsToggled), IsActivated);
                            /* GD.Print("TOGGLED: ", IsActivated); */
                        }

                        /*    GD.Print("Pressed"); */
                    }

                    _wasLeftClickPressedInside = false;
                    _wasLeftClickPressedOutside = false;
                    /*      GD.Print("Left Button Up"); */

                    if (!IsToggleMode)
                    {
                        Texture = HoveredTexture;
                    }
                }

                if (Input.IsActionPressed("target"))
                {
                    _wasRightClickPressedInside = true;
                    /*  GD.Print("Right Button Down"); */
                }

                if (Input.IsActionJustReleased("target"))
                {
                    _wasRightClickPressedInside = false;
                    _wasRightClickPressedOutside = false;
                    EmitSignal(nameof(RightButtonPressed));
                    /*  GD.Print("Right Button Up");
                     GD.Print("Pressed"); */
                }
            }
            else
            {
                if (_wasLeftClickPressedInside || _wasLeftClickPressedOutside)
                {
                    _wasLeftClickPressedInside = false;
                    _wasLeftClickPressedOutside = false;

                    if (!IsToggleMode)
                    {
                        Texture = DefaultTexture;
                    }

                    /*        GD.Print("Left Button Up"); */
                }

                if (Input.IsActionPressed("select"))
                {
                    _wasLeftClickPressedOutside = true;
                }

                if (_wasRightClickPressedInside || _wasRightClickPressedOutside)
                {
                    _wasRightClickPressedInside = false;
                    _wasRightClickPressedOutside = false;
                    Texture = DefaultTexture;
                    /* GD.Print("Right Button Up"); */
                }

                if (Input.IsActionPressed("target"))
                {
                    _wasRightClickPressedOutside = true;
                }
            }
        }
    }

    public void SetIsActivated(bool isActivated)
    {
        IsActivated = isActivated;

        if (isActivated)
        {
            Texture = PressedTexture;
        }
        else
        {
            if (_isHovered)
            {
                Texture = HoveredTexture;
            }
            else
            {
                Texture = DefaultTexture;
            }

        }
    }

    public void SetIsDisabled(bool isDisabled)
    {
        IsDisabled = isDisabled;
        Texture = DefaultTexture;
    }
}

