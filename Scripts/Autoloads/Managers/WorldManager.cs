using Godot;
using System;

public class WorldManager : Node
{

    public static Node World;

    public static Node CurrentWorldSpace;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Viewport root = GetTree().Root;
        var child = root.GetChild(root.GetChildCount() - 1);

        World = child;
    }

    public override void _Process(float delta)
    {

    }

    public static async void ChangeWorldSpace(string worldSpaceScenePath)
    {

        CurrentWorldSpace?.CallDeferred("queue_free");

        var loadingScreen = (LoadingScreen)UIManager.Add(nameof(Globals.Constants.UI.LOADING_SCREEN), Globals.Constants.UI.LOADING_SCREEN);

        loadingScreen.LoadResource(worldSpaceScenePath);

        var resource = await World.ToSignal(loadingScreen, nameof(LoadingScreen.ResourceLoaded));

        CurrentWorldSpace = (resource[0] as PackedScene).Instance();

        World.CallDeferred("add_child", CurrentWorldSpace);

        UIManager.Remove(nameof(Globals.Constants.UI.LOADING_SCREEN));

        var transitionScreen = (CanvasLayer)UIManager.GetUI(nameof(Globals.Constants.UI.TRANSITION_SCREEN));
        transitionScreen.GetNode<AnimationPlayer>("AnimationPlayer").Play("Dissolve");
    }

    public static void RemoveWorldSpace()
    {
        CurrentWorldSpace?.CallDeferred("queue_free");
        CurrentWorldSpace = null;
    }
}
