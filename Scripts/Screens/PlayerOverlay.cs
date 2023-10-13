
using Godot;
using System;

public class PlayerOverlay : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Globals.PlayerData.Player.Connect(nameof(Entity.XPGained), this, nameof(UpdateXPBar));
        UpdateXPBar(Globals.PlayerData.Player);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        GetNode<Label>("MarginContainer/HBoxContainer/Stats/MarginContainer/VBoxContainer/AttackDamageContainer/Label").Text = Globals.PlayerData.Player.AttackDamage.ToString();
        GetNode<Label>("MarginContainer/HBoxContainer/Stats/MarginContainer/VBoxContainer/AttackRangeContainer/Label").Text = Globals.PlayerData.Player.AttackRange.ToString();
        GetNode<Label>("MarginContainer/HBoxContainer/Stats/MarginContainer/VBoxContainer/AttackSpeedContainer/Label").Text = Globals.PlayerData.Player.AttackSpeed.ToString();
        GetNode<Label>("MarginContainer/HBoxContainer/Stats/MarginContainer/VBoxContainer/MovementSpeedContainer/Label").Text = Globals.PlayerData.Player.MoveSpeed.ToString();
        GetNode<ProgressBar>("MarginContainer/HBoxContainer/Status/MarginContainer/VBoxContainer/Healthbar").MaxValue = Globals.PlayerData.Player.MaxHealth;
        GetNode<ProgressBar>("MarginContainer/HBoxContainer/Status/MarginContainer/VBoxContainer/Healthbar").Value = Globals.PlayerData.Player.Health;

        GetNode<Label>("MarginContainer/HBoxContainer/Inventory/MarginContainer/VBoxContainer/GoldContainer/Count").Text = Mathf.Floor(Globals.PlayerData.Player.Gold).ToString();

        var qAbility = GetNode<TextureProgress>("MarginContainer/HBoxContainer/Status/MarginContainer/VBoxContainer/HBoxContainer/AbilityContainer/AbilityOneContainer/Q");
        qAbility.MaxValue = Globals.PlayerData.Player.Abilities[0].CooldownTimer.WaitTime;
        qAbility.Value = Globals.PlayerData.Player.Abilities[0].CooldownTimer.TimeLeft;

        var wAbility = GetNode<TextureProgress>("MarginContainer/HBoxContainer/Status/MarginContainer/VBoxContainer/HBoxContainer/AbilityContainer/AbilityTwoContainer/W");
        wAbility.MaxValue = Globals.PlayerData.Player.Abilities[1].CooldownTimer.WaitTime;
        wAbility.Value = Globals.PlayerData.Player.Abilities[1].CooldownTimer.TimeLeft;

        var eAbility = GetNode<TextureProgress>("MarginContainer/HBoxContainer/Status/MarginContainer/VBoxContainer/HBoxContainer/AbilityContainer/AbilityThreeContainer/E");
        eAbility.MaxValue = Globals.PlayerData.Player.Abilities[2].CooldownTimer.WaitTime;
        eAbility.Value = Globals.PlayerData.Player.Abilities[2].CooldownTimer.TimeLeft;

        var rAbility = GetNode<TextureProgress>("MarginContainer/HBoxContainer/Status/MarginContainer/VBoxContainer/HBoxContainer/AbilityContainer/AbilityFourContainer/R");
        rAbility.MaxValue = Globals.PlayerData.Player.Abilities[3].CooldownTimer.WaitTime;
        rAbility.Value = Globals.PlayerData.Player.Abilities[3].CooldownTimer.TimeLeft;
    }

    public void UpdateXPBar(Entity entity)
    {
        GetNode<TextureProgress>("MarginContainer/HBoxContainer/XP/VBoxContainer/TextureProgress").MaxValue = entity.GetRequiredExperienceForLevel(entity.Level + 1);
        GetNode<TextureProgress>("MarginContainer/HBoxContainer/XP/VBoxContainer/TextureProgress").Value = entity.Experience;
        GetNode<Label>("MarginContainer/HBoxContainer/XP/VBoxContainer/MarginContainer/Label").Text = entity.Level.ToString();
    }
}