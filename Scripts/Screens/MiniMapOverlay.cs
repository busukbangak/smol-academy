using Godot;
using System;

public class MiniMapOverlay : CanvasLayer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var playerCameraPosition = GetViewport().GetCamera().GetParent<Spatial>().Translation;
        var camera = GetNode<Spatial>("ViewportContainer/Viewport/MapCamera");
        camera.Translation = new Vector3(playerCameraPosition.x, 30, playerCameraPosition.z);
    }
}
