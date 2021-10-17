using Godot;
using System;

public class Turret : Entity
{

    public override void _Ready()
    {
        base._Ready();
        ChangeTurretColor(AssignedTeam == TeamColor.Blue ? Colors.SteelBlue : Colors.OrangeRed);
    }

    private void ChangeTurretColor(Color color)
    {
        //TODO: Need to duplicate material and set it, because jsut creating the new one will replace it
        SpatialMaterial spatialMaterial = new SpatialMaterial();
        spatialMaterial.AlbedoColor = color;

        Model.GetNode<CSGMesh>("Turret/Bot").Set("material", spatialMaterial);
    }

    public override void OnDead()
    {
        base.OnDead();
    }
}