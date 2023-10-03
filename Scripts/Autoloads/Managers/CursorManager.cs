using Godot;
using System;

public enum CursorType
{
    Default,
    Attack
}

public class CursorManager : Node
{
    [Export]
    private Resource _defaultCursorImage;
    private static Resource _staticDefaultCursorImage;

    [Export]
    private Resource _attackCursorImage;
    private static Resource _staticAttackCursorImage;


    public override void _Ready()
    {
        _staticDefaultCursorImage = _defaultCursorImage;
        _staticAttackCursorImage = _attackCursorImage;
        ChangeCursor(CursorType.Default);
        Input.MouseMode = Input.MouseModeEnum.Confined;
    }

    public static void ChangeCursor(CursorType cursorType)
    {
        switch (cursorType)
        {
            case CursorType.Default:
                Input.SetCustomMouseCursor(_staticDefaultCursorImage);
                break;
            case CursorType.Attack:
                Input.SetCustomMouseCursor(_staticAttackCursorImage);
                break;
        }
    }


}
