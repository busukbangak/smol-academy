using Globals;
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

        var deadOverlay = (Control)UIManager.Add(nameof(Constants.UI.DEAD_OVERLAY), Constants.UI.DEAD_OVERLAY);
        deadOverlay.Visible = false;

        UIManager.Add(nameof(Constants.UI.STATS_OVERLAY), Constants.UI.STATS_OVERLAY);
        UIManager.Add(nameof(Constants.UI.MINIMAP_OVERLAY), Constants.UI.MINIMAP_OVERLAY);
        UIManager.Add(nameof(Constants.UI.ANNOUNCER_OVERLAY), Constants.UI.ANNOUNCER_OVERLAY);
        UIManager.Add(nameof(Constants.UI.PLAYER_OVERLAY), Constants.UI.PLAYER_OVERLAY);
        UIManager.Add(nameof(Constants.UI.ENTITY_OVERLAY), Constants.UI.ENTITY_OVERLAY);

        GetTree().CreateTimer(5f).Connect("timeout", this, nameof(WelcomeMessage));
        GetTree().CreateTimer(5f).Connect("timeout", this, nameof(AddGold));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        elapsedTime += delta;

        if (UIManager.GetUI(nameof(Constants.UI.STATS_OVERLAY)) != null)
        {
            UIManager.GetUI(nameof(Constants.UI.STATS_OVERLAY)).GetNode<Label>("StatsContainer/HBoxContainer/GameStats/Time").Text = ElapsedTimeToString();
            UIManager.GetUI(nameof(Constants.UI.STATS_OVERLAY)).GetNode<Label>("StatsContainer/HBoxContainer/MinionCounter/Label").Text = PlayerData.Player.MinionKills.ToString();
        }

        if (UIManager.GetUI(nameof(Constants.UI.DEAD_OVERLAY)) != null)
        {
            if ((UIManager.GetUI(nameof(Constants.UI.DEAD_OVERLAY)) as Control).Visible)
            {
                UIManager.GetUI(nameof(Constants.UI.DEAD_OVERLAY)).GetNode<Label>("CenterContainer/VBoxContainer/Counter").Text = ((int)Math.Ceiling(PlayerData.Player.RespawnTimer.TimeLeft)).ToString();
            }
        }


    }
    
    public void WelcomeMessage()
    {
        (UIManager.GetUI(nameof(Constants.UI.ANNOUNCER_OVERLAY)) as AnnouncerOverlay).ShowMessage("Welcome To LANE1");
    }

    public void SpawnPlayer()
    {
        var player = PlayerData.AssignedSmol.Instance<Smol>();
        PlayerData.Player = player;
        player.AssignedTeam = PlayerData.AssignedTeam;
        player.Gold = 400;
        player.Connect(nameof(Entity.Respawned), this, nameof(OnPlayerRespawn));
        player.Connect(nameof(Entity.Die), this, nameof(OnPlayerDie));
        player.Connect(nameof(Entity.Kill), this, nameof(OnPlayerKill));

        GetNode<Navigation>("Navigation").AddChild(player);
        player.GlobalTranslation = GetNode<Spatial>(player.AssignedTeam == TeamColor.Blue ? "Navigation/NavigationMeshInstance/BlueSpawn" : "Navigation/NavigationMeshInstance/RedSpawn").GlobalTranslation;

        var camera = GetNode<Camera>("Camera");
        camera.GlobalTranslation = player.GlobalTranslation;
        camera.Translate(new Vector3(0, 0, -15));
    }

    public void OnPlayerRespawn(Smol smol)
    {
        (UIManager.GetUI(nameof(Constants.UI.DEAD_OVERLAY)) as Control).Visible = false;
        smol.GlobalTranslation = GetNode<Spatial>(smol.AssignedTeam == TeamColor.Blue ? "Navigation/NavigationMeshInstance/BlueSpawn" : "Navigation/NavigationMeshInstance/RedSpawn").GlobalTranslation;
    }

    public void OnPlayerDie(Smol smol)
    {
        (UIManager.GetUI(nameof(Constants.UI.ANNOUNCER_OVERLAY)) as AnnouncerOverlay).ShowMessage("An Ally has been slain");
        (UIManager.GetUI(nameof(Constants.UI.DEAD_OVERLAY)) as Control).Visible = true;
        UIManager.GetUI(nameof(Constants.UI.STATS_OVERLAY)).GetNode<Label>("StatsContainer/HBoxContainer/PlayerStats/KDAContainer/Deaths").Text = smol.Deaths.ToString();
    }

    public void OnPlayerKill(Entity entity, Entity killedEntity)
    {
        if (killedEntity is Smol)
        {
            (UIManager.GetUI(nameof(Constants.UI.ANNOUNCER_OVERLAY)) as AnnouncerOverlay).ShowMessage("An Enemy has been slain");
        }
        else if (killedEntity is Turret)
        {
            (UIManager.GetUI(nameof(Constants.UI.ANNOUNCER_OVERLAY)) as AnnouncerOverlay).ShowMessage("Turret has been destroyed");
        }

        UIManager.GetUI(nameof(Constants.UI.STATS_OVERLAY)).GetNode<Label>("StatsContainer/HBoxContainer/MinionCounter/Label").Text = entity.MinionKills.ToString();
        UIManager.GetUI(nameof(Constants.UI.STATS_OVERLAY)).GetNode<Label>("StatsContainer/HBoxContainer/PlayerStats/KDAContainer/Kills").Text = entity.Kills.ToString();

    }

    public void OnNexusDestroyed(Entity entity)
    {
        Node endgameOverlay;
        if (entity.AssignedTeam == PlayerData.Player.AssignedTeam)
        {
            endgameOverlay = UIManager.Add(nameof(Constants.UI.DEFEAT_OVERLAY), Constants.UI.DEFEAT_OVERLAY);
        }
        else
        {
            endgameOverlay = UIManager.Add(nameof(Constants.UI.VICTORY_OVERLAY), Constants.UI.VICTORY_OVERLAY);
        }

        endgameOverlay.GetNode<CustomButton>("CenterContainer/VBoxContainer/CustomButton").Connect(nameof(CustomButton.LeftButtonPressed), this, nameof(OnGameExit));
    }

    public async void OnGameExit()
    {
        var transitionScreen = (CanvasLayer)UIManager.GetUI(nameof(Constants.UI.TRANSITION_SCREEN));
        transitionScreen.GetNode<AnimationPlayer>("AnimationPlayer").PlayBackwards("Dissolve");
        await ToSignal(transitionScreen.GetNode<AnimationPlayer>("AnimationPlayer"), "animation_finished");

        WorldManager.RemoveWorldSpace();

        UIManager.Remove(nameof(Constants.UI.DEFEAT_OVERLAY));
        UIManager.Remove(nameof(Constants.UI.VICTORY_OVERLAY));
        UIManager.Remove(nameof(Constants.UI.STATS_OVERLAY));
        UIManager.Remove(nameof(Constants.UI.DEAD_OVERLAY));
        UIManager.Remove(nameof(Constants.UI.MINIMAP_OVERLAY));
        UIManager.Remove(nameof(Constants.UI.ANNOUNCER_OVERLAY));
        UIManager.Remove(nameof(Constants.UI.PLAYER_OVERLAY));
        UIManager.Remove(nameof(Constants.UI.ENTITY_OVERLAY));

        UIManager.Add(nameof(Constants.UI.MAIN_SCREEN), Constants.UI.MAIN_SCREEN);
    }

    public void AddGold()
    {
        PlayerData.Player.Gold += 2.4f;
        GetTree().CreateTimer(1f).Connect("timeout", this, nameof(AddGold));
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
