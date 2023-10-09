using Godot;
using System;

public class WorldManager : Node
{

    public static Node World;

    public static Node CurrentWorldSpace;

    private static ResourceInteractiveLoader _resourceInteractiveLoader;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Viewport root = GetTree().Root;
        var child = root.GetChild(root.GetChildCount() - 1);

        World = child;
    }

    public override void _Process(float delta)
    {
        if (_resourceInteractiveLoader == null) return;

        var error = _resourceInteractiveLoader.Poll();

    }

    public static async void ChangeWorldSpace(string worldSpaceScenePath)
    {

        CurrentWorldSpace?.CallDeferred("queue_free");

        var loadingScreen = (LoadingScreen)UIManager.Add(nameof(Globals.Constants.Screens.LOADING), Globals.Constants.Screens.LOADING);

        loadingScreen.LoadResource(worldSpaceScenePath);

        var resource = await World.ToSignal(loadingScreen, nameof(LoadingScreen.ResourceLoaded));

        CurrentWorldSpace = (resource[0] as PackedScene).Instance();

        World.CallDeferred("add_child", CurrentWorldSpace);

        UIManager.Remove(nameof(Globals.Constants.Screens.LOADING));
    }

    public static void RemoveWorldSpace()
    {
        CurrentWorldSpace?.CallDeferred("queue_free");
    }
}
