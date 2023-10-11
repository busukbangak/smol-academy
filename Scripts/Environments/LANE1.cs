using Godot;
using System;
using System.Collections.Generic;

public class LANE1 : Node
{
    private float elapsedTime = 0f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SpawnPlayer();
        DebugManager.Add(nameof(elapsedTime), this, nameof(ElapsedTimeToString), true);


        UIManager.Add(nameof(Globals.Constants.Screens.STATS), Globals.Constants.Screens.STATS);
        UIManager.Add(nameof(Globals.Constants.Screens.MINIMAP), Globals.Constants.Screens.MINIMAP);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        elapsedTime += delta;
    }

    public override void _Notification(int what)
    {
        // Handle notifications
        if (what == NotificationPredelete)
        {
            DebugManager.Remove(nameof(elapsedTime));
        }
    }

    public void SpawnPlayer()
    {
        var player = Globals.PlayerData.AssignedSmol.Instance<Smol>();
        player.AssignedTeam = Globals.PlayerData.AssignedTeam;
        player.Connect(nameof(Entity.Respawned), this, nameof(RespawnPlayer));

        GetNode<Navigation>("Navigation").AddChild(player);
        player.GlobalTranslation = GetNode<Spatial>(player.AssignedTeam == TeamColor.Blue ? "Navigation/NavigationMeshInstance/BlueSpawn" : "Navigation/NavigationMeshInstance/RedSpawn").GlobalTranslation;

        var camera = GetNode<Camera>("Camera");
        camera.GlobalTranslation = player.GlobalTranslation;
        camera.Translate(new Vector3(0, 0, -15));
    }

    public void RespawnPlayer(Smol smol)
    {
        smol.GlobalTranslation = GetNode<Spatial>(smol.AssignedTeam == TeamColor.Blue ? "Navigation/NavigationMeshInstance/BlueSpawn" : "Navigation/NavigationMeshInstance/RedSpawn").GlobalTranslation;
    }

    public void OnNexusDestroyed(TeamColor teamColor)
    {
        WorldManager.RemoveWorldSpace();
        UIManager.Add(nameof(Globals.Constants.Screens.MAIN), Globals.Constants.Screens.MAIN);
    }

    public void OnBlueNexusDie()
    {
        OnNexusDestroyed(TeamColor.Blue);
    }

    public void OnRedNexusDie()
    {
        OnNexusDestroyed(TeamColor.Red);
    }

    public string ElapsedTimeToString()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        return $"{minutes:00}:{seconds:00}";
    }

    public int ElapsedTimeSeconds()
    {
        return Mathf.FloorToInt(elapsedTime % 60f);
    }
}
