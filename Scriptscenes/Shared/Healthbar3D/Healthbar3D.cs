using Godot;
using System;

public class Healthbar3D : Spatial
{

    private ProgressBar _healthbar;

    public override void _Ready()
    {
        _healthbar = GetNode<ProgressBar>("Viewport/Healthbar");
    }

    public void OnEntityUpdateHealthbar(float health, float maxHealth)
    {
        _healthbar.MaxValue = maxHealth;
        _healthbar.Value = health;

        Visible = health <= 0 ? false : true;
    }
}
